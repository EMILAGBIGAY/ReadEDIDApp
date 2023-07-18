using System.Windows.Forms;

namespace Edid_GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            ShowEDID = new Button();
            CompareEDID = new Button();
            SuspendLayout();
            // 
            // ShowEDID
            // 
            ShowEDID.Location = new Point(858, 380);
            ShowEDID.Margin = new Padding(2, 3, 2, 3);
            ShowEDID.Name = "ShowEDID";
            ShowEDID.Size = new Size(556, 295);
            ShowEDID.TabIndex = 0;
            ShowEDID.Text = "Get EDID";
            ShowEDID.UseVisualStyleBackColor = true;
            ShowEDID.Click += button1_Click;
            // 
            // CompareEDID
            // 
            CompareEDID.Location = new Point(858, 759);
            CompareEDID.Margin = new Padding(2, 3, 2, 3);
            CompareEDID.Name = "CompareEDID";
            CompareEDID.Size = new Size(556, 295);
            CompareEDID.TabIndex = 1;
            CompareEDID.Text = "Compare";
            CompareEDID.UseVisualStyleBackColor = true;
            CompareEDID.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2412, 1681);
            Controls.Add(CompareEDID);
            Controls.Add(ShowEDID);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 3, 2, 3);
            Name = "Form1";
            Text = "EDID Validation";
            FormClosing += ApplicationExitHandler;
            ResumeLayout(false);
        }

        public void ApplicationExitHandler(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("no");
                for (int i = 0; i < fileGlobal.files.Count; i++)
                {
                    string filePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileGlobal.files[i]);
                    string filePath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileGlobal.EDIDInfo[i]);
                    if (File.Exists(fileGlobal.files[i]) && File.Exists(fileGlobal.EDIDInfo[i]))
                    {
                        File.Delete(fileGlobal.files[i]);
                        File.Delete(fileGlobal.EDIDInfo[i]);

                    }
                    else
                    {
                        Console.WriteLine("File does not exist.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }
        }

        #endregion

        private Button ShowEDID;
        private Button CompareEDID;
        private VScrollBar vScrollBar1;
    }
}