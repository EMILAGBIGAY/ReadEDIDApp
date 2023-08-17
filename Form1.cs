using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Edid_GUI
{
    public partial class Form1 : Form
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.FromArgb(41, 44, 51), ButtonBorderStyle.Solid);
        }

        public Form1()
        {
            InitializeComponent();
            Reader edid = new Reader();
            edid.Runner();

            ShowEDID.MouseEnter += buttonShow_hover;
            CompareEDID.MouseEnter += buttonCompare_hover;
            button1.MouseEnter += buttonOne_hover;

            ShowEDID.MouseLeave += buttonShowLeave;
            CompareEDID.MouseLeave += buttonCompareLeave;
            button1.MouseLeave += buttonOneLeave;

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

        private void buttonShow_hover(object sender, EventArgs e)
        {
            ShowEDID.BackColor = SystemColors.ButtonHighlight;
        }
        private void buttonCompare_hover(object sender, EventArgs e)
        {
            CompareEDID.BackColor = SystemColors.ButtonHighlight;
        }
        private void buttonOne_hover(object sender, EventArgs e)
        {
            button1.BackColor = SystemColors.ButtonHighlight;
        }

        private void buttonShowLeave(object sender, EventArgs e)
        {
            ShowEDID.BackColor = Color.LightGray;
        }
        private void buttonCompareLeave(object sender, EventArgs e)
        {
            CompareEDID.BackColor = Color.LightGray;
        }
        private void buttonOneLeave(object sender, EventArgs e)
        {
            button1.BackColor = Color.LightGray;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            string filePath = "";
            string fileContent = "";

            Form2 readPN = new Form2();
            readPN.ShowDialog();
            try
            {
                string box = readPN.boxValue();
                string partNum = box.Substring(3, 5);

                APICalls call = new APICalls();
                string dbEDID = await call.getEDID(partNum);
                using (StreamWriter writer = new StreamWriter(fileGlobal.CompareEF))
                {
                    writer.WriteLine(dbEDID);
                }

                //ABCD3KJF12345
                //asd23511gjdfg
                //abc12345EMILA
                //ABCmoney12345

                if (File.Exists(fileGlobal.CompareEF))
                {
                    ScrollableMessageBoxDouble form2 = new ScrollableMessageBoxDouble("Compare EDIDs");

                    process correctDB = new process();
                    using (StreamWriter writer = new StreamWriter(fileGlobal.OutEdid)) { }
                    correctDB.ParseEDID(fileGlobal.CompareEF, fileGlobal.OutEdid);

                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileGlobal.OutEdid);
                    fileContent = File.ReadAllText(fileGlobal.OutEdid);
                    ScrollableMessageBox correctComp = new ScrollableMessageBox(fileGlobal.OutEdid, "Correct EDID");

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
                MessageBox.Show("can't find part number" + ex.Message);
                return;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form4 Login = new Form4();
            Login.ShowDialog();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
            string secondFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DBoutput.txt");

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
