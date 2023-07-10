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
            ShowEDID.Location = new Point(359, 157);
            ShowEDID.Margin = new Padding(1);
            ShowEDID.Name = "ShowEDID";
            ShowEDID.Size = new Size(229, 108);
            ShowEDID.TabIndex = 0;
            ShowEDID.Text = "Get EDID";
            ShowEDID.UseVisualStyleBackColor = true;
            ShowEDID.Click += button1_Click;
            // 
            // CompareEDID
            // 
            CompareEDID.Location = new Point(359, 304);
            CompareEDID.Margin = new Padding(1);
            CompareEDID.Name = "CompareEDID";
            CompareEDID.Size = new Size(229, 108);
            CompareEDID.TabIndex = 1;
            CompareEDID.Text = "Compare";
            CompareEDID.UseVisualStyleBackColor = true;
            CompareEDID.Click += button2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(993, 615);
            Controls.Add(CompareEDID);
            Controls.Add(ShowEDID);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(1);
            Name = "Form1";
            Text = "EDID Validation";
            ResumeLayout(false);
        }

        #endregion

        private Button ShowEDID;
        private Button CompareEDID;
        private VScrollBar vScrollBar1;
    }
}