using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace İs_Emri_Formu
{
    public partial class Form5 : Form
    {

        // İş tanımlarını tutan bir liste
        public List<string> JobDescriptions { get; private set; }

        private DataStorage storage = new DataStorage(); // DataStorage sınıfı oluşturuluyor

        public Form5()
        {
            InitializeComponent();
            panel1.Visible = false;

            // JSON dosyasından job descriptions'ı yükleyelim
            JobDescriptions = storage.LoadJobDescriptions();
            UpdateListBox(); // Mevcut tanımları göster

        }

        // Form yüklendiğinde listbox'a JobDescriptions'ı ekleyelim
        private void Form5_Load(object sender, EventArgs e)
        {
            // jobDescriptions listesini ListBox'a ekle
            foreach (var job in JobDescriptions)
            {
                listBox_isler.Items.Add(job);
                VerilenIsinTanimi.Items.Add(job);
            }
        }

        // ListBox'a mevcut iş tanımlarını ekleyelim
        private void UpdateListBox()
        {
            listBox_isler.Items.Clear();
            foreach (var job in JobDescriptions)
            {
                listBox_isler.Items.Add(job);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!panel1.Visible)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
        }

        private void GonderBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_ekle_Click(object sender, EventArgs e)
        {
            // Yeni iş tanımı ekle
            if (!string.IsNullOrWhiteSpace(textBox_isTanimi.Text))
            {
                JobDescriptions.Add(textBox_isTanimi.Text);
                UpdateListBox();
                textBox_isTanimi.Clear();
                // Değişiklikleri kaydedelim
                storage.SaveJobDescriptions(JobDescriptions);
            }
        }

        private void button_kaldir_Click(object sender, EventArgs e)
        {
            // Seçilen iş tanımını kaldır
            if (listBox_isler.SelectedItem != null)
            {
                JobDescriptions.Remove(listBox_isler.SelectedItem.ToString());
                UpdateListBox();
                // Değişiklikleri kaydedelim
                storage.SaveJobDescriptions(JobDescriptions);
            }
        }

        private void listBox_isler_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
