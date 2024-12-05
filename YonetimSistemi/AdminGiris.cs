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
    public partial class AdminGiris : Form
    {
        public AdminGiris()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=22181617007");
        private void btnAdminGiris_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand("SELECT password FROM adminler WHERE username=@username", baglanti);
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

        private void AdminGiris_Load(object sender, EventArgs e)
        {

        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            GirisPaneli donus = new GirisPaneli();
            donus.Show();
            this.Hide();
        }
    }
}
