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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
            InitializeButtons();

        }

        private void InitializeButtons()
        {
            Button[] buttons = { btn6, btn5, btn4, btn3, btn2, btn1 };
            foreach (Button button in buttons)
            {
                button.Click += new EventHandler(Button_Click);
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string filterCriteria = clickedButton.Text.ToLower();
            Etkinlikler etkinlikler = new Etkinlikler(filterCriteria);
            etkinlikler.Show();




        }
    }
}
