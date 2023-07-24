namespace DiscoverHelper
{
    partial class DiscoverHelper
    {
        private System.ComponentModel.IContainer components = null;
        private Button UpdateDataButton;
        private Label HeaderLabel;
        private Button FindBestChoice;
        private RichTextBox richTextBox1;
        private ProgressBar UpdateProgressBar;
        private TextBox SearchCardTextBox;
        private Label labelCard;
        private Label label1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscoverHelper));
            UpdateDataButton = new Button();
            HeaderLabel = new Label();
            FindBestChoice = new Button();
            richTextBox1 = new RichTextBox();
            UpdateProgressBar = new ProgressBar();
            SearchCardTextBox = new TextBox();
            labelCard = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // UpdateDataButton
            // 
            UpdateDataButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UpdateDataButton.BackColor = Color.DarkRed;
            UpdateDataButton.FlatStyle = FlatStyle.Flat;
            UpdateDataButton.ForeColor = Color.White;
            UpdateDataButton.Location = new Point(847, 143);
            UpdateDataButton.Margin = new Padding(6);
            UpdateDataButton.Name = "UpdateDataButton";
            UpdateDataButton.Size = new Size(191, 60);
            UpdateDataButton.TabIndex = 0;
            UpdateDataButton.Text = "Update Data";
            UpdateDataButton.UseVisualStyleBackColor = false;
            UpdateDataButton.Click += UpdateDataButton_Click;
            // 
            // HeaderLabel
            // 
            HeaderLabel.Anchor = AnchorStyles.Top;
            HeaderLabel.AutoSize = true;
            HeaderLabel.BackColor = Color.Transparent;
            HeaderLabel.Font = new Font("Belwe Bd BT", 13.875F, FontStyle.Regular, GraphicsUnit.Point);
            HeaderLabel.ForeColor = Color.SeaShell;
            HeaderLabel.Location = new Point(281, 41);
            HeaderLabel.Margin = new Padding(6, 0, 6, 0);
            HeaderLabel.Name = "HeaderLabel";
            HeaderLabel.Size = new Size(499, 45);
            HeaderLabel.TabIndex = 1;
            HeaderLabel.Text = "Hearthstone Discover Tool";
            HeaderLabel.TextAlign = ContentAlignment.TopCenter;
            HeaderLabel.Click += HeaderLabel_Click;
            // 
            // FindBestChoice
            // 
            FindBestChoice.BackColor = Color.DarkRed;
            FindBestChoice.FlatStyle = FlatStyle.Flat;
            FindBestChoice.ForeColor = Color.White;
            FindBestChoice.Location = new Point(393, 138);
            FindBestChoice.Margin = new Padding(6);
            FindBestChoice.Name = "FindBestChoice";
            FindBestChoice.Size = new Size(227, 62);
            FindBestChoice.TabIndex = 2;
            FindBestChoice.Text = "Get Discover data";
            FindBestChoice.UseVisualStyleBackColor = false;
            // 
            // richTextBox1
            // 
            richTextBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richTextBox1.BackColor = Color.FromArgb(32, 32, 32);
            richTextBox1.ForeColor = Color.White;
            richTextBox1.Location = new Point(22, 247);
            richTextBox1.Margin = new Padding(6);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(1016, 446);
            richTextBox1.TabIndex = 3;
            richTextBox1.Text = "";
            // 
            // UpdateProgressBar
            // 
            UpdateProgressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            UpdateProgressBar.BackColor = Color.FromArgb(64, 64, 64);
            UpdateProgressBar.ForeColor = Color.DarkRed;
            UpdateProgressBar.Location = new Point(22, 716);
            UpdateProgressBar.Margin = new Padding(6);
            UpdateProgressBar.Name = "UpdateProgressBar";
            UpdateProgressBar.Size = new Size(1016, 49);
            UpdateProgressBar.TabIndex = 4;
            // 
            // SearchCardTextBox
            // 
            SearchCardTextBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            SearchCardTextBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            SearchCardTextBox.BackColor = Color.FromArgb(32, 32, 32);
            SearchCardTextBox.ForeColor = Color.White;
            SearchCardTextBox.Location = new Point(22, 154);
            SearchCardTextBox.Margin = new Padding(6);
            SearchCardTextBox.Name = "SearchCardTextBox";
            SearchCardTextBox.Size = new Size(342, 39);
            SearchCardTextBox.TabIndex = 7;
            // 
            // labelCard
            // 
            labelCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelCard.AutoSize = true;
            labelCard.BackColor = Color.Transparent;
            labelCard.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelCard.ForeColor = Color.White;
            labelCard.Location = new Point(22, 103);
            labelCard.Margin = new Padding(6, 0, 6, 0);
            labelCard.Name = "labelCard";
            labelCard.Size = new Size(196, 45);
            labelCard.TabIndex = 8;
            labelCard.Text = "Search Card:";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.ForeColor = Color.White;
            label1.Location = new Point(915, 772);
            label1.Name = "label1";
            label1.Size = new Size(123, 32);
            label1.TabIndex = 9;
            label1.Text = "by Olanga";
            // 
            // DiscoverHelper
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 28);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1065, 813);
            Controls.Add(label1);
            Controls.Add(labelCard);
            Controls.Add(SearchCardTextBox);
            Controls.Add(UpdateProgressBar);
            Controls.Add(richTextBox1);
            Controls.Add(FindBestChoice);
            Controls.Add(HeaderLabel);
            Controls.Add(UpdateDataButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 2, 4, 2);
            Name = "DiscoverHelper";
            Text = "Discover Helper";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}