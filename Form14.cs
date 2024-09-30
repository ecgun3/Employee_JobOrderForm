using İs_Emri_Formu;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace İmalat
{
    public partial class Form14 : Form
    {

        SqlConnection connectionString = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);

        string selectedIsEmriNo;

        private DataTable OngoruMalzeme_dt;
        private DataTable OngoruSure_dt;
        private DataTable GerceklesenMalzeme_dt;
        private DataTable GerceklesenSure_dt;

        public Form14()
        {
            InitializeComponent();

        }

        private void Form14_Load(object sender, System.EventArgs e)
        {
            FillIsEmriNoListBox();

            OngoruMalzeme_dt = new DataTable();
            OngoruSure_dt = new DataTable();
            GerceklesenMalzeme_dt = new DataTable();
            GerceklesenSure_dt = new DataTable();
        }


        #region İşEmriNo Combobox1

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

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                selectedIsEmriNo = listBox1.SelectedItem.ToString();
                LoadData(selectedIsEmriNo);
                BilgiGetir_Click(selectedIsEmriNo);
            }
        }
        #endregion

        #region LoadData (Datagrid)
        private void LoadData(string value)
        {
            #region 1.Try-catch
            try
            {
                connectionString.Open();

                //Toplam maliyeti ve süreleri öngörülen ve gerçekleşen olarak bir değişkene atamak
                //ISNULL()--> NULL ise 0 değerini döndürür

                #region Sorgular
                string queryMaliyet_Ongorulen = "SELECT ISNULL(ToplamMaliyet, 0) FROM ImalatMalz_OngoruTB WHERE IsEmriNo = @IsEmriNo";
                string queryMaliyet_Gerceklesen = "SELECT ISNULL(ToplamMaliyet, 0) FROM ImalatMalz_GerceklesenTB WHERE IsEmriNo = @IsEmriNo";

                //ToplamSure varchar biçiminde olduğu için hh:mm:ss formatına dönüştürmem lazım (doğrudan zaman biçiminde kullanmak için) --> süreyi TimeSpasn nesnesine dönüştürmek için
                string querySure_Ongorulen = @"
                SELECT 
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, 1, CHARINDEX(' saat', ToplamSure) - 1) AS INT) AS VARCHAR), 2) + ':' +
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, CHARINDEX(' saat', ToplamSure) + 6, CHARINDEX(' dk', ToplamSure) - CHARINDEX(' saat', ToplamSure) - 6) AS INT) AS VARCHAR), 2) + ':' +
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, CHARINDEX(' dk', ToplamSure) + 4, CHARINDEX(' sn', ToplamSure) - CHARINDEX(' dk', ToplamSure) - 4) AS INT) AS VARCHAR), 2) AS ToplamSure
                FROM Ongoru_Sure 
                WHERE IsEmriNo = @IsEmriNo";

                string querySure_Gerceklesen = @"
                SELECT 
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, 1, CHARINDEX(' saat', ToplamSure) - 1) AS INT) AS VARCHAR), 2) + ':' +
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, CHARINDEX(' saat', ToplamSure) + 6, CHARINDEX(' dk', ToplamSure) - CHARINDEX(' saat', ToplamSure) - 6) AS INT) AS VARCHAR), 2) + ':' +
                    RIGHT('0' + CAST(CAST(SUBSTRING(ToplamSure, CHARINDEX(' dk', ToplamSure) + 4, CHARINDEX(' sn', ToplamSure) - CHARINDEX(' dk', ToplamSure) - 4) AS INT) AS VARCHAR), 2) AS ToplamSure
                FROM Gerceklesen_Sure 
                WHERE IsEmriNo = @IsEmriNo";
                #endregion

                //Her bir SqlCommand nesnesi --> Bir SQL sorgusunu çalıştırır ve sonuç alır

                #region Maliyet için
                decimal maliyetOngorulen;
                decimal maliyetGerceklesen;

                //Öngörülen Maliyet
                SqlCommand commandMaliyetOngorulen = new SqlCommand(queryMaliyet_Ongorulen, connectionString);
                commandMaliyetOngorulen.Parameters.AddWithValue("@IsEmriNo", value);
                object maliyetOngorulenObj = commandMaliyetOngorulen.ExecuteScalar();

                /*
                 * NumberStyles.Any --> hem . hem , le yazılmış numaraları çevirebilir
                 * CulutreInfo --> dil ve bölgeden bağımsız biçimlendieir
                 */

                if (maliyetOngorulenObj != null && decimal.TryParse(maliyetOngorulenObj.ToString().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out maliyetOngorulen))
                    maliyetOngorulen = Convert.ToDecimal((decimal)maliyetOngorulen);
                else
                    maliyetOngorulen = 0;

                //Gerçekleşen Maliyet
                SqlCommand commandMaliyetGerceklesen = new SqlCommand(queryMaliyet_Gerceklesen, connectionString);
                commandMaliyetGerceklesen.Parameters.AddWithValue("@IsEmriNo", value);
                object maliyetGerceklesenObj = commandMaliyetGerceklesen.ExecuteScalar();

                if (maliyetGerceklesenObj != null && decimal.TryParse(maliyetGerceklesenObj.ToString().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out maliyetGerceklesen))
                    maliyetGerceklesen = Convert.ToDecimal((decimal)maliyetGerceklesen);
                else
                    maliyetGerceklesen = 0;

                // TextBox'lara atama
                OngoruMaliyet.Text = maliyetOngorulen.ToString("C", CultureInfo.CreateSpecificCulture("de-DE"));
                GerceklesenMaliyet.Text = maliyetGerceklesen.ToString("C", CultureInfo.CreateSpecificCulture("de-DE"));

                // Maliyet farkını hesaplayıp TextBox'a atama
                decimal maliyetFarki = maliyetGerceklesen - maliyetOngorulen;

                if (maliyetFarki > 0)
                    MaliyetFarki.Text = maliyetFarki.ToString("C", CultureInfo.CreateSpecificCulture("de-DE")) + " Zarar";
                else
                    MaliyetFarki.Text = Math.Abs(maliyetFarki).ToString("C", CultureInfo.CreateSpecificCulture("de-DE")) + " Kar";

                #endregion

                #region Süre için
                TimeSpan sureOngorulen = TimeSpan.Zero;
                TimeSpan sureGerceklesen = TimeSpan.Zero;

                //Öngörülen Süre
                SqlCommand commandSureOngorulen = new SqlCommand(querySure_Ongorulen, connectionString);
                commandSureOngorulen.Parameters.AddWithValue("@IsEmriNo", value);
                object sureOngorulenObj = commandSureOngorulen.ExecuteScalar();

                if (sureOngorulenObj != null && TimeSpan.TryParse(sureOngorulenObj.ToString(), out sureOngorulen)) //string'i --> TimeSpan objectine çevirdim
                    OngoruSure.Text = sureOngorulen.ToString();
                else
                    OngoruSure.Text = TimeSpan.Zero.ToString();

                //Gerçekleşen Süre
                SqlCommand commandSureGerceklesen = new SqlCommand(querySure_Gerceklesen, connectionString);
                commandSureGerceklesen.Parameters.AddWithValue("@IsEmriNo", value); //commandSureGerceklesen --> bir sorgu çalıştırır
                object sureGerceklesenObj = commandSureGerceklesen.ExecuteScalar();

                if (sureGerceklesenObj != null && TimeSpan.TryParse(sureGerceklesenObj.ToString(), out sureGerceklesen))
                    GerceklesenSure.Text = sureGerceklesen.ToString();
                else
                    GerceklesenSure.Text = TimeSpan.Zero.ToString();

                // Süre farkını hesaplayıp TextBox'a atama
                TimeSpan sureFarki = sureGerceklesen - sureOngorulen;
                string sureFarkiText;

                //iki timespan nesnesi arasındaki farkı hesaplayıp fomratladık
                if (sureFarki.TotalSeconds > 0)
                    sureFarkiText = string.Format("{0:D2} saat {1:D2} dk {2:D2} sn (ZARAR)", sureFarki.Hours, sureFarki.Minutes, sureFarki.Seconds);
                else
                    sureFarkiText = string.Format("{0:D2} saat {1:D2} dk {2:D2} sn (KAR)", Math.Abs(sureFarki.Hours), Math.Abs(sureFarki.Minutes), Math.Abs(sureFarki.Seconds));

                SureFarki.Text = sureFarkiText;
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanından veri çekilirken hata oluştu (LoadData() ilk kısım Form2): " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
            #endregion

            //---//
            #region 2. Try-catch
            try
            {
                connectionString.Open();

                // İlk DataGridView için veri çekme
                string queryOngoruMalz = "SELECT * FROM ImalatMalz_OngoruTB WHERE IsEmriNo = @IsEmriNo";
                SqlCommand commandOngoruMalz = new SqlCommand(queryOngoruMalz, connectionString);
                commandOngoruMalz.Parameters.AddWithValue("@IsEmriNo", value);
                SqlDataAdapter adapterOngoruMalz = new SqlDataAdapter(commandOngoruMalz);
                OngoruMalzeme_dt.Clear();
                adapterOngoruMalz.Fill(OngoruMalzeme_dt);
                OngoruMalzemeGrid.DataSource = OngoruMalzeme_dt;

                // İkinci DataGridView için veri çekme
                string queryOngoruSuresi = "SELECT * FROM Ongoru_Sure WHERE IsEmriNo = @IsEmriNo";
                SqlCommand commandOngoruSure = new SqlCommand(queryOngoruSuresi, connectionString);
                commandOngoruSure.Parameters.AddWithValue("@IsEmriNo", value);
                SqlDataAdapter adapterOngoruSure = new SqlDataAdapter(commandOngoruSure);
                OngoruSure_dt.Clear();
                adapterOngoruSure.Fill(OngoruSure_dt);
                OngoruSureGrid.DataSource = OngoruSure_dt;

                //Üçüncü
                string queryGerceklesenMalz = "SELECT * FROM ImalatMalz_GerceklesenTB WHERE IsEmriNo = @IsEmriNo";
                SqlCommand commandGerceklesenMalz = new SqlCommand(queryGerceklesenMalz, connectionString);
                commandGerceklesenMalz.Parameters.AddWithValue("@IsEmriNo", value);
                SqlDataAdapter adapterGerceklesenMalz = new SqlDataAdapter(commandGerceklesenMalz);
                GerceklesenMalzeme_dt.Clear();
                adapterGerceklesenMalz.Fill(GerceklesenMalzeme_dt);
                GerkceklesenMalzemeGrid.DataSource = GerceklesenMalzeme_dt;


                //Dödüncü 
                string queryGerceklesenSure = "SELECT * FROM Gerceklesen_Sure WHERE IsEmriNo = @IsEmriNo";
                SqlCommand commandGerceklesenSure = new SqlCommand(queryGerceklesenSure, connectionString);
                commandGerceklesenSure.Parameters.AddWithValue("@IsEmriNo", value);
                SqlDataAdapter adapterGerceklesenSure = new SqlDataAdapter(commandGerceklesenSure);
                GerceklesenSure_dt.Clear();
                adapterGerceklesenSure.Fill(GerceklesenSure_dt);
                GerceklesenSureGrid.DataSource = GerceklesenSure_dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanından veri çekilirken hata oluştu (LoadData() ikinci kısım Form2): " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
            #endregion
        }
        #endregion


        private void BilgiGetir_Click(string selectedIsEmriNo)
        {
            try
            {
                connectionString.Open();

                // ImalatSure_Onay --> Gerceklesen_Sure ' den ToplamSure
                string query1 = "SELECT ToplamSure FROM Gerceklesen_Sure WHERE IsEmriNo = @IsEmriNo";
                SqlCommand command1 = new SqlCommand(query1, connectionString);
                command1.Parameters.AddWithValue("@IsEmriNo", selectedIsEmriNo);
                object result1 = command1.ExecuteScalar();
                if (result1 != null)
                {
                    textBox5.Text = result1.ToString();
                }

                // GerceklesenKayitTarihi_Onay --> Ongoru_Sure ' den KayitTarihi
                string query2 = "SELECT CONVERT(VARCHAR(10), KayitTarihi, 104) FROM Ongoru_Sure WHERE IsEmriNo = @IsEmriNo"; //Bu sayede sadece tarihi getirdik.
                SqlCommand command2 = new SqlCommand(query2, connectionString);
                command2.Parameters.AddWithValue("@IsEmriNo", selectedIsEmriNo);
                object result2 = command2.ExecuteScalar();
                if (result2 != null)
                {
                    textBox1.Text = result2.ToString();
                }

                // TeslimEden_Onay --> IsTB' de İsiTalepEden
                string query3 = "SELECT İsiTalepEden FROM IsTB WHERE İsEmriNo = @IsEmriNo";
                SqlCommand command3 = new SqlCommand(query3, connectionString);
                command3.Parameters.AddWithValue("@IsEmriNo", selectedIsEmriNo);
                object result3 = command3.ExecuteScalar();
                if (result3 != null)
                {
                    textBox4.Text = result3.ToString();
                }

                // TeslimAlan_Onay --> OnayTB'de İsTalepOnayiVeren
                string query4 = "SELECT İsTalepOnayiVeren FROM OnayTB WHERE İsEmriNo = @IsEmriNo";
                SqlCommand command4 = new SqlCommand(query4, connectionString);
                command4.Parameters.AddWithValue("@IsEmriNo", selectedIsEmriNo);
                object result4 = command4.ExecuteScalar();
                if (result4 != null)
                {
                    textBox3.Text = result4.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler alınırken hata oluştu (BilgiGetir_Click): " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Kaydet();
        }

        #region Onay Kaydet
        private void Kaydet()
        {
            try
            {
                connectionString.Open();

                //SQL
                string query = @"INSERT INTO ImalatOnayTB   (İmalatSure,
                                                            İsinTeslimTarihi,
                                                            TeslimEden,
                                                            TeslimAlan,
                                                            KontrolEden,                                                                    
                                                            İsEmriNo,
                                                            KayitTarihi)
                                        VALUES (@İmalatSure,
                                                @İsinTeslimTarihi,
                                                @TeslimEden,
                                                @TeslimAlan,
                                                @KontrolEden,                                                                    
                                                @İsEmriNo,
                                                GETDATE())";

                SqlCommand cmd = new SqlCommand(query, connectionString);

                cmd.Parameters.AddWithValue("@İmalatSure", textBox5.Text);
                cmd.Parameters.AddWithValue("@İsinTeslimTarihi", textBox1.Text);
                cmd.Parameters.AddWithValue("@TeslimEden", textBox4.Text);
                cmd.Parameters.AddWithValue("@TeslimAlan", textBox3.Text);
                cmd.Parameters.AddWithValue("@KontrolEden", textBox6.Text);
                cmd.Parameters.AddWithValue("@İsEmriNo", selectedIsEmriNo);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Veritabanına kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanına kaydedilirken hata oluştu (Kaydet): " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();

            this.Dispose();
        }
    }
}
