using System;
using Npgsql; 
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace YonetimSistemi
{
    public partial class AdminMenu : Form
    {
        private string connectionString = "Host=localhost;Port=5432;Database=dbYonetim;User ID=postgres;Password=iK663746";
        private Label lblAd, lblSehir, lblTarih, lblEtkinlikTur1, lblSalonNo1, lblResim1;
        private TextBox txtAd, txtSehir, txtResimYolu1;
        private DateTimePicker dateTimePicker1;
        private Button btnGuncelle2, btnSil2, btnResimSec1,btnIptal2;
        private ComboBox cmbEtkinlikTur1, cmbSalonNo1;
       
       
        public AdminMenu()
        {
            InitializeComponent();
            InitializeUpdatePanel();
            LoadEtkinlikler();     
        }

       
        private void LoadEtkinlikler()
        {
            comboBox1.Items.Clear();
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT etkinlik_id, ad, sehir, tarih FROM etkinlikler";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string etkinlik = $"{reader["etkinlik_id"]} - {reader["ad"]} - {reader["sehir"]} - {Convert.ToDateTime(reader["tarih"]).ToShortDateString()}";
                            comboBox1.Items.Add(etkinlik);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }



        private void btn_ekle_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            int startY = 20;
            int startX = 20;

           
            Label lblAd = new Label
            {
                Text = "Etkinlik Adı:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblAd);

            
            TextBox txtAd = new TextBox
            {
                Name = "txtAd",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY)
            };
            panel1.Controls.Add(txtAd);

            startY += 40;

            
            Label lblSehir = new Label
            {
                Text = "Şehir:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblSehir);

           
            TextBox txtSehir = new TextBox
            {
                Name = "txtSehir",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY)
            };
            panel1.Controls.Add(txtSehir);

            startY += 40;

           
            Label lblTarih = new Label
            {
                Text = "Tarih:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblTarih);

           
            DateTimePicker dateTimePicker1 = new DateTimePicker
            {
                Name = "dateTimePicker1",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY),
                Format = DateTimePickerFormat.Short
            };
            panel1.Controls.Add(dateTimePicker1);

            startY += 40;

            
            Label lblEtkinlikTur = new Label
            {
                Text = "Etkinlik Türü:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblEtkinlikTur);

           
            ComboBox cmbEtkinlikTur = new ComboBox
            {
                Name = "cmbEtkinlikTur",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEtkinlikTur.Items.AddRange(new object[] { "tiyatro", "sinema", "konser" });
            panel1.Controls.Add(cmbEtkinlikTur);

            startY += 40;

            Label lblSalonNo = new Label
            {
                Text = "Salon No:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblSalonNo);

           
            ComboBox cmbSalonNo = new ComboBox
            {
                Name = "cmbSalonNo",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel1.Controls.Add(cmbSalonNo);

           using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT salon_no, salon_ad FROM salonlar";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbSalonNo.Items.Add($"{reader["salon_no"]} - {reader["salon_ad"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Salonlar yüklenirken hata oluştu: " + ex.Message);
                }
            }

            startY += 40;

            Label lblResim = new Label
            {
                Text = "Resim Yolu:",
                Location = new Point(startX, startY),
                AutoSize = true
            };
            panel1.Controls.Add(lblResim);

            TextBox txtResimYolu = new TextBox
            {
                Name = "txtResimYolu",
                Size = new Size(200, 25),
                Location = new Point(startX + 130, startY)
            };
            panel1.Controls.Add(txtResimYolu);

           
            Button btnResimSec = new Button
            {
                Text = "Resim Seç",
                Location = new Point(startX + 340, startY - 5),
                Size = new Size(100, 30)
            };
            btnResimSec.Click += (s, ev) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Resim Seç",
                    Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFilePath);
                    string destinationFilePath = Path.Combine(@"C:\Users\irem\source\repos\YonetimSistemi2\YonetimSistemi\dbYonetim", fileName);

                    try
                    {
                       
                        File.Copy(sourceFilePath, destinationFilePath, true);
                        txtResimYolu.Text = destinationFilePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Resim kopyalanırken hata oluştu: " + ex.Message);
                    }
                }
            };
            panel1.Controls.Add(btnResimSec);

            startY += 40;

           Button btnKaydet = new Button
            {
                Text = "Kaydet",
                Location = new Point(startX, startY),
                Size = new Size(100, 30)
            };
            btnKaydet.Click += (s, ev) =>
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO etkinlikler (ad, sehir, tarih, resim_yolu, etkinlik_tur, salon_no) VALUES (@ad, @sehir, @tarih, @resim_yolu, @etkinlik_tur, @salon_no)";
                        using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ad", txtAd.Text.Trim());
                            cmd.Parameters.AddWithValue("@sehir", txtSehir.Text.Trim());
                            cmd.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@resim_yolu", txtResimYolu.Text.Trim());
                            cmd.Parameters.AddWithValue("@etkinlik_tur", cmbEtkinlikTur.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@salon_no", int.Parse(cmbSalonNo.SelectedItem.ToString().Split('-')[0].Trim()));

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Etkinlik başarıyla eklendi.");
                                LoadEtkinlikler();
                            }
                            else
                            {
                                MessageBox.Show("Etkinlik ekleme başarısız.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hata: " + ex.Message);
                    }
                }
            };
            panel1.Controls.Add(btnKaydet);
            Button btnIptal = new Button
            {
                Text = "İptal",
                Location = new Point(startX+120, startY),
                Size = new Size(100, 30)
            };
            panel1.Controls.Add(btnIptal);
            btnIptal.Click += (s, ev) => {
               panel1.Controls.Clear();
                InitializeUpdatePanel();
            };
        }


        private void InitializeUpdatePanel()
        {
            int startY = 20;
           
          
            lblAd = new Label
            {
                Text = "Etkinlik Adı:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblAd);

          
            txtAd = new TextBox
            {
                Name = "txtAd",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                Visible = false
            };
            panel1.Controls.Add(txtAd);

            startY += 40;

            
            lblSehir = new Label
            {
                Text = "Şehir:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblSehir);

           
            txtSehir = new TextBox
            {
                Name = "txtSehir",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                Visible = false
            };
            panel1.Controls.Add(txtSehir);

            startY += 40;

          
            lblTarih = new Label
            {
                Text = "Tarih:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblTarih);

           
            dateTimePicker1 = new DateTimePicker
            {
                Name = "dateTimePicker1",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                Format = DateTimePickerFormat.Short,
                Visible = false
            };
            panel1.Controls.Add(dateTimePicker1);
            startY += 40;
           
            lblEtkinlikTur1 = new Label
            {
                Text = "Etkinlik Türü:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblEtkinlikTur1);


            cmbEtkinlikTur1 = new ComboBox
            {
                Name = "cmbEtkinlikTur",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            cmbEtkinlikTur1.Items.AddRange(new object[] { "tiyatro", "sinema", "konser" });
            panel1.Controls.Add(cmbEtkinlikTur1);

            startY += 40;

            lblSalonNo1 = new Label
            {
                Text = "Salon No:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblSalonNo1);

          
            cmbSalonNo1 = new ComboBox
            {
                Name = "cmbSalonNo",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            panel1.Controls.Add(cmbSalonNo1);

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT salon_no, salon_ad FROM salonlar";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbSalonNo1.Items.Add($"{reader["salon_no"]} - {reader["salon_ad"]}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Salonlar yüklenirken hata oluştu: " + ex.Message);
                }
            }

            startY += 40;

            lblResim1 = new Label
            {
                Text = "Resim Yolu:",
                Location = new Point(20, startY),
                AutoSize = true,
                Visible = false
            };
            panel1.Controls.Add(lblResim1);

            txtResimYolu1 = new TextBox
            {
                Name = "txtResimYolu",
                Size = new Size(150, 25),
                Location = new Point(140, startY),
                Visible = false
            };
            panel1.Controls.Add(txtResimYolu1);

            btnResimSec1 = new Button
            {
                Text = "Resim Seç",
                Location = new Point(150 + 145, startY - 5),
                Size = new Size(100, 30),
                Visible = false
            };
            btnResimSec1.Click += (s, ev) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Title = "Resim Seç",
                    Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sourceFilePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourceFilePath);
                    string destinationFilePath = Path.Combine(@"C:\Users\irem\source\repos\YonetimSistemi2\YonetimSistemi\dbYonetim", fileName);

                    try
                    {
                        File.Copy(sourceFilePath, destinationFilePath, true);
                        txtResimYolu1.Text = destinationFilePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Resim kopyalanırken hata oluştu: " + ex.Message);
                    }
                }
            };
            panel1.Controls.Add(btnResimSec1);

            startY += 40;

            btnGuncelle2 = new Button
            {
                Text = "Güncelle",
                Location = new Point(20, startY + 20),
                Size = new Size(100, 30),
                Visible = false
            };
            btnGuncelle2.Click += btn_guncelle2_Click;
            panel1.Controls.Add(btnGuncelle2);

            btnSil2 = new Button
            {
                Text = "Sil",
                Location = new Point(130, startY + 20),
                Size = new Size(100, 30),
                Visible = false
            };
            btnSil2.Click += btn_sil2_Click;
            panel1.Controls.Add(btnSil2);

            btnIptal2 = new Button
            {
                Text = "İptal",
                Location = new Point(240, startY + 20),
                Size = new Size(100, 30),
                Visible = false
            };
            btnIptal2.Click += btn_iptal_Click;
            panel1.Controls.Add(btnIptal2);
        }

        private void btn_iptal_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            InitializeUpdatePanel();
        }

        private void btn_guncelle_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir etkinlik seçiniz.");
                return;
            }

            string selectedItem = comboBox1.SelectedItem.ToString();
            string etkinlikId = selectedItem.Split('-')[0].Trim();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ad, e.sehir, tarih, etkinlik_tur, e.salon_no, resim_yolu, s.salon_ad FROM etkinlikler e JOIN salonlar s ON e.salon_no = s.salon_no WHERE etkinlik_id = @etkinlik_id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@etkinlik_id", Convert.ToInt32(etkinlikId));
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtAd.Text = reader["ad"].ToString();
                                txtSehir.Text = reader["sehir"].ToString();
                                dateTimePicker1.Value = Convert.ToDateTime(reader["tarih"]);
                                cmbEtkinlikTur1.SelectedItem = reader["etkinlik_tur"].ToString();
                                cmbSalonNo1.SelectedItem = $"{reader["salon_no"]} - {reader["salon_ad"]}";
                                txtResimYolu1.Text = reader["resim_yolu"].ToString();
                            }
                        }
                    }

                    lblAd.Visible = true;
                    lblEtkinlikTur1.Visible = true;
                    cmbEtkinlikTur1.Visible = true;
                    lblSalonNo1.Visible = true;
                    cmbSalonNo1.Visible = true;
                    lblResim1.Visible = true;
                    txtResimYolu1.Visible = true;
                    btnResimSec1.Visible = true;
                    txtAd.Visible = true;
                    lblSehir.Visible = true;
                    txtSehir.Visible = true;
                    lblTarih.Visible = true;
                    dateTimePicker1.Visible = true;
                    btnGuncelle2.Visible = true;
                    btnSil2.Visible = true;
                    btnIptal2.Visible = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }


       
        private void btn_guncelle2_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            string etkinlikId = selectedItem.Split('-')[0].Trim();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE etkinlikler SET ad = @ad, sehir = @sehir, tarih = @tarih WHERE etkinlik_id = @etkinlik_id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", txtAd.Text.Trim());
                        cmd.Parameters.AddWithValue("@sehir", txtSehir.Text.Trim());
                        cmd.Parameters.AddWithValue("@tarih", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@etkinlik_id", Convert.ToInt32(etkinlikId));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Etkinlik başarıyla güncellendi.");
                            LoadEtkinlikler();
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme başarısız.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

      
        private void btn_sil2_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            string etkinlikId = selectedItem.Split('-')[0].Trim();

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM etkinlikler WHERE etkinlik_id = @etkinlik_id";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@etkinlik_id", Convert.ToInt32(etkinlikId));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Etkinlik başarıyla silindi.");
                            LoadEtkinlikler();
                        }
                        else
                        {
                            MessageBox.Show("Silme işlemi başarısız.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void btn_cikis_Click(object sender, EventArgs e)
        {
            GirisPaneli giris = new GirisPaneli();
            giris.Show();
            this.Close();
        }
    }
}
