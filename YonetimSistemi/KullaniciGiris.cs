using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace YonetimSistemi
{
       public partial class KullaniciGiris : Form
    {
        private TextBox txtAd, txtSoyad, txtTel, txtEmail, txtUsername, txtSifre1; 
        private Button btnKaydet, btnIptal;
        public KullaniciGiris()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=iK663746");
        private void btnGirisYap_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand("SELECT password,kullanici_id FROM kullanicilar WHERE username=@username", baglanti);
                komut1.Parameters.AddWithValue("@username", txtKullanici.Text);
                NpgsqlDataReader oku = komut1.ExecuteReader();

                if (oku.Read())
                {
                    string storedPassword = oku["password"].ToString();
                    if (storedPassword == txtSifre.Text)
                    {
                        Menu menu = new Menu();
                        GlobalVariables.kullanici_id= Convert.ToInt32(oku["kullanici_id"]);
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
            panel1.Controls.Clear();
            panel1.AutoScroll = true;
            int startY = 20;

            panel1.Controls.Add(new Label { Text = "Kullanıcı Adı:", AutoSize = true, Location = new Point(20, startY) });
            txtUsername = new TextBox { Size = new Size(200, 25), Location = new Point(150,startY) };
            panel1.Controls.Add(txtUsername);
            startY += 20;
            panel1.Controls.Add(new Label { Text = "Ad:", AutoSize = true, Location = new Point(20, startY) }); 
            txtAd = new TextBox { Size = new Size(100, 25), Location = new Point(150, startY) }; 
            panel1.Controls.Add(txtAd);
            startY += 20;
            panel1.Controls.Add(
                new Label { Text = "Soyad:", AutoSize = true, Location = new Point(20, startY) }); 
            txtSoyad = new TextBox { Size = new Size(100, 25), Location = new Point(150, startY) }; 
            panel1.Controls.Add(txtSoyad);
            startY += 20;
            panel1.Controls.Add(new Label { Text = "Tel:", AutoSize = true, Location = new Point(20, startY) }); 
            txtTel = new TextBox { Size = new Size(100, 25), Location = new Point(150, startY) };
            panel1.Controls.Add(txtTel);
            startY += 20;
            panel1.Controls.Add(new Label { Text = "Email:", AutoSize = true, Location = new Point(20, startY) }); 
            txtEmail = new TextBox { Size = new Size(100, 25), Location = new Point(150, startY) };
            panel1.Controls.Add(txtEmail);
            startY += 20;
            panel1.Controls.Add(new Label { Text = "Şifre:", AutoSize = true, Location = new Point(20, startY) });
            txtSifre1 = new TextBox { Size = new Size(100, 25), Location = new Point(150, startY) };
            panel1.Controls.Add(txtSifre1);
            startY += 20;
            btnKaydet = new Button { Text = "Kaydet", Size = new Size(75, 30), Location = new Point(20, startY) };
            btnIptal = new Button { Text = "İptal", Size = new Size(75, 30), Location = new Point(100, startY) };
            btnKaydet.Click += BtnKaydet_Click;
            btnIptal.Click += BtnIptal_Click;
            panel1.Controls.Add(btnKaydet);
            panel1.Controls.Add(btnIptal);

           

        }

        private void BtnIptal_Click(object sender, EventArgs e)
        {
           KullaniciGiris giris = new KullaniciGiris();
            giris.Show();
            this.Close();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (baglanti.State == ConnectionState.Closed)
                {
                    baglanti.Open();
                }
                var textBoxes = new List<TextBox> { txtAd, txtSoyad, txtTel, txtEmail, txtUsername, txtSifre1 }; 
                foreach (var textBox in textBoxes) { 
                    if (string.IsNullOrWhiteSpace(textBox.Text)) 
                    { 
                        MessageBox.Show("Lütfen tüm alanları doldurun."); 
                        return; 
                    } 
                }

                string kontrol = "SELECT username,email FROM kullanicilar WHERE username = @username or email=@email";
                using (NpgsqlCommand komutKontrol = new NpgsqlCommand(kontrol, baglanti))
                {
                    komutKontrol.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                    komutKontrol.Parameters.AddWithValue("@email", txtEmail.Text.Trim());

                    using (NpgsqlDataReader oku = komutKontrol.ExecuteReader())
                    {
                        if (oku.Read())
                        {
                            MessageBox.Show("Bu kullanıcı adı veya mail zaten kayıtlı!");
                            return;
                        }
                    }
                }


                string kullaniciEkle = "INSERT INTO kullanicilar (username,ad,soyad,tel,email, password) VALUES (@username,@ad,@soyad,@tel,@email,@password)";
                using (NpgsqlCommand eklemeKomutu = new NpgsqlCommand(kullaniciEkle, baglanti))
                {
                    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo; 
                    eklemeKomutu.Parameters.AddWithValue("@username", txtUsername.Text.Trim()); 
                    eklemeKomutu.Parameters.AddWithValue("@ad", textInfo.ToTitleCase(txtAd.Text.Trim())); 
                    eklemeKomutu.Parameters.AddWithValue("@soyad", textInfo.ToTitleCase(txtSoyad.Text.Trim())); 
                    eklemeKomutu.Parameters.AddWithValue("@tel", txtTel.Text.Trim()); 
                    eklemeKomutu.Parameters.AddWithValue("@email", txtEmail.Text.Trim()); 
                    eklemeKomutu.Parameters.AddWithValue("@password", txtSifre1.Text.Trim());
                    eklemeKomutu.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarılı!");
                    GirisPaneli donus = new GirisPaneli();
                    donus.Show();
                    this.Close();
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
            this.Close();

        }
    }
}
