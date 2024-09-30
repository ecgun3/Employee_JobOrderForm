using İmalat;
using İmalatMalzemeler_Gerceklesen;
using İmalatMalzemeleri;
using MalzemelBirimFiyat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İs_Emri_Formu
{
    public partial class Form3 : Form
    {

        public Form3()
        {
            InitializeComponent();
        }

        //Form Olustur
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();

            this.Hide();
        }

        //Form Duzenle
        private void button2_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form7 form7 = new Form7();
            form7.Show();

            this.Hide();
        }


        //İmalatMalzemeleri Ongoru
        private void button3_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11();
            form11.Show();

            this.Hide();
        }

        //İmalatMalzemeleri Gerçeklesen
        private void button4_Click(object sender, EventArgs e)
        {
            Form9 form9 = new Form9();
            form9.Show();

            this.Hide();
        }

        //MalzemeBirimFiyat 
        private void button5_Click(object sender, EventArgs e)
        {
            Form12 form12 = new Form12();
            form12.Show();

            this.Hide();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();

            this.Hide();
        }
    }
}
