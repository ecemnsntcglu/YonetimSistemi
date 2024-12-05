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
    public partial class Konser : Form
    {
        public Konser()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432; Database=dbYonetim; user ID=postgres; password=22181617007");
        private void btnListele_Click(object sender, EventArgs e)
        {
            try
            {
                string sorgu = "SELECT * FROM konserler";
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                // Verileri DataGridView'e yükle
                dataGridView1.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Hiç veri bulunamadı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yükleme hatası: " + ex.Message);
            }
        }
    }
}
