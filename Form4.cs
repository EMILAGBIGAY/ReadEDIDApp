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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private string userName()
        {
            return textBox1.Text;
        }

        private string Password()
        {
            return textBox2.Text;
        }
        private APICalls apiCalls = new APICalls();

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = userName();
            string password = Password();
            APICalls.User user = await apiCalls.GetUserInfo(username, password);
            if (user != null && user.Role == "ADMIN")
            {
                Form3 Save = new Form3();
                Save.ShowDialog();
                Close();
            }
            else if (user != null && user.Role != "ADMIN")
            {
                Close();
                MessageBox.Show("Invalid Permissions");
            }
            else
            {
                Close();
                MessageBox.Show("Invalid Login");
            }
        }
        private async void EnteredPass(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string username = userName();
                string password = Password();
                APICalls.User user = await apiCalls.GetUserInfo(username, password);
                if (user != null && user.Role == "ADMIN" /*|| username == "ABCDE"&& password == "12345"*/)
                {
                    Form3 Save = new Form3();
                    Save.ShowDialog();
                    Close();
                }
                else if (user != null && user.Role != "ADMIN")
                {
                    Close();
                    MessageBox.Show("Invalid Permissions");
                }
                else
                {
                    Close();
                    MessageBox.Show("Invalid Login");
                }

            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
