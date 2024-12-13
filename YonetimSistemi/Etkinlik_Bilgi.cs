using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YonetimSistemi
{
    public partial class Etkinlik_Bilgi : Form
    {
        private string _etkinlikAdi;
        private string _etkinlik_tur;
        private string _tarih;
        private string _sehir;
        private string _resimYolu;
        private string _salonAdi;
        private decimal _enDusukFiyat;
    

        public Etkinlik_Bilgi(string etkinlikAdi,string etkinlik_tur, string tarih, string sehir, string resimYolu, string salonAdi, decimal enDusukFiyat)
        {
            _etkinlikAdi = etkinlikAdi.ToUpper();
            _etkinlik_tur = etkinlik_tur;
            _tarih = tarih;
            _sehir = sehir.ToUpper();
            _resimYolu = resimYolu;
            _salonAdi = salonAdi.ToUpper();
            _enDusukFiyat = enDusukFiyat;
            InitializeComponent();
            LoadBiletDetails();
        }

        private void LoadBiletDetails()
        {
            this.Text = $"Bilet - {_etkinlikAdi} - {_tarih}";

            // Mevcut label1'i kullanarak etkinlik bilgilerini gösterme
            label1.Text = $"Etkinlik: {_etkinlikAdi}\nTarih: {_tarih}\nŞehir: {_sehir}\nSalon: {_salonAdi}\nEn Düşük Fiyat: {_enDusukFiyat:C}";
            label1.Font = new Font(label1.Font.FontFamily, 14, FontStyle.Bold);

            // Mevcut pictureBox1'i kullanarak resmi gösterme
            if (File.Exists(_resimYolu))
            {
                pictureBox1.Image = Image.FromFile(_resimYolu);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bilet biletForm = new Bilet(_etkinlikAdi,_etkinlik_tur, _tarih, _sehir,  _salonAdi);
            biletForm.ShowDialog();
            
        }

       

        
    }
}
