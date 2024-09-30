using İs_Emri_Formu;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace MalzemelBirimFiyat
{
    public partial class Form12 : Form
    {
        SqlConnection connectionString = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"]);
        private DataTable dt;

        public Form12()
        {
            InitializeComponent();
        }

        #region Girilmiş Son Fiyatı Çek
        private void SonFiyat_Click(object sender, EventArgs e)
        {
            //KayitTarihi Desc olarak en son kaydı getir.

            SqlDataAdapter adapter = new SqlDataAdapter();

            try
            {
                if (connectionString.State != ConnectionState.Open)
                    connectionString.Open();

                string query = @"SELECT * FROM MalzBirimFiyatTB ORDER BY KayitTarihi DESC";

                adapter.SelectCommand = new SqlCommand(query, connectionString);

                dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    //Sayfa1
                    YoneticiID.Text = row["YoneticiID"] != DBNull.Value ? row["YoneticiID"].ToString() : string.Empty;
                    İmalatCeligi.Text = row["İmalatCeligi"] != DBNull.Value ? row["İmalatCeligi"].ToString() : string.Empty;
                    TakimCeligi.Text = row["TakimCeligi"] != DBNull.Value ? row["TakimCeligi"].ToString() : string.Empty;
                    Pirinc.Text = row["Pirinc"] != DBNull.Value ? row["Pirinc"].ToString() : string.Empty;
                    Stavax.Text = row["Stavax"] != DBNull.Value ? row["Stavax"].ToString() : string.Empty;
                    Aluminyum.Text = row["Aluminyum"] != DBNull.Value ? row["Aluminyum"].ToString() : string.Empty;
                    PaslanmazCelik.Text = row["PaslanmazCelik"] != DBNull.Value ? row["PaslanmazCelik"].ToString() : string.Empty;
                    PoliamidKestamit.Text = row["PoliamidKestamit"] != DBNull.Value ? row["PoliamidKestamit"].ToString() : string.Empty;
                    Bakir.Text = row["Bakir"] != DBNull.Value ? row["Bakir"].ToString() : string.Empty;
                    Tungsten.Text = row["Tungsten"] != DBNull.Value ? row["Tungsten"].ToString() : string.Empty;
                    ApmcoYerli.Text = row["ApmcoYerli"] != DBNull.Value ? row["ApmcoYerli"].ToString() : string.Empty;
                    AmpcoIthal.Text = row["AmpcoIthal"] != DBNull.Value ? row["AmpcoIthal"].ToString() : string.Empty;
                    Vulkolon.Text = row["Vulkolon"] != DBNull.Value ? row["Vulkolon"].ToString() : string.Empty;
                    Elastomer.Text = row["Elastomer"] != DBNull.Value ? row["Elastomer"].ToString() : string.Empty;
                    AluKosebent.Text = row["AluKosebent"] != DBNull.Value ? row["AluKosebent"].ToString() : string.Empty;
                    AluProf45x45.Text = row["AluProf45x45"] != DBNull.Value ? row["AluProf45x45"].ToString() : string.Empty;

                    //Sayfa2
                    AluProf30x30.Text = row["AluProf30x30"] != DBNull.Value ? row["AluProf30x30"].ToString() : string.Empty;
                    AluProf25x20.Text = row["AluProf25x20"] != DBNull.Value ? row["AluProf25x20"].ToString() : string.Empty;
                    DemirProf50x50.Text = row["DemirProf50x50"] != DBNull.Value ? row["DemirProf50x50"].ToString() : string.Empty;
                    DemirProf40x40.Text = row["DemirProf40x40"] != DBNull.Value ? row["DemirProf40x40"].ToString() : string.Empty;
                    DemirProf30x30.Text = row["DemirProf30x30"] != DBNull.Value ? row["DemirProf30x30"].ToString() : string.Empty;
                    DemirProf25x25.Text = row["DemirProf25x25"] != DBNull.Value ? row["DemirProf25x25"].ToString() : string.Empty;
                    DemirKose50x50.Text = row["DemirKose50x50"] != DBNull.Value ? row["DemirKose50x50"].ToString() : string.Empty;
                    DemirKose30x30.Text = row["DemirKose30x30"] != DBNull.Value ? row["DemirKose30x30"].ToString() : string.Empty;
                    SilmeDemir3x30.Text = row["SilmeDemir3x30"] != DBNull.Value ? row["SilmeDemir3x30"].ToString() : string.Empty;
                    SilmeDemir5x50.Text = row["SilmeDemir5x50"] != DBNull.Value ? row["SilmeDemir5x50"].ToString() : string.Empty;
                    SilmeDemir10x20.Text = row["SilmeDemir10x20"] != DBNull.Value ? row["SilmeDemir10x20"].ToString() : string.Empty;
                    SilmeDemir10x30.Text = row["SilmeDemir10x30"] != DBNull.Value ? row["SilmeDemir10x30"].ToString() : string.Empty;
                    MDF8x1830x1830.Text = row["MDF8x1830x1830"] != DBNull.Value ? row["MDF8x1830x1830"].ToString() : string.Empty;
                    MDF18x1830x1830.Text = row["MDF18x1830x1830"] != DBNull.Value ? row["MDF18x1830x1830"].ToString() : string.Empty;
                    Polycarbonsolid4x1000x2000.Text = row["Polycarbonsolid4x1000x2000"] != DBNull.Value ? row["Polycarbonsolid4x1000x2000"].ToString() : string.Empty;
                    DemirSacDelikli.Text = row["DemirSacDelikli"] != DBNull.Value ? row["DemirSacDelikli"].ToString() : string.Empty;
                    DemirSacDuz.Text = row["DemirSacDuz"] != DBNull.Value ? row["DemirSacDuz"].ToString() : string.Empty;
                }
                else //Veri girilmemiş kayit yok 
                {
                    MessageBox.Show("Daha Önce Kayıt Bulunamadı. Birim fiyat giriniz.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("En son girilen birim fiyatlar getirilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                if (connectionString.State != ConnectionState.Closed)
                    connectionString.Close();
            }
        }
        #endregion

        /*
        #region DigerEkleButon
        private void EkleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                connectionString.Open();

                // Malzeme adı ve fiyat değerleri
                string malzemeAdi = DigerMalz.Text;
                string fiyat = DigerFiyat.Text;

                if (string.IsNullOrWhiteSpace(malzemeAdi))
                {
                    MessageBox.Show("Malzeme adı boş olamaz.");
                    return;
                }

                // Malzeme adıyla veritabanında sütun ekleme sorgusu
                string queryAddColumn = @"";

                SqlCommand commandAddColumn = new SqlCommand(queryAddColumn, connectionString);
                commandAddColumn.Parameters.AddWithValue("@", malzemeAdi);
                commandAddColumn.ExecuteNonQuery();

                // Fiyat değeri varsa ekle sütuna:
                if (!string.IsNullOrWhiteSpace(fiyat))
                {
                    string insertValueQuery =@"";
                    SqlCommand insertValueCommand = new SqlCommand(insertValueQuery, connectionString);
                    
                    insertValueCommand.Parameters.AddWithValue("@",string.IsNullOrWhiteSpace(fiyat) ? DBNull.Value : (object)fiyat);
                    insertValueCommand.ExecuteNonQuery();
                }
                else
                {
                    // Eğer fiyat girilmemişse NULL olarak kaydet
                    string insertNullValueQuery = @"";
                    SqlCommand insertNullValueCommand = new SqlCommand(insertNullValueQuery, connectionString);
                    insertNullValueCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Malzeme ve fiyat bilgisi veritabanına başarıyla kaydedildi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("'Malzeme ve fiyat' veritabanına kaydedilirken hata oluştu: " + ex.Message);
            }
            finally
            {
                connectionString.Close();
            }
        }
        #endregion
        */

        private void EkleBtn_Click(object sender, EventArgs e)
        {

        }

        //Girilen bilgileri SQL veritabanına aktar.
        #region SQL kaydet
        private void KaydetButon_Click(object sender, System.EventArgs e)
        {
            try
            {
                connectionString.Open();

                // SQL sorgusu
                string query = @"INSERT INTO MalzBirimFiyatTB (YoneticiID,
                                                    İsim, 
                                                    Soyisim, 
                                                    KayitTarihi,
                                                    İmalatCeligi,
                                                    TakimCeligi,
                                                    Pirinc,
                                                    Stavax,
                                                    Aluminyum,
                                                    PaslanmazCelik,
                                                    PoliamidKestamit,
                                                    Bakir,
                                                    Tungsten,
                                                    ApmcoYerli,
                                                    AmpcoIthal,
                                                    Vulkolon,
                                                    Elastomer,
                                                    AluKosebent,
                                                    AluProf45x45,
                                                    AluProf30x30,
                                                    AluProf25x20,
                                                    DemirProf50x50,
                                                    DemirProf40x40,
                                                    DemirProf30x30,
                                                    DemirProf25x25,
                                                    DemirKose50x50,
                                                    DemirKose30x30,
                                                    SilmeDemir3x30,
                                                    SilmeDemir5x50,
                                                    SilmeDemir10x20,
                                                    SilmeDemir10x30,
                                                    MDF8x1830x1830,
                                                    MDF18x1830x1830,
                                                    Polycarbonsolid4x1000x2000,
                                                    DemirSacDelikli,
                                                    DemirSacDuz)


                                VALUES  (@YoneticiID,
                                         @İsim, 
                                         @Soyisim, 
                                         GETDATE(), 
                                         @İmalatCeligi,
                                         @TakimCeligi,
                                         @Pirinc,
                                         @Stavax,
                                         @Aluminyum,
                                         @PaslanmazCelik,
                                         @PoliamidKestamit,
                                         @Bakir,
                                         @Tungsten,
                                         @ApmcoYerli,           
                                         @AmpcoIthal,           
                                         @Vulkolon,
                                         @Elastomer,
                                         @AluKosebent,
                                         @AluProf45x45,
                                         @AluProf30x30,
                                         @AluProf25x20,
                                         @DemirProf50x50,
                                         @DemirProf40x40,
                                         @DemirProf30x30,
                                         @DemirProf25x25,
                                         @DemirKose50x50,
                                         @DemirKose30x30,
                                         @SilmeDemir3x30,
                                         @SilmeDemir5x50,
                                         @SilmeDemir10x20,
                                         @SilmeDemir10x30,
                                         @MDF8x1830x1830,
                                         @MDF18x1830x1830,
                                         @Polycarbonsolid4x1000x2000,
                                         @DemirSacDelikli,
                                         @DemirSacDuz)";

                SqlCommand command = new SqlCommand(query, connectionString);

                // Yönetici Bilgileri:
                command.Parameters.AddWithValue("@YoneticiID", string.IsNullOrWhiteSpace(YoneticiID.Text) ? DBNull.Value : (object)YoneticiID.Text);
                command.Parameters.AddWithValue("@İsim", string.IsNullOrWhiteSpace(İsim.Text) ? DBNull.Value : (object)İsim.Text);
                command.Parameters.AddWithValue("@Soyisim", string.IsNullOrWhiteSpace(Soyisim.Text) ? DBNull.Value : (object)Soyisim.Text);

                // Birim Fiyatlar:
                command.Parameters.AddWithValue("@İmalatCeligi", string.IsNullOrWhiteSpace(İmalatCeligi.Text) ? DBNull.Value : (object)İmalatCeligi.Text);
                command.Parameters.AddWithValue("@TakimCeligi", string.IsNullOrWhiteSpace(TakimCeligi.Text) ? DBNull.Value : (object)TakimCeligi.Text);
                command.Parameters.AddWithValue("@Pirinc", string.IsNullOrWhiteSpace(Pirinc.Text) ? DBNull.Value : (object)Pirinc.Text);
                command.Parameters.AddWithValue("@Stavax", string.IsNullOrWhiteSpace(Stavax.Text) ? DBNull.Value : (object)Stavax.Text);
                command.Parameters.AddWithValue("@Aluminyum", string.IsNullOrWhiteSpace(Aluminyum.Text) ? DBNull.Value : (object)Aluminyum.Text);
                command.Parameters.AddWithValue("@PaslanmazCelik", string.IsNullOrWhiteSpace(PaslanmazCelik.Text) ? DBNull.Value : (object)PaslanmazCelik.Text);
                command.Parameters.AddWithValue("@PoliamidKestamit", string.IsNullOrWhiteSpace(PoliamidKestamit.Text) ? DBNull.Value : (object)PoliamidKestamit.Text);
                command.Parameters.AddWithValue("@Bakir", string.IsNullOrWhiteSpace(Bakir.Text) ? DBNull.Value : (object)Bakir.Text);
                command.Parameters.AddWithValue("@Tungsten", string.IsNullOrWhiteSpace(Tungsten.Text) ? DBNull.Value : (object)Tungsten.Text);
                command.Parameters.AddWithValue("@ApmcoYerli", string.IsNullOrWhiteSpace(ApmcoYerli.Text) ? DBNull.Value : (object)ApmcoYerli.Text);
                command.Parameters.AddWithValue("@AmpcoIthal", string.IsNullOrWhiteSpace(AmpcoIthal.Text) ? DBNull.Value : (object)AmpcoIthal.Text);
                command.Parameters.AddWithValue("@Vulkolon", string.IsNullOrWhiteSpace(Vulkolon.Text) ? DBNull.Value : (object)Vulkolon.Text);
                command.Parameters.AddWithValue("@Elastomer", string.IsNullOrWhiteSpace(Elastomer.Text) ? DBNull.Value : (object)Elastomer.Text);
                command.Parameters.AddWithValue("@AluKosebent", string.IsNullOrWhiteSpace(AluKosebent.Text) ? DBNull.Value : (object)AluKosebent.Text);
                command.Parameters.AddWithValue("@AluProf45x45", string.IsNullOrWhiteSpace(AluProf45x45.Text) ? DBNull.Value : (object)AluProf45x45.Text);
                command.Parameters.AddWithValue("@AluProf30x30", string.IsNullOrWhiteSpace(AluProf30x30.Text) ? DBNull.Value : (object)AluProf30x30.Text);
                command.Parameters.AddWithValue("@AluProf25x20", string.IsNullOrWhiteSpace(AluProf25x20.Text) ? DBNull.Value : (object)AluProf25x20.Text);
                command.Parameters.AddWithValue("@DemirProf50x50", string.IsNullOrWhiteSpace(DemirProf50x50.Text) ? DBNull.Value : (object)DemirProf50x50.Text);
                command.Parameters.AddWithValue("@DemirProf40x40", string.IsNullOrWhiteSpace(DemirProf40x40.Text) ? DBNull.Value : (object)DemirProf40x40.Text);
                command.Parameters.AddWithValue("@DemirProf30x30", string.IsNullOrWhiteSpace(DemirProf30x30.Text) ? DBNull.Value : (object)DemirProf30x30.Text);
                command.Parameters.AddWithValue("@DemirProf25x25", string.IsNullOrWhiteSpace(DemirProf25x25.Text) ? DBNull.Value : (object)DemirProf25x25.Text);
                command.Parameters.AddWithValue("@DemirKose50x50", string.IsNullOrWhiteSpace(DemirKose50x50.Text) ? DBNull.Value : (object)DemirKose50x50.Text);
                command.Parameters.AddWithValue("@DemirKose30x30", string.IsNullOrWhiteSpace(DemirKose30x30.Text) ? DBNull.Value : (object)DemirKose30x30.Text);
                command.Parameters.AddWithValue("@SilmeDemir3x30", string.IsNullOrWhiteSpace(SilmeDemir3x30.Text) ? DBNull.Value : (object)SilmeDemir3x30.Text);
                command.Parameters.AddWithValue("@SilmeDemir5x50", string.IsNullOrWhiteSpace(SilmeDemir5x50.Text) ? DBNull.Value : (object)SilmeDemir5x50.Text);
                command.Parameters.AddWithValue("@SilmeDemir10x20", string.IsNullOrWhiteSpace(SilmeDemir10x20.Text) ? DBNull.Value : (object)SilmeDemir10x20.Text);
                command.Parameters.AddWithValue("@SilmeDemir10x30", string.IsNullOrWhiteSpace(SilmeDemir10x30.Text) ? DBNull.Value : (object)SilmeDemir10x30.Text);
                command.Parameters.AddWithValue("@MDF8x1830x1830", string.IsNullOrWhiteSpace(MDF8x1830x1830.Text) ? DBNull.Value : (object)MDF8x1830x1830.Text);
                command.Parameters.AddWithValue("@MDF18x1830x1830", string.IsNullOrWhiteSpace(MDF18x1830x1830.Text) ? DBNull.Value : (object)MDF18x1830x1830.Text);
                command.Parameters.AddWithValue("@Polycarbonsolid4x1000x2000", string.IsNullOrWhiteSpace(Polycarbonsolid4x1000x2000.Text) ? DBNull.Value : (object)Polycarbonsolid4x1000x2000.Text);
                command.Parameters.AddWithValue("@DemirSacDelikli", string.IsNullOrWhiteSpace(DemirSacDelikli.Text) ? DBNull.Value : (object)DemirSacDelikli.Text);
                command.Parameters.AddWithValue("@DemirSacDuz", string.IsNullOrWhiteSpace(DemirSacDuz.Text) ? DBNull.Value : (object)DemirSacDuz.Text);


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
    }
}
