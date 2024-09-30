using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace İs_Emri_Formu
{
    public partial class Form7 : Form
    {
        readonly SqlConnection connectionString = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
        readonly private DataTable dt;
        public Form7()
        {
            InitializeComponent();



        }
        private void Form7_Load_1(object sender, EventArgs e)
        {
            // ListBox'u doldur
            FillIsEmriNoListBox();

        }

        private void FillIsEmriNoListBox()
        {
            try
            {
                connectionString.Open();

                // SQL Query
                string query = "SELECT İsEmriNo FROM IsTB";

                SqlCommand command = new SqlCommand(query, connectionString);
                SqlDataReader reader = command.ExecuteReader();

                // ListBox'ı temizle
                listBox1.Items.Clear();

                // ListBox'a verileri doldur
                while (reader.Read())
                {
                    listBox1.Items.Add(reader["İsEmriNo"].ToString());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler alınırken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string selectedIsEmriNo = listBox1.SelectedItem.ToString();
                GetDataFromDatabase(selectedIsEmriNo);
            }
        }

        private void GetDataFromDatabase(string isEmriNo)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                if (connectionString.State != ConnectionState.Open)
                    connectionString.Open();

                // SQL Query
                string query = "SELECT * FROM IsTB WHERE İsEmriNo = @isEmriNo";

                adapter.SelectCommand = new SqlCommand(query, connectionString);

                // Parametreyi ayarla
                adapter.SelectCommand.Parameters.AddWithValue("@isEmriNo", isEmriNo);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // TextBox'lara ve diğer alanlara verileri doldurma
                    İsEmriNo3.Text = row["İsEmriNo"].ToString();
                    İsiTalepEdenTxtbox2.Text = row["İsiTalepEden"] != DBNull.Value ? row["İsiTalepEden"].ToString() : string.Empty;
                    VerilenIsTanım2Txtbox.Text = row["VerilenIsinTanimi"] != DBNull.Value ? row["VerilenIsinTanimi"].ToString() : string.Empty;
                    textBox1.Text = row["İstenenTeslimTarihi"] != DBNull.Value ? Convert.ToDateTime(row["İstenenTeslimTarihi"]).ToString("dd.MM.yyyy") : string.Empty;
                    İsTanim2.Text = row["IsTanimi"] != DBNull.Value ? row["IsTanimi"].ToString() : string.Empty;
                    ProjeKodutxtBox2.Text = row["ProjeKodu"] != DBNull.Value ? row["ProjeKodu"].ToString() : string.Empty;
                    EvetChckBox2.Checked = row["Oneri"].ToString().Equals("Evet", StringComparison.OrdinalIgnoreCase);
                    HayırChckBox2.Checked = row["Oneri"].ToString().Equals("Hayır", StringComparison.OrdinalIgnoreCase);
                    OneriNo2.Text = row["OneriNo"] != DBNull.Value ? row["OneriNo"].ToString() : string.Empty;
                    IsinYapildigiBlmTxtbox.Text = row["IsınYapildigiBolum"] != DBNull.Value ? row["IsınYapildigiBolum"].ToString() : string.Empty;
                    ProDurumuSeriChckBox2.Checked = row["ProjeDurumu"].ToString().Equals("Seri", StringComparison.OrdinalIgnoreCase);
                    ProDurumuProjeChckbox2.Checked = row["ProjeDurumu"].ToString().Equals("Proje", StringComparison.OrdinalIgnoreCase);
                    IsiVerenBolum2.Text = row["IsıVerenBolum"] != DBNull.Value ? row["IsıVerenBolum"].ToString() : string.Empty;
                    GondermeTarihitxtbox2.Text = row["GondermeTarihi"] != DBNull.Value ? row["GondermeTarihi"].ToString() : string.Empty;
                }
                else
                {
                    MessageBox.Show("İş emri bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri alınamadı: " + ex.Message);
            }
            finally
            {
                if (connectionString.State != ConnectionState.Closed)
                {
                    connectionString.Close();
                }
                adapter.Dispose();
            }
        }
        //Combobox durumları:
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Visible = true;
        }


        //Onay Butonu
        private void OnayButon_Click(object sender, EventArgs e)
        {
            SebepLabel.Visible = false;
            SebebiTxt.Visible = false;
        }

        //OnayNo butonu
        private void OnayNoButon_Click(object sender, EventArgs e)
        {
            SebebiTxt.Visible = true;
            SebepLabel.Visible = true;

            SebebiTxt.Focus();
        }


        #region TRY-CATCH
        private void Kaydet()
        {
            try
            {
                connectionString.Open();

                // SQL sorgusu
                string query = @"IF EXISTS (SELECT 1 FROM OnayTB WHERE İsEmriNo = @İsEmriNo)
                                BEGIN
                                    UPDATE OnayTB
                                    SET İsTalepOnayiVeren = @İsTalepOnayiVeren,
                                        İstenenTeslimTarihi = @İstenenTeslimTarihi,
                                        İsTeslimTarihiUygunlugu = @İsTeslimTarihiUygunlugu,
                                        UygunOlanTeslimTarihi = @UygunOlanTeslimTarihi,
                                        KaliphaneAmiri = @KaliphaneAmiri,
                                        Aciklama = @Aciklama,
                                        OnayDurumu = @OnayDurumu,
                                        Sebebi = @Sebebi
                                    WHERE İsEmriNo = @İsEmriNo;
                                END
                                ELSE
                                BEGIN
                                    INSERT INTO OnayTB (İsEmriNo,
                                                        İsTalepOnayiVeren,
                                                        İstenenTeslimTarihi,
                                                        İsTeslimTarihiUygunlugu,
                                                        UygunOlanTeslimTarihi,
                                                        KaliphaneAmiri,
                                                        Aciklama,
                                                        OnayDurumu,
                                                        Sebebi)
                                    VALUES (@İsEmriNo,
                                            @İsTalepOnayiVeren,
                                            @İstenenTeslimTarihi,
                                            @İsTeslimTarihiUygunlugu,
                                            @UygunOlanTeslimTarihi,
                                            @KaliphaneAmiri,
                                            @Aciklama,
                                            @OnayDurumu,
                                            @Sebebi);
                                END";

                SqlCommand command = new SqlCommand(query, connectionString);


                //İsTalepOnayiVeren
                command.Parameters.AddWithValue("@İsTalepOnayiVeren", OnayVerentxtbox.Text);

                //İstenenTeslimTarihi
                command.Parameters.AddWithValue("@İstenenTeslimTarihi", textBox1.Text);

                //İsTeslimTarihiUygunlugu (Checkbox)
                if (radioButton1.Checked)
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu", "Uygun");
                else if (radioButton2.Checked)
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu", "Uygun Değil");
                else
                    command.Parameters.AddWithValue("@İsTeslimTarihiUygunlugu", DBNull.Value);

                //UygunOlanTeslimTarihi --> dateTimePicker
                command.Parameters.AddWithValue("@UygunOlanTeslimTarihi", dateTimePicker2.Value);

                //KaliphaneAmiri
                command.Parameters.AddWithValue("@KaliphaneAmiri", KaliphaneTextbox.Text);

                //Aciklama
                command.Parameters.AddWithValue("@Aciklama", AciklamaTextbox.Text);

                //İs emri
                command.Parameters.AddWithValue("@İsEmriNo", İsEmriNo3.Text);

                //Onay Butonu
                if (OnayButon.Checked)
                {
                    command.Parameters.AddWithValue("@OnayDurumu", "Onaylıyorum");
                    command.Parameters.AddWithValue("@Sebebi", DBNull.Value);
                }
                else if (OnayNoButon.Checked)
                {
                    command.Parameters.AddWithValue("@OnayDurumu", "Onaylamıyorum");
                    command.Parameters.AddWithValue("@Sebebi", SebebiTxt.Text);
                }
                else
                {
                    MessageBox.Show("Onay butonları boş bırakılamaz!");
                    return;
                }

                command.ExecuteNonQuery();
                MessageBox.Show("Veritabanına başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına kaydedilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }
        #endregion


        private void button1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
            this.Dispose();
        }

        private void FormKayıtButon_Click_1(object sender, EventArgs e)
        {
            Kaydet();
        }
    }
}
