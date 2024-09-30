﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//İsim soyisim başta dursun,
//Enter'a basınca sıradaki textbox'A veya combobox'a gitsin
//
namespace İs_Emri_Formu
{
    public partial class Form1 : Form
    {
        SqlConnection connectionString = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);

        string projeKodu;

        private List<string> JobDescriptions = new List<string> {};
        private DataStorage storage = new DataStorage(); // DataStorage sınıfı oluşturuluyor



        public Form1()
        {
            InitializeComponent();

            //Click olayını ekleyelim:
            İsTanim.Click += new EventHandler(İsTanim_Click);
            İsiTalepEdenTxtbox.Click += new EventHandler(İsiTalepEdenTxtbox_Click);

            //sadece rakam için:
            isEmriNo.KeyPress += new KeyPressEventHandler(isEmriNo_KeyPress);
            OneriNo.KeyPress += new KeyPressEventHandler(OneriNo_KeyPress);

            //checkedCange olayı:
            EvetChckBox.CheckedChanged += new EventHandler(EvetChckBox_CheckedChanged);
            HayırChckBox.CheckedChanged += new EventHandler(HayırChckBox_CheckedChanged);
            ProDurumuSeriChckBox.CheckedChanged += new EventHandler(ProDurumuSeriChckBox_CheckedChanged);
            ProDurumuProjeChckbox.CheckedChanged += new EventHandler(ProDurumuProjeChckbox_CheckedChanged);
            IsıVerenBolum.SelectedIndexChanged += new EventHandler(comboBox3_SelectedIndexChanged);

            //Controls olayı:
            Controls.Add(EvetChckBox);
            Controls.Add(HayırChckBox);
            Controls.Add(ProDurumuSeriChckBox);
            Controls.Add(ProDurumuProjeChckbox);
            Controls.Add(IsıVerenBolum);

            //Yukleme olaylari
            this.Load += new EventHandler(Form1_Load);

            // Başlangıçta iş tanımlarını doldur
            JobDescriptions = storage.LoadJobDescriptions();
            UpdateComboBox(JobDescriptions); 


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EvetChckBox.Visible = true;
            HayırChckBox.Visible = true;

            ProDurumuSeriChckBox.Visible = true;
            ProDurumuProjeChckbox.Visible = true;

            //Is emri no olusturur
            isEmriNo.Text = DateTime.Now.ToString("ddMMyyyyHHmmss");
            textBoxProjeKodu.Text = ProjeKoduOlustur();

            
        }
        private string ProjeKoduOlustur()
        {
            //Proje kodunun tanimlanmasi
            string projeKodu = "P" + DateTime.Now.ToString("ddMMyyyyHHmmss");

            //Random random = new Random();

            //int currentYear = DateTime.Now.Year;
            //string lastTwoDigits = (currentYear % 100).ToString();
            //int randomNumber2 = random.Next(10000, 100000);
            //string projeKodu = "P" + lastTwoDigits + randomNumber2.ToString();




            return projeKodu;
        }

        // ComboBox'ı güncellemek için kullanılan fonksiyon
        private void UpdateComboBox(List<string> _jobDescriptions)
        {
            VerilenIsinTanimi.Items.Clear();
            foreach (var job in _jobDescriptions)
            {
                VerilenIsinTanimi.Items.Add(job);
            }
        }


        //CLİCK OLYLARI:

        //İsim soyisim girmek için textbox
        private void İsiTalepEdenTxtbox_Click(object sender, EventArgs e)
        {
            if (İsiTalepEdenTxtbox.Text == "İsim Soyisim Giriniz: ")
                İsiTalepEdenTxtbox.Text = "";
        }

        //iş emri girmek için textBox
        private void isEmriNo_Click(object sender, EventArgs e)
        {
            if (isEmriNo.Text == "İş Emri No: ")
                isEmriNo.Text = "";
        }

