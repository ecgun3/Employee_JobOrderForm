using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace İs_Emri_Formu
{
    internal class DataStorage
    {
        private readonly string filePath = "jobDescriptions.json"; // JSON dosyasının kaydedileceği yer

        // Listeyi JSON dosyasına kaydet
        public void SaveJobDescriptions(List<string> jobDescriptions)
        {
            try
            {
                // Listeyi JSON formatına serileştir
                string jsonString = JsonConvert.SerializeObject(jobDescriptions, Formatting.Indented);

                // JSON dosyasına yaz
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler kaydedilirken bir hata oluştu: " + ex.Message);
            }
        }

        // JSON dosyasından listeyi yükle
        public List<string> LoadJobDescriptions()
        {
            try
            {
                // Eğer dosya mevcutsa
                if (File.Exists(filePath))
                {
                    // JSON dosyasını oku
                    string jsonString = File.ReadAllText(filePath);

                    // JSON'u listeye dönüştür
                    return JsonConvert.DeserializeObject<List<string>>(jsonString);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message);
            }

            // Eğer dosya yoksa ya da bir hata oluşursa boş bir liste döndür
            return new List<string>();
        }
    }
}
