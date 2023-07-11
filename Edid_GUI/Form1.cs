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
            for (int i = 0; i < fileGlobal.EDIDInfo.Count; i++)
            {
                string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileGlobal.EDIDInfo[i]);
                if (File.Exists(filePath1))
                {
                    string fileContents1 = File.ReadAllText(filePath1);
                    int num = i + 1;
                    using (var scrollableDialog = new ScrollableMessageBox(fileContents1, "EDID Display "+num))
                    {
                        scrollableDialog.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("No EDID Found");
                }
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation1.txt");
            string filePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");

            string file2Content = File.ReadAllText(filePath2);

            Form2 readPN = new Form2();
            readPN.ShowDialog();
            try
            {
                string box = readPN.boxValue();
                string partNum = box.Substring(3, 5);
                //KR0D3KJF71763815E11N
                if (File.Exists(box) || partNum == "D3KJF")
                {
                    ScrollableMessageBoxDouble form2 = new ScrollableMessageBoxDouble(filePath2, filePath2, "Compare EDIDs");
                    ScrollableMessageBox correctComp = new ScrollableMessageBox(file2Content, "Correct EDID");

                    Screen screen = Screen.PrimaryScreen;
                    Rectangle bounds = screen.Bounds;
                    int screenWidth = bounds.Width;
                    int screenHeight = bounds.Height;

                    int formWidth = screenWidth / 2;
                    int formHeight = screenHeight;
                    int form1X = 0;
                    int form1Y = 0;
                    int form2X = formWidth;
                    int form2Y = 0;

                    form2.StartPosition = FormStartPosition.Manual;
                    form2.Location = new Point(form1X, form1Y + 130);
                    form2.Size = new Size(formWidth, formHeight - 340);
                    correctComp.StartPosition = FormStartPosition.Manual;
                    correctComp.Location = new Point(form2X, form2Y + 130);
                    correctComp.Size = new Size(formWidth, formHeight - 340);

                    Task task1 = Task.Run(() => correctComp.ShowDialog());
                    Task task2 = Task.Run(() => form2.ShowDialog());

                    await Task.WhenAll(task1, task2);
                }
                else
                {
                    MessageBox.Show("EDID not found");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bad Data Reading: Re-Enter Data");
                return;
            }
            
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

        }

        //private TableLayoutPanel tableLayoutPanel;
        private RichTextBox richTextBox1;

        private void InitializeComponent()
        {
            //tableLayoutPanel = new TableLayoutPanel();
            richTextBox1 = new RichTextBox();
            
            SuspendLayout();
            
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            Controls.Add(richTextBox1);
            this.Load += CompareFiles;
            
            
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1300, 700);
            Name = "ScrollableMessageBox";
            Text = "Message";
            ResumeLayout(false);
            PerformLayout();
        }

        private void CompareFiles(object sender, EventArgs e)
        {
            string firstFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");
            string secondFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");

            try
            {
                List<string> firstLines = File.ReadAllLines(firstFilePath).ToList();
                List<string> secondLines = File.ReadAllLines(secondFilePath).ToList();

                richTextBox1.Lines = firstLines.ToArray();
                //richTextBox2.Lines = secondLines.ToArray();

                int lineCount = Math.Min(firstLines.Count, secondLines.Count);

                for (int i = 0; i < lineCount; i++)
                {
                    string firstLine = firstLines[i];
                    string secondLine = secondLines[i];

                    int colonIndex1 = firstLine.IndexOf(':');
                    int colonIndex2 = secondLine.IndexOf(':');

                    if (colonIndex1 >= 0 && colonIndex2 >= 0)
                    {
                        string firstText = firstLine.Substring(colonIndex1 + 1).Trim();
                        string secondText = secondLine.Substring(colonIndex2 + 1).Trim();

                        if (firstText != secondText)
                        {
                            int startIndex1 = richTextBox1.GetFirstCharIndexFromLine(i) + colonIndex1 + 1;
                            int length = firstLine.Length - (colonIndex1 + 1);

                            richTextBox1.Select(startIndex1, length);
                            richTextBox1.SelectionColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file reading or comparison
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
