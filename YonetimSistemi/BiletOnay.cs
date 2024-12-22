using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace YonetimSistemi
{
    public partial class BiletOnay : Form
    {
        private readonly List<int> _selectedKoltukIds;
        private string connectionString = "Host=localhost;Port=5432;Database=dbYonetim;User ID=postgres;Password=iK663746";
        public BiletOnay(List<int> selectedKoltukIds)
        {
           
           InitializeComponent();
            _selectedKoltukIds = selectedKoltukIds;
            InitializePanel();

        }

        private void InitializePanel()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ad, soyad, tel, email FROM kullanicilar WHERE kullanici_id = @kullanici_id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kullanici_id", GlobalVariables.kullanici_id);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                textAd.Text = reader["ad"].ToString();
                                textSoyad.Text = reader["soyad"].ToString();
                                textTel.Text = reader["tel"].ToString();
                                textMail.Text = reader["email"].ToString();
                            }
                        }
                    }

                    
                    query = "SELECT bilet_id, fiyat FROM biletler WHERE bilet_id = ANY(@bilet_ids)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                       
                        cmd.Parameters.AddWithValue("@bilet_ids", _selectedKoltukIds.ToArray());
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                           int bilet_sayisi=0;
                            decimal toplamFiyat = 0;

                            while (reader.Read())
                            {
                             bilet_sayisi++;
                                toplamFiyat += reader.GetDecimal(reader.GetOrdinal("fiyat"));
                            }

                            textBiletler.Text = bilet_sayisi.ToString();
                            textFiyat.Text = toplamFiyat.ToString("C"); 
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }


        private void btn_onayla_Click(object sender, EventArgs e)
        {
            if (_selectedKoltukIds == null || _selectedKoltukIds.Count == 0)
            {
                MessageBox.Show("Seçili bilet bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connString = "Host=localhost; Port=5432; Database=dbYonetim; User ID=postgres; Password=iK663746";
            int kullanici_id = GlobalVariables.kullanici_id; 
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "UPDATE biletler SET kullanici_id = @kullanici_id, satis_tarihi = @satis_tarihi, durum = true WHERE bilet_id = ANY(@biletIds)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kullanici_id", kullanici_id);
                        cmd.Parameters.AddWithValue("@satis_tarihi", DateTime.Now);
                        cmd.Parameters.AddWithValue("@biletIds", _selectedKoltukIds.ToArray());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        MessageBox.Show($"{rowsAffected} bilet başarıyla onaylandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button_geri_Click(object sender, EventArgs e)
        {
            this.Close();
          
        }
    }
}
