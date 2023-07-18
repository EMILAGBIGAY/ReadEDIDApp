using System.Windows.Forms;

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
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fileGlobal.EDIDInfo.Count; i++)
            {
                string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileGlobal.EDIDInfo[i]);
                if (File.Exists(filePath1))
                {
                    string fileContents1 = File.ReadAllText(filePath1);
                    int num = i + 1;
                    using (var scrollableDialog = new ScrollableMessageBox(filePath1, "EDID Display " + num))
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
            //will take edid from database
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation1.txt");

            string fileContent = File.ReadAllText(filePath);

            Form2 readPN = new Form2();
            readPN.ShowDialog();
            try
            {
                string box = readPN.boxValue();
                string partNum = box.Substring(3, 5);
                //edidinformation1.txt
                if (File.Exists(box) || partNum == "D3KJF")
                {
                    ScrollableMessageBoxDouble form2 = new ScrollableMessageBoxDouble("Compare EDIDs");

                    /*
                    fileGlobal.CompareEF.Add("dbOut.txt");
                    fileGlobal.OutEdid.Add("EDIDdatabase.txt");
                    process correctDB = new process();
                    using (StreamWriter writer = new StreamWriter(fileGlobal.EDIDInfo[fileGlobal.EDIDInfo.Count()-1])) { }
                    correctDB.ParseEDID(fileGlobal.files[fileGlobal.files.Count() - 1], fileGlobal.EDIDInfo[fileGlobal.EDIDInfo.Count() - 1]);

                    */

                    ScrollableMessageBox correctComp = new ScrollableMessageBox(fileContent, "Correct EDID");

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
                MessageBox.Show("can't find part number");
                return;
            }

        }
    }

    public class ScrollableMessageBox : Form
    {
        public string file;
        private RichTextBox messageTextBox;
        public ScrollableMessageBox(string message, string title)
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            // Set the message and title
            messageTextBox.Text = message;
            file = message;
            Text = title;
        }



        private void InitializeComponent()
        {
            messageTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // messageTextBox
            // 
            messageTextBox.Dock = DockStyle.Fill;
            messageTextBox.Multiline = true;
            messageTextBox.ReadOnly = true;
            messageTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            this.Load += highlightFiles;
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
        private void highlightFiles(object sender, EventArgs e)
        {
            string filePath = file;

            try
            {
                string fileContent = File.ReadAllText(filePath);
                messageTextBox.Text = fileContent;

                string keyword = "Pass";
                int index = 0;

                while (index < messageTextBox.TextLength)
                {
                    int keywordIndex = messageTextBox.Find(keyword, index, RichTextBoxFinds.None);

                    if (keywordIndex >= 0)
                    {
                        messageTextBox.Select(keywordIndex - 20, keyword.Length + 21);
                        messageTextBox.SelectionColor = Color.Green;
                        index = keywordIndex + keyword.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                string keyword2 = "Fail";
                int badindex = 0;

                while (index < messageTextBox.TextLength)
                {
                    int keywordIndex = messageTextBox.Find(keyword2, index, RichTextBoxFinds.None);

                    if (keywordIndex >= 0)
                    {
                        messageTextBox.Select(keywordIndex - 20, keyword2.Length + 21);
                        messageTextBox.SelectionColor = Color.Green;
                        index = keywordIndex + keyword.Length;
                    }
                    else
                    {
                        break;
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
    public class ScrollableMessageBoxDouble : Form
    {
        public ScrollableMessageBoxDouble(string title)
        {
            InitializeComponent();
            Text = title;

        }

        private RichTextBox richTextBox1;

        private void InitializeComponent()
        {
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
            string firstFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation1.txt");
            string secondFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EDIDInformation2.txt");

            try
            {
                List<string> firstLines = File.ReadAllLines(firstFilePath).ToList();
                List<string> secondLines = File.ReadAllLines(secondFilePath).ToList();

                richTextBox1.Lines = firstLines.ToArray();

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
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
