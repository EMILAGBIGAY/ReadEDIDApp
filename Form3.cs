using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edid_GUI
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Dpi;
            for (int i = 1; i <= fileGlobal.EDIDInfo.Count; i++)
            {
                comboBox1.Items.Add("EDID " + i);
            }

        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private string ComboBoxValue()
        {
            return comboBox1.Text;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string part = BoxValue();
                string EdidNum = ComboBoxValue();
                EdidNum = EdidNum.Replace("EDID ", string.Empty);
                int Num = int.Parse(EdidNum);
                string con = File.ReadAllText(fileGlobal.files[Num - 1]);
                con = con.Replace(" ", string.Empty);
                con = con.Replace("\n", string.Empty);
                con = con.Replace("\r", string.Empty);

                APICalls call = new APICalls();
                _ = call.setEDID(part, con);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("can't find part number " + ex.Message);
                return;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private string BoxValue()
        {
            return textBox1.Text;
        }

    }
}
