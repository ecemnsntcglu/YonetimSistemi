using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Npgsql;

namespace YonetimSistemi
{
    public partial class BiletOnay : Form
    {
        private readonly List<int> _selectedKoltukIds;

        public BiletOnay(List<int> selectedKoltukIds)
        {
            InitializeComponent();
            _selectedKoltukIds = selectedKoltukIds;
        }

        private void btn_onayla_Click(object sender, EventArgs e)
        {
            if (_selectedKoltukIds == null || _selectedKoltukIds.Count == 0)
            {
                MessageBox.Show("Seçili bilet bulunamadı.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connString = "Host=localhost; Port=5432; Database=dbYonetim; User ID=postgres; Password=iK663746";

            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "UPDATE biletler SET durum = true WHERE bilet_id = ANY(@biletIds)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@biletIds", _selectedKoltukIds.ToArray());

                        int rowsAffected = cmd.ExecuteNonQuery();

                        MessageBox.Show($"{rowsAffected} bilet başarıyla onaylandı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
