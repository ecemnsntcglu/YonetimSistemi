using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YonetimSistemi
{
    public partial class EtkinlikEkle : Form
    {
        public EtkinlikEkle()
        {
            InitializeComponent();
        }

        private void button_ekle_Click(object sender, EventArgs e)
        {

            NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=iK663746");
            string connString = "server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=iK663746";
            baglanti.Open();
            NpgsqlCommand komut1 = new NpgsqlCommand("SELECT salon_no FROM salonlar WHERE ad=@ad", baglanti);
            komut1.Parameters.AddWithValue("@ad", domainUpDown_salon.Text);
            NpgsqlDataReader oku = komut1.ExecuteReader();


            string etkinlikAd = textBox_etkinlikAdi.Text;
            DateTime etkinlikTarihi = dateTimePicker.Value;
            string etkinlikTuru = domainUpDown_tür.Text;
            string etkinlikSalon = oku["salon_no"].ToString();
            baglanti.Close();
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    // Bağlantıyı aç
                    conn.Open();

                    // SQL sorgusunu oluştur
                    string sql = "INSERT INTO etkinlikler (ad,tarih, etkinlik_turu,salon_no) " +
                                 "VALUES (@etkinlikAd, @etkinlikTarihi, @etkinlikTuru,@etkinlikSalon)";

                    // Sorguyu hazırlamak için komut nesnesi oluştur
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        // Parametreleri ekle
                        cmd.Parameters.AddWithValue("ad", etkinlikAd);
                        cmd.Parameters.AddWithValue("tarih", etkinlikTarihi);
                        cmd.Parameters.AddWithValue("etkinlik_turu", etkinlikTuru);
                        cmd.Parameters.AddWithValue("salon_no", etkinlikSalon);
                        // Sorguyu çalıştır
                        cmd.ExecuteNonQuery();
                    }

                    // Başarı mesajı göster
                    MessageBox.Show("Etkinlik başarıyla eklendi!");
                }
                catch (Exception ex)
                {
                    // Hata durumunda mesaj göster
                    MessageBox.Show("Hata:Eklemede problem oluştu. " + ex.Message);
                }
            }
        }

    }
}
