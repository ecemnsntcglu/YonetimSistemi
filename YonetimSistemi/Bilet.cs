using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;

namespace YonetimSistemi
{
    public partial class Bilet : Form
    {
        private readonly string _etkinlikAdi;
        private readonly string _etkinlik_tur;
        private readonly string _tarih;
        private readonly string _sehir;
        private readonly string _salonAdi;
        System.Windows.Forms.Label label3 = new System.Windows.Forms.Label();
        private List<int> selectedKoltukIds = new List<int>();

        int biletsayisi;

        public Bilet(string etkinlikAdi, string etkinlik_tur, string tarih, string sehir, string salonAdi)
        {
            _etkinlikAdi = etkinlikAdi.ToUpper();
            _etkinlik_tur = etkinlik_tur;
            _tarih = tarih;
            _sehir = sehir.ToUpper();
           
            _salonAdi = salonAdi;
           

            InitializeComponent();
            LoadBiletDetails();
            InitializeSections(_salonAdi);
        }

        private void LoadBiletDetails()
        {
            this.Text = $"Bilet - {_etkinlikAdi} - {_tarih}";

            // Etkinlik bilgilerini label üzerinde göster
            label1.Text = $"Etkinlik: {_etkinlikAdi}\nTarih: {_tarih}\nŞehir: {_sehir}\nSalon: {_salonAdi.ToUpper()}";
            label1.Font = new Font(label1.Font.FontFamily, 7, FontStyle.Bold);

            // Resmi atama
            switch (_etkinlik_tur.ToLower())
            {
                case "konser":
                    pictureBox1.Image = ımageList1.Images[0];
                    break;
                case "film":
                    pictureBox1.Image = ımageList1.Images[1];
                    break;
                case "tiyatro":
                    pictureBox1.Image = ımageList1.Images[1];
                    break;
                default:
                    pictureBox1.Image = null;
                    break;
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void InitializeSections(string salonAd)
        {
            string connString = "Host=localhost; Port=5432; Database=dbYonetim; User ID=postgres; Password=iK663746";
            string query = @"
        SELECT DISTINCT bolum 
        FROM koltuk_duzeni 
        WHERE salon_no = (SELECT salon_no FROM salonlar WHERE salon_ad = @salonAd)
        ORDER BY bolum";  // Bölümleri alfabetik sıralıyoruz

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@salonAd", salonAd);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("Bu salona ait hiçbir bölüm bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; // Fonksiyondan çık, çünkü daha fazla işleme gerek yok
                            }

                            int posX = 20, posY = 20, buttonSize = 60;
                            int padding = 10;

                            while (reader.Read())
                            {
                                string section = reader["bolum"].ToString();

                                Button sectionButton = new Button
                                {
                                    Width = buttonSize,
                                    Height = buttonSize,
                                    Text = section,
                                    Tag = section,
                                    Location = new Point(posX, posY)
                                };

                                sectionButton.Click += SectionButton_Click;
                                panel1.Controls.Add(sectionButton);

                                posX += buttonSize + padding;
                                if (posX + buttonSize > panel1.ClientSize.Width)
                                {
                                    posX = 20;
                                    posY += buttonSize + padding;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SectionButton_Click(object sender, EventArgs e)
        {
            if (sender is Button clickedButton && clickedButton.Tag is string section)
            {
                LoadKoltukDuzeni(section);
            }
        }

        private void LoadKoltukDuzeni(string section)
        {
             biletsayisi = 0;
            panel1.Visible = false;  // Bölüm butonları panelini gizle
            pictureBox1.Visible = false;
            label1.Visible = false;

            panel2.Controls.Clear();  // panel2'yi temizle

            // Yeni bölüm adını gösteren bir etiket ekle
            System.Windows.Forms.Label label2 = new System.Windows.Forms.Label();
            label2.Text = "BÖLÜM " + section;
            label2.Location = new Point(5, 5);
            panel2.Controls.Add(label2);

            string connString = "Host=localhost; Port=5432; Database=dbYonetim; User ID=postgres; Password=iK663746";
            string query = @"
        SELECT b.bilet_id,k.sira_no, k.koltuk_no, b.durum
        FROM biletler b
        JOIN koltuk_duzeni k ON b.koltuk_id = k.koltuk_id
        WHERE b.etkinlik_id = (SELECT etkinlik_id FROM etkinlikler WHERE ad = @_etkinlikAdi)
          AND k.bolum = @section
        ORDER BY k.sira_no, k.koltuk_no";

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("_etkinlikAdi", _etkinlikAdi);
                        cmd.Parameters.AddWithValue("section", section);

                        using (var reader = cmd.ExecuteReader())
                        {
                            int posX = 15, posY = 30, buttonSize = 30;

                            while (reader.Read())
                            {
                                int _bilet_id= reader.GetInt32(reader.GetOrdinal("bilet_id"));
                                char siraNo = reader.GetChar(reader.GetOrdinal("sira_no"));
                                int koltukNo = reader.GetInt32(reader.GetOrdinal("koltuk_no"));
                                bool durum = reader.GetBoolean(reader.GetOrdinal("durum"));

                                // Koltuk bilgilerini Button'a ekleyip göster
                                Button koltukButton = new Button
                                { Name = $"{_bilet_id }",
                                    Width = buttonSize,
                                    Height = buttonSize,
                                    Text = $"{siraNo}-{koltukNo}",
                                    BackColor = durum ? Color.Red : Color.Green,
                                    Location = new Point(posX, posY)
                                };

                                koltukButton.Click += KoltukButton_Click;
                                panel2.Controls.Add(koltukButton);

                                posX += buttonSize + 1;
                                if (posX + buttonSize > panel2.ClientSize.Width)
                                {
                                    posX = 15;
                                    posY += buttonSize + 1;
                                }
                            }

                            // Geri dönme butonunu ekle
                            posX += 5;
                            posY += 5;
                            Button geri = new Button
                            {
                                Width = 170,
                                Height = 30,
                                Text = "SEÇİM PANELİNE DÖN",
                                BackColor = Color.LightGray,
                                Location = new Point(posX, posY)
                            };
                            Button ileri = new Button
                            {
                                Width = 170,
                                Height = 30,
                                Text = "SEÇİM PANELİNE DÖN",
                                BackColor = Color.LightGray,
                                Location = new Point(posX+5, posY)
                            };
                            geri.Click += geri_Click;
                            ileri.Click += ileri_Click;
                            panel2.Controls.Add(geri);
                            panel2.Controls.Add(ileri);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void geri_Click(object sender, EventArgs e)
        {
      
            panel2.Controls.Clear();
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(pictureBox1);
            panel1.Visible = true;
            panel1.Controls.Clear();
            InitializeSections(_salonAdi);
            pictureBox1.Visible = true;
            label1.Visible = true;
            LoadBiletDetails();
        }
        private void KoltukButton_Click(object sender, EventArgs e)
        {
           
            if (sender is Button clickedButton)
            {
                if (clickedButton.BackColor == Color.Green)
                {
                    biletsayisi++;
                    selectedKoltukIds.Add(Convert.ToInt32(clickedButton.Name));
                    clickedButton.BackColor = Color.Blue;
                }
                else if (clickedButton.BackColor == Color.Blue)
                {
                    biletsayisi--;
                    selectedKoltukIds.RemoveAll(n => n==Convert.ToInt32(clickedButton.Name));
                    clickedButton.BackColor = Color.Green;
                   
                }

                
                string biletBilgisi = string.Join(", ", selectedKoltukIds);
                label3.Location = new Point(100, 5);
                panel2.Controls.Add(label3);
                label3.Text ="bilet sayısı: "+biletsayisi + " SEÇİMLERİNİZ: " + biletBilgisi;
                

            }
        }
        private void ileri_Click(object sender, EventArgs e)
        {

            BiletOnay biletonay = new BiletOnay(selectedKoltukIds);
            biletonay.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
