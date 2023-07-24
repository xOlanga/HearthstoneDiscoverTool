using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DiscoverHelper
{
    public partial class DiscoverHelper : Form
    {
        private const string JsonFileUrl = "https://api.hearthstonejson.com/v1/latest/enUS/cards.json";

        public DiscoverHelper()
        {
            InitializeComponent();
            UpdateProgressBar.Minimum = 0;
            UpdateProgressBar.Maximum = 100;
            FindBestChoice.Click += FindBestChoiceButton_Click;

        }

        private void FindBestChoiceButton_Click(object sender, EventArgs e)
        {
            string searchText = SearchCardTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                List<FilteredCardData> filteredCards = LoadFilteredCardsFromFile();

                // Search for the card matching the user input
                FilteredCardData card = filteredCards.FirstOrDefault(c => c.name.Equals(searchText, StringComparison.OrdinalIgnoreCase));
                if (card != null)
                {
                    string discoverStats = GetDiscoverStats(card);
                    richTextBox1.Text = discoverStats;
                }
                else
                {
                    richTextBox1.Text = "Card not found.";
                }
            }
            else
            {
                richTextBox1.Clear();
            }
        }




        private List<FilteredCardData> LoadFilteredCardsFromFile()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CardData");
            string filteredCardsFilePath = Path.Combine(folderPath, "filtered_cards.json");

            if (System.IO.File.Exists(filteredCardsFilePath))
            {
                string filteredCardsJson = System.IO.File.ReadAllText(filteredCardsFilePath);
                return JsonConvert.DeserializeObject<List<FilteredCardData>>(filteredCardsJson);
            }

            return new List<FilteredCardData>();
        }


        private void SaveFilteredCardsToJsonFile(List<FilteredCardData> filteredCards)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CardData");
            string filteredCardsFilePath = Path.Combine(folderPath, "filtered_cards.json");

            string filteredCardsJson = JsonConvert.SerializeObject(filteredCards, Formatting.Indented);
            System.IO.File.WriteAllText(filteredCardsFilePath, filteredCardsJson);
        }

        private string GetDiscoverStats(FilteredCardData card)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CardData");
            string setFolderPath = IsStandardSet(card.set) ? "Standard" : "Wild";
            string filePath = Path.Combine(folderPath, setFolderPath, $"{card.name} {card.dbfId}.json");

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                JObject dataObject = JObject.Parse(jsonData);
                JArray cardArray = (JArray)dataObject["series"]["data"]["ALL"];

                // Sort the cards by popularity (descending order)
                cardArray = new JArray(cardArray.OrderByDescending(c => c["popularity"]));

                // Create the formatted string for displaying the Discover stats
                string result = $"Discover stats for card: {card.name}{Environment.NewLine}";
                //result += $"Set: {card.set}{Environment.NewLine}";
                result += Environment.NewLine;

                foreach (JToken cardToken in cardArray)
                {
                    string cardName = cardToken["name"].ToString();
                    double popularity = cardToken["popularity"].ToObject<double>();

                    result += $"{cardName}: ";
                    result += $"{popularity}%{Environment.NewLine}";

                }

                return result;
            }

            return "Discover stats not available for this card.";
        }


        private void UpdateDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                string jsonContent = DownloadJsonFile(JsonFileUrl);
                List<CardData> cards = ParseJsonData(jsonContent);
                List<FilteredCardData> filteredCards = FilterCardsByMechanics(cards, "DISCOVER");
                filteredCards = FilterOutBattlegroundsAndSpecialSets(filteredCards);
                List<AllCardData> allCards = cards.Select(card => new AllCardData
                {
                    name = card.name,
                    dbfId = card.dbfId
                }).ToList();


                SaveAllCardsToFile(allCards);
                SaveFilteredCardsToJsonFile(filteredCards);
                SaveFilteredCardsToFile(filteredCards);

                MessageBox.Show("Data updated and filtered successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SaveAllCardsToFile(List<AllCardData> allCards)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CardData");
            Directory.CreateDirectory(folderPath);

            string allCardsFilePath = Path.Combine(folderPath, "all_cards.json");
            string allCardsJson = JsonConvert.SerializeObject(allCards, Formatting.Indented);
            File.WriteAllText(allCardsFilePath, allCardsJson);
        }

        private List<FilteredCardData> FilterOutBattlegroundsAndSpecialSets(List<FilteredCardData> filteredCards)
        {
            List<FilteredCardData> result = new List<FilteredCardData>();

            foreach (FilteredCardData filteredCard in filteredCards)
            {
                if (!filteredCard.set.Equals("BATTLEGROUNDS", StringComparison.OrdinalIgnoreCase) &&
                    !filteredCard.id.StartsWith("PVPDR", StringComparison.OrdinalIgnoreCase) &&
                    !filteredCard.id.StartsWith("Story", StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(filteredCard);
                }
            }

            return result;
        }

        private string DownloadJsonFile(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        private List<CardData> ParseJsonData(string jsonContent)
        {
            return JsonConvert.DeserializeObject<List<CardData>>(jsonContent);
        }

        private List<FilteredCardData> FilterCardsByMechanics(List<CardData> cards, string targetMechanic)
        {
            List<FilteredCardData> filteredCards = new List<FilteredCardData>();

            foreach (CardData card in cards)
            {
                if (card.mechanics != null && card.mechanics.Contains(targetMechanic))
                {
                    if (card.set != null)
                    {
                        FilteredCardData filteredCard = new FilteredCardData
                        {
                            name = card.name,
                            id = card.id,
                            dbfId = card.dbfId,
                            set = card.set,
                            type = card.type
                        };

                        filteredCards.Add(filteredCard);
                    }
                }
            }

            return filteredCards;
        }


        private void SaveFilteredCardsToFile(List<FilteredCardData> filteredCards)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "CardData");
            Directory.CreateDirectory(folderPath);

            string wildFolderPath = Path.Combine(folderPath, "Wild");
            Directory.CreateDirectory(wildFolderPath);

            string standardFolderPath = Path.Combine(folderPath, "Standard");
            Directory.CreateDirectory(standardFolderPath);

            string allCardsFilePath = Path.Combine(folderPath, "all_cards.json");
            string allCardsJson = File.ReadAllText(allCardsFilePath);
            List<FilteredCardData> allCards = JsonConvert.DeserializeObject<List<FilteredCardData>>(allCardsJson);

            using (WebClient client = new WebClient())
            {
                int totalCards = filteredCards.Count;
                int completedCards = 0;

                foreach (FilteredCardData filteredCard in filteredCards)
                {
                    string cardName = allCards.FirstOrDefault(card => card.dbfId == filteredCard.dbfId)?.name;
                    if (string.IsNullOrEmpty(cardName))
                    {
                        string errorMessage = $"Unable to find card name for dbfId: {filteredCard.dbfId}";
                        richTextBox1.AppendText(errorMessage + Environment.NewLine);
                        continue;
                    }

                    string dbfId = filteredCard.dbfId.ToString();

                    // Save the data for Wild
                    string wildGameType = "RANKED_WILD";
                    string wildUrl = $"https://hsreplay.net/analytics/query/single_card_choices_by_winrate_v2/?GameType={wildGameType}&card_id={dbfId}&RankRange=GOLD";
                    string wildFilePath = Path.Combine(wildFolderPath, $"{cardName} {filteredCard.dbfId}.json");

                    try
                    {
                        string jsonData = client.DownloadString(wildUrl);
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            string updatedJsonData = ReplaceDbfIdWithCardName(jsonData);
                            using (StreamWriter writer = new StreamWriter(wildFilePath, false))
                            {
                                writer.Write(updatedJsonData);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Error downloading data for card: {cardName} ({filteredCard.dbfId}) in Wild";
                        errorMessage += Environment.NewLine + "Error message: " + ex.Message;
                        richTextBox1.AppendText(errorMessage + Environment.NewLine);
                    }

                    // Check if the card is also in the Standard set
                    if (IsStandardSet(filteredCard.set))
                    {
                        // Save the data for Standard
                        string standardGameType = "RANKED_STANDARD";
                        string standardUrl = $"https://hsreplay.net/analytics/query/single_card_choices_by_winrate_v2/?GameType={standardGameType}&card_id={dbfId}&RankRange=GOLD";
                        string standardFilePath = Path.Combine(standardFolderPath, $"{cardName} {filteredCard.dbfId}.json");

                        try
                        {
                            string jsonData = client.DownloadString(standardUrl);
                            if (!string.IsNullOrEmpty(jsonData))
                            {
                                string updatedJsonData = ReplaceDbfIdWithCardName(jsonData);
                                using (StreamWriter writer = new StreamWriter(standardFilePath, false))
                                {
                                    writer.Write(updatedJsonData);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMessage = $"Error downloading data for card: {cardName} ({filteredCard.dbfId}) in Standard";
                            errorMessage += Environment.NewLine + "Error message: " + ex.Message;
                            richTextBox1.AppendText(errorMessage + Environment.NewLine);
                        }
                    }

                    completedCards++;
                    int progress = (int)((float)completedCards / totalCards * 100);
                    UpdateProgressBar.Value = progress;
                }
            }
        }

        private string ReplaceDbfIdWithCardName(string jsonData)
        {
            string allCardsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "CardData", "all_cards.json");
            string allCardsJson = File.ReadAllText(allCardsFilePath);
            List<FilteredCardData> allCards = JsonConvert.DeserializeObject<List<FilteredCardData>>(allCardsJson);

            JObject dataObject = JObject.Parse(jsonData);
            JArray cardArray = (JArray)dataObject["series"]["data"]["ALL"];

            foreach (JToken cardToken in cardArray)
            {
                string dbfId = cardToken["dbf_id"].ToString();
                string cardName = allCards.FirstOrDefault(card => card.dbfId.ToString() == dbfId)?.name;
                if (!string.IsNullOrEmpty(cardName))
                {
                    cardToken["name"] = cardName;
                }
            }

            return dataObject.ToString();
        }

        private bool IsStandardSet(string set)
        {
            List<string> standardSets = new List<string>
            {
                "THE_SUNKEN_CITY",
                "ICECROWN",
                "RETURN_OF_THE_LICH_KING",
                "REVENDRETH",
                "BATTLE_OF_THE_BANDS",
                "CORE"
            };

            return standardSets.Contains(set);
        }

        public class CardData
        {
            public int? armor { get; set; }
            public string? artist { get; set; }
            public string? cardClass { get; set; }
            public bool? collectible { get; set; }
            public int? cost { get; set; }
            public int? dbfId { get; set; }
            public bool? elite { get; set; }
            public string? flavor { get; set; }
            public int? health { get; set; }
            public int? heroPowerDbfId { get; set; }
            public string? id { get; set; }
            public List<string>? mechanics { get; set; }
            public string? name { get; set; }
            public string? rarity { get; set; }
            public List<string>? referencedTags { get; set; }
            public string? set { get; set; }
            public string? text { get; set; }
            public string? type { get; set; }
        }

        public class FilteredCardData
        {
            public string? name { get; set; }
            public string? id { get; set; }
            public int? dbfId { get; set; }
            public string set { get; set; } = string.Empty;
            public string? type { get; set; }
        }

        public class AllCardData
        {
            public string? name { get; set; }
            public int? dbfId { get; set; }
        }

        private void labelCard_Click(object sender, EventArgs e)
        {

        }

        private void HeaderLabel_Click(object sender, EventArgs e)
        {

        }
    }
}