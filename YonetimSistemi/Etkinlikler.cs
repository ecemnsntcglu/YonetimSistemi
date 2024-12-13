using Npgsql;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace YonetimSistemi
{
    public partial class Etkinlikler : Form
    {
        private string _filter;
        private FlowLayoutPanel _panel;
        string etkinlik_tur;

        public Etkinlikler(string filter)
        {
            _filter = filter.ToLower();
            InitializeComponent();
            InitializePanel();
            LoadEvents();
        }

        private void InitializePanel()
        {
            _panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(_panel);
        }

        private void LoadEvents()
        {
            string connString = "Host=localhost; Port=5432; Database=dbYonetim; User ID=postgres; Password=iK663746";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = @"
                    SELECT e.*, s.salon_ad, COALESCE(MIN(b.fiyat), 0) AS min_fiyat
                    FROM etkinlikler e
                    LEFT JOIN salonlar s ON e.salon_no = s.salon_no
                    LEFT JOIN biletler b ON e.etkinlik_id = b.etkinlik_id
                    WHERE e.sehir = @filter OR e.etkinlik_tur = @filter
                    GROUP BY e.etkinlik_id, s.salon_ad";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("filter", _filter);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string etkinlikAdi = reader["ad"].ToString();
                             etkinlik_tur = reader["etkinlik_tur"].ToString();
                            string tarih = reader["tarih"].ToString();
                            string sehir = reader["sehir"].ToString();
                            string resimYolu = reader["resim_yolu"].ToString();
                            string salonAdi = reader["salon_ad"].ToString();
                            decimal enDusukFiyat = reader.GetDecimal(reader.GetOrdinal("min_fiyat"));
                            CreateEventButton(etkinlikAdi, tarih, sehir, resimYolu, salonAdi, enDusukFiyat);
                        }
                    }
                }
            }
        }

        private void CreateEventButton(string etkinlikAdi, string tarih, string sehir, string resimYolu, string salonAdi, decimal enDusukFiyat)
        {
            Button eventButton = new Button
            {
                Width = 110,
                Height = 150,
                Tag = new { etkinlikAdi, tarih, sehir, resimYolu, salonAdi, enDusukFiyat }
            };

            if (File.Exists(resimYolu))
            {
                eventButton.BackgroundImage = Image.FromFile(resimYolu);
                eventButton.BackgroundImageLayout = ImageLayout.Stretch;
            }

            Label label = new Label
            {
                Text = $"{etkinlikAdi}\n{tarih}\n{sehir}\nSalon: {salonAdi}\nFiyat: {enDusukFiyat:C}",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            eventButton.Click += new EventHandler(EventButton_Click);

            FlowLayoutPanel container = new FlowLayoutPanel
            {
                Width = 200,
                Height = 250,
                FlowDirection = FlowDirection.TopDown
            };

            container.Controls.Add(eventButton);
            container.Controls.Add(label);

            _panel.Controls.Add(container);
        }

        private void EventButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            var eventInfo = (dynamic)clickedButton.Tag;
            Etkinlik_Bilgi biletForm = new Etkinlik_Bilgi(eventInfo.etkinlikAdi, etkinlik_tur, eventInfo.tarih, eventInfo.sehir, eventInfo.resimYolu, eventInfo.salonAdi, eventInfo.enDusukFiyat);
            biletForm.ShowDialog();
        }
    }
}

    