        //İşin tanımını girmek için Textbox:
        private void İsTanim_Click(object sender, EventArgs e)
        {
            if (İsTanim.Text == "İşin Tanımı: ")
                İsTanim.Text = "";
        }

        //KEYPRESS OLAYLARI:

        //Sadece numara girmesi için keypress
        private void isEmriNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Sadece rakamlar ve geri silme karakterine izin 
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Sadece rakam giriniz.");
            }
        }
        private void OneriNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Sadece rakam giriniz.");
            } 
        }

        //CHECKBOX CHANGE Durumları:
        private void EvetChckBox_CheckedChanged(object sender, EventArgs e)
        {
            bool isCecked = EvetChckBox.Checked;

            OneriNo.Visible = isCecked;
            OneriNoLabel.Visible = isCecked;

            //Evet seçiliyken görünür yap,değilken görünmez yap:
            if(!isCecked)
            {
                OneriNo.Visible = false;
                OneriNoLabel.Visible = false;
            }

            //bu yanarken diğeri yanmamalı.
            if (isCecked)
                HayırChckBox.Checked = false;
        }
        
        private void HayırChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (HayırChckBox.Checked) //hayır yanarken evet yanmamalı
                EvetChckBox.Checked = false;
        }

        private void ProDurumuSeriChckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(ProDurumuSeriChckBox.Checked) //Seri durumdayken proje durumu olamaz
                ProDurumuProjeChckbox.Checked = false;
        }

        private void ProDurumuProjeChckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ProDurumuProjeChckbox.Checked) //Proje durumundayken seri durumu olamaz
                ProDurumuSeriChckBox.Checked = false;
        }

        //Diğer seçeneğini seçtiğimizde olanlar:
         
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Debug
            string selectedOption = IsıVerenBolum.SelectedItem?.ToString(); //Buna akternatif çözüm(?) ekledik !!!

            if (selectedOption == "Diğer..")
            {
                DigerSecenekLabel.Visible = true;
                DigerSecenekTxtBox.Visible = true;

                DigerSecenekTxtBox.Focus(); // TextBox'a odaklanma
            }
            else
            {
                DigerSecenekTxtBox.Visible = false;
                DigerSecenekLabel.Visible = false;
                DigerSecenekTxtBox.Text = ""; // TextBox içeriğini temizleme
            }
        }

        private bool IsIsEmriNoExists(string isEmriNo)
        {
            bool İsEmriNo_exists = false;

            try
            {
                connectionString.Open();
                string query = "SELECT COUNT(*) FROM IsTB WHERE İsEmriNo = @IsEmriNo";
                SqlCommand command = new SqlCommand(query, connectionString);
                command.Parameters.AddWithValue("@IsEmriNo", isEmriNo);

                int count = (int)command.ExecuteScalar();
                İsEmriNo_exists = count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına bağlanırken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }

            return İsEmriNo_exists;
        }

        //Veritabanına kaydetme:
        private void GonderBtn_Click(object sender, EventArgs e)
        {
            //İsEmriNo zaten primary key olduğu için daha önceden varsa uyarı mesajı
            string isEmriNoValue = isEmriNo.Text.Trim();

            if (IsIsEmriNoExists(isEmriNoValue))
            {
                MessageBox.Show("Girdiğiniz iş emri numarası zaten mevcut. Lütfen başka bir numara giriniz.");
                return; // İşlemi durdur
            }

            // Veritabanına kaydetme 
            Kaydet();


        }

        //Kaydetme methodunu oluşturalım:
        private void Kaydet()
        {
            try
            {
                connectionString.Open();

                // SQL sorgusu
                string query = @"INSERT INTO IsTB   (İsEmriNo, 
                                                    VerilenIsinTanimi, 
                                                    IsTanimi,
                                                    ProjeKodu,
                                                    Oneri,
                                                    OneriNo,
                                                    IsınYapildigiBolum,
                                                    ProjeDurumu,
                                                    IsıVerenBolum,
                                                    GondermeTarihi,
                                                    İsiTalepEden,
                                                    İstenenTeslimTarihi) 
                                VALUES  (@IsEmriNo,
                                        @VerilenIsinTanimi,
                                        @IsTanimi, 
                                        @ProjeKodu, 
                                        @Oneri,
                                        @OneriNo,
                                        @IsınYapildigiBolum,
                                        @ProjeDurumu,
                                        @IsıVerenBolum,
                                        GETDATE(),
                                        @İsiTalepEden,
                                        @İstenenTeslimTarihi)";

                SqlCommand command = new SqlCommand(query, connectionString);
                
                command.Parameters.AddWithValue("@IsEmriNo", isEmriNo.Text);
                command.Parameters.AddWithValue("@VerilenIsinTanimi", VerilenIsinTanimi.Text);
                command.Parameters.AddWithValue("@IsTanimi", İsTanim.Text);
                command.Parameters.AddWithValue("@ProjeKodu", textBoxProjeKodu.Text);

                //Oneri Checkbox seçeneğine göre Evet veya Hayır
                if (EvetChckBox.Checked)
                    command.Parameters.AddWithValue("@Oneri", "Evet");
                else if (HayırChckBox.Checked)
                    command.Parameters.AddWithValue("@Oneri", "Hayır");
                else
                    command.Parameters.AddWithValue("@Oneri", DBNull.Value);

                // Oneri == "Hayır" --> OneriNo = NULL
                if (HayırChckBox.Checked || string.IsNullOrEmpty(OneriNo.Text))
                    command.Parameters.AddWithValue("@OneriNo", DBNull.Value);
                else if (long.TryParse(OneriNo.Text, out long oneriNoValue))
                {
                    command.Parameters.AddWithValue("@OneriNo", oneriNoValue);
                }
                else
                {
                    MessageBox.Show("Öneri No geçerli bir sayı olmalıdır.");
                    return;
                }
                //command.Parameters.AddWithValue("@OneriNo", string.IsNullOrEmpty(OneriNo.Text) ? DBNull.Value : Convert.ToInt64(OneriNo.Text));

                command.Parameters.AddWithValue("@IsınYapildigiBolum", IsınYapildigiBolum.Text);

                //Proje Durumu
                if (ProDurumuSeriChckBox.Checked)
                    command.Parameters.AddWithValue("@ProjeDurumu", "Seri");
                else if (ProDurumuProjeChckbox.Checked)
                    command.Parameters.AddWithValue("@ProjeDurumu", "Proje");
                else
                    command.Parameters.AddWithValue("@ProjeDurumu", DBNull.Value);

                // İşi Veren Bölüm
                if (IsıVerenBolum.SelectedItem != null && IsıVerenBolum.SelectedItem.ToString() == "Diğer..")
                {
                    command.Parameters.AddWithValue("@IsıVerenBolum", DigerSecenekTxtBox.Text);
                }
                else if (IsıVerenBolum.SelectedItem != null)
                {
                    command.Parameters.AddWithValue("@IsıVerenBolum", IsıVerenBolum.SelectedItem.ToString());
                }
                else
                {
                    command.Parameters.AddWithValue("@IsıVerenBolum", DBNull.Value);
                }

                command.Parameters.AddWithValue("@İsiTalepEden", İsiTalepEdenTxtbox.Text);

                command.Parameters.AddWithValue("@İstenenTeslimTarihi", dateTimePicker1.Value);

                command.ExecuteNonQuery();
                MessageBox.Show("Veritabanına başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Veritabanına kaydedilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBoxProjeKodu.ReadOnly = false;
            }
            else
            {
                textBoxProjeKodu.ReadOnly = true;
                textBoxProjeKodu.Text = ProjeKoduOlustur();
            }
        }

        private void button_Geri_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Dispose();


        }
    }
}
