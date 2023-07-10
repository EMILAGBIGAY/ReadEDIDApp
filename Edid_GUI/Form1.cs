namespace Edid_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Reader edid = new Reader();
            edid.Runner();
        }

        //show
        private void button1_Click(object sender, EventArgs e)
        {

            string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation1.txt");
            string filePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");

            if (File.Exists(filePath1))
            {
                string fileContents1 = File.ReadAllText(filePath1);

                using (var scrollableDialog = new ScrollableMessageBox(fileContents1, "EDID Display 1"))
                {
                    scrollableDialog.ShowDialog();
                }
                if (File.Exists(filePath2))
                {
                    string fileContents2 = File.ReadAllText(filePath2);
                    using (var scrollableDialog = new ScrollableMessageBox(fileContents2, "EDID Display 2"))
                    {
                        scrollableDialog.ShowDialog();
                    }
                }
            }
            else
            {
                MessageBox.Show("output1.txt");
            }
        }
        public string Compare(string one, string two)
        {
            string[] norm = one.Split('\n');
            string[] red = two.Split('\n');
            Array.Resize(ref red, norm.Length);

            for (int i = 0; i < norm.Length; i++)
            {
                if (norm[i] != red[i])
                {
                    red[i] = $"[red]{red[i]}[/red]";
                }
            }
            string highlightedContent = string.Join("\n", red);
            return highlightedContent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation1.txt");
            string filePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");

            string file1Content = File.ReadAllText(filePath1);
            string file2Content = File.ReadAllText(filePath2);

            //string highlightedContent = Compare(file1Content, file2Content);

            //File.WriteAllText(filePath2, highlightedContent);
            ScrollableMessageBoxDouble form2 = new ScrollableMessageBoxDouble(filePath1, filePath2, "Compare EDIDs");
            //MessageBox.Show(highlightedContent, "Highlighted Differences", MessageBoxButtons.OK, MessageBoxIcon.Information);
            form2.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }

    public class ScrollableMessageBox : Form
    {
        public ScrollableMessageBox(string message, string title)
        {
            InitializeComponent();

            // Set the message and title
            messageTextBox.Text = message;
            Text = title;
        }

        private TextBox messageTextBox;

        private void InitializeComponent()
        {
            messageTextBox = new TextBox();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.Dock = DockStyle.Fill;
            messageTextBox.Multiline = true;
            messageTextBox.ReadOnly = true;
            messageTextBox.ScrollBars = ScrollBars.Vertical;
            // 
            // ScrollableMessageBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(750, 600);
            Controls.Add(messageTextBox);
            Name = "ScrollableMessageBox";
            Text = "Message";
            ResumeLayout(false);
            PerformLayout();
        }
    }
    public class ScrollableMessageBoxDouble : Form
    {
        public ScrollableMessageBoxDouble(string filePath1, string filePath2, string title)
        {
            InitializeComponent();
            Text = title;
            string fileContents1;

            try
            {
                fileContents1 = File.ReadAllText(filePath1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file 1: {ex.Message}");
                return;
            }

            string fileContents2;
            try
            {
                fileContents2 = File.ReadAllText(filePath2);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file 2: {ex.Message}");
                return;
            }

            richTextBox1.Text = fileContents1;
            richTextBox2.Text = fileContents2;
        }

        private TableLayoutPanel tableLayoutPanel;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;

        private void InitializeComponent()
        {
            tableLayoutPanel = new TableLayoutPanel();
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Add(richTextBox1, 0, 0);
            tableLayoutPanel.Controls.Add(richTextBox2, 1, 0);
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            // 
            // richTextBox2
            // 
            richTextBox2.Dock = DockStyle.Fill;
            richTextBox2.ReadOnly = true;
            richTextBox2.ScrollBars = RichTextBoxScrollBars.Vertical;
            // 
            // ScrollableMessageBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1300, 700);
            Controls.Add(tableLayoutPanel);
            Name = "ScrollableMessageBox";
            Text = "Message";
            ResumeLayout(false);
        }
    }

}