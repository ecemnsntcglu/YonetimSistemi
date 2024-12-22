using Npgsql;
using System;
using System.Windows.Forms;

namespace YonetimSistemi
{
    public partial class AdminGiris : Form
    {
        public AdminGiris()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=iK663746");
        private void btnAdminGiris_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                NpgsqlCommand komut1 = new NpgsqlCommand("SELECT admin_id ,password FROM adminler WHERE username=@username", baglanti);
                komut1.Parameters.AddWithValue("@username", txtKullanici.Text);
                NpgsqlDataReader oku = komut1.ExecuteReader();

                if (oku.Read())
                {
                    string storedPassword = oku["password"].ToString();
                    int admin = Convert.ToInt32(oku["admin_id"]);
                    if (storedPassword == txtSifre.Text)
                    {
                        AdminMenu menu = new AdminMenu();
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
        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            GirisPaneli donus = new GirisPaneli();
            donus.Show();
            this.Hide();
        }
    }
}
