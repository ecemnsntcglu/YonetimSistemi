using System;
using Npgsql;
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
    public partial class GirisPaneli : Form
    {
        public GirisPaneli()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=22181617007");

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
