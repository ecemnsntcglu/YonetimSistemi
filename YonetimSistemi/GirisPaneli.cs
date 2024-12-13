using System;
using System.Windows.Forms;

namespace YonetimSistemi
{
    public partial class GirisPaneli : Form
    {
        public GirisPaneli()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            AdminGiris giris = new AdminGiris();
            giris.Show();
            this.Hide();
        }

        private void btnKullanici_Click(object sender, EventArgs e)
        {
            KullaniciGiris giris = new KullaniciGiris();
            giris.Show();
            this.Hide();

        }
    }
}
