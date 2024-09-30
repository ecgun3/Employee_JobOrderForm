using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İs_Emri_Formu
{
    public partial class Form4 : Form
    {

        bool password = false;

        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "1234")
            {
                password = true;
                
            }
            else
            {
                password = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (password)
            {
                this.Close();

                Form5 form5 = new Form5();
                form5.Show();


            }
            else
            {
                MessageBox.Show("Hata: Şifre yanlış");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Şifreyi göster
                textBox1.UseSystemPasswordChar = false;
            }
            else
            {
                // Şifreyi gizle
                textBox1.UseSystemPasswordChar = true;
            }
        }
    }
}
