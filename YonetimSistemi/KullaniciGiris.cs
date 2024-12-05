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
    public partial class KullaniciGiris : Form
    {
        public KullaniciGiris()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=22181617007");
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand("SELECT password FROM kullanicilar WHERE username=@username", baglanti);
                komut1.Parameters.AddWithValue("@username", txtKullanici.Text);
                NpgsqlDataReader oku = komut1.ExecuteReader();

                if (oku.Read())
                {
                    string storedPassword = oku["password"].ToString();
                    if (storedPassword == txtSifre.Text)
                    {
                        MessageBox.Show("Giriş başarılı!");
                        Menu menu = new Menu();
                        menu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Hatalı şifre!");
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı bulunamadı!");
                }

                oku.Close();
                komut1.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}");
            }
            finally
            {
                baglanti.Close();
            }

        }

        private void btnKaydol_Click(object sender, EventArgs e)
        {

            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }

                // Kullanıcı adı kontrolü
                string kontrol = "SELECT username FROM kullanicilar WHERE username = @username";
                using (NpgsqlCommand komutKontrol = new NpgsqlCommand(kontrol, baglanti))
                {
                    komutKontrol.Parameters.AddWithValue("@username", txtKullanici.Text.Trim());

                    using (NpgsqlDataReader oku = komutKontrol.ExecuteReader())
                    {
                        if (oku.Read()) // username zaten var
                        {
                            MessageBox.Show("Bu kullanıcı adı zaten alınmış!");
                            return; 
                        }
                    }
                }

                // Yeni kullanıcı ekleme
                string kullaniciEkle = "INSERT INTO kullanicilar (username, password) VALUES (@username, @password)";
                using (NpgsqlCommand eklemeKomutu = new NpgsqlCommand(kullaniciEkle, baglanti))
                {
                    eklemeKomutu.Parameters.AddWithValue("@username", txtKullanici.Text.Trim());
                    eklemeKomutu.Parameters.AddWithValue("@password", txtSifre.Text.Trim());
                    eklemeKomutu.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarılı!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            GirisPaneli donus = new GirisPaneli();
            donus.Show();
            this.Hide();

        }
    }
}
