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
            button1 = new Button();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // ShowEDID
            // 
            ShowEDID.AutoSize = true;
            ShowEDID.BackColor = Color.LightGray;
            ShowEDID.Image = (Image)resources.GetObject("ShowEDID.Image");
            ShowEDID.Location = new Point(492, 383);
            ShowEDID.Margin = new Padding(2);
            ShowEDID.Name = "ShowEDID";
            ShowEDID.Size = new Size(523, 276);
            ShowEDID.TabIndex = 0;
            ShowEDID.Text = "Get EDID";
            ShowEDID.TextAlign = ContentAlignment.TopCenter;
            ShowEDID.TextImageRelation = TextImageRelation.TextAboveImage;
            ShowEDID.UseVisualStyleBackColor = false;
            ShowEDID.Click += button1_Click;
            // 
            // CompareEDID
            // 
            CompareEDID.AutoSize = true;
            CompareEDID.BackColor = Color.LightGray;
            CompareEDID.FlatStyle = FlatStyle.Flat;
            CompareEDID.ForeColor = SystemColors.WindowFrame;
            CompareEDID.Image = (Image)resources.GetObject("CompareEDID.Image");
            CompareEDID.Location = new Point(1212, 381);
            CompareEDID.Margin = new Padding(2);
            CompareEDID.Name = "CompareEDID";
            CompareEDID.Size = new Size(524, 278);
            CompareEDID.TabIndex = 1;
            CompareEDID.Text = "Compare";
            CompareEDID.TextAlign = ContentAlignment.TopCenter;
            CompareEDID.TextImageRelation = TextImageRelation.TextAboveImage;
            CompareEDID.UseVisualStyleBackColor = false;
            CompareEDID.Click += button2_Click;
            // 
            // button1
            // 
            button1.AutoSize = true;
            button1.BackColor = Color.LightGray;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.Location = new Point(861, 862);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(523, 267);
            button1.TabIndex = 2;
            button1.Text = "Save EDID";
            button1.TextAlign = ContentAlignment.TopCenter;
            button1.TextImageRelation = TextImageRelation.TextAboveImage;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(28, 30);
            pictureBox1.Margin = new Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(269, 156);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(240F, 240F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(41, 44, 51);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(2270, 1517);
            Controls.Add(pictureBox1);
            Controls.Add(button1);
            Controls.Add(CompareEDID);
            Controls.Add(ShowEDID);
            Font = new Font("Microsoft Sans Serif", 15.9000006F, FontStyle.Regular, GraphicsUnit.Point);
            ForeColor = SystemColors.WindowFrame;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "Form1";
            Text = "EDID Validation";
            FormClosing += ApplicationExitHandler;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        public void ApplicationExitHandler(object sender, EventArgs e)
        {
            try
            {
                File.Delete("DBoutput.txt");
                File.Delete("DBedidFile.txt");
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
        private Button button1;
        private PictureBox pictureBox1;
    }
}