namespace YonetimSistemi
{
    partial class EtkinlikEkle
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.domainUpDown_tür = new System.Windows.Forms.DomainUpDown();
            this.textBox_etkinlikAdi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.l = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ekle = new System.Windows.Forms.Button();
            this.domainUpDown_salon = new System.Windows.Forms.DomainUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // domainUpDown_tür
            // 
            this.domainUpDown_tür.Location = new System.Drawing.Point(143, 109);
            this.domainUpDown_tür.Name = "domainUpDown_tür";
            this.domainUpDown_tür.Size = new System.Drawing.Size(120, 22);
            this.domainUpDown_tür.TabIndex = 0;
            this.domainUpDown_tür.Text = "domainUpDown1";
            // 
            // textBox_etkinlikAdi
            // 
            this.textBox_etkinlikAdi.Location = new System.Drawing.Point(143, 29);
            this.textBox_etkinlikAdi.Name = "textBox_etkinlikAdi";
            this.textBox_etkinlikAdi.Size = new System.Drawing.Size(120, 22);
            this.textBox_etkinlikAdi.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Etkinlik Adı";
            // 
            // l
            // 
            this.l.AutoSize = true;
            this.l.Location = new System.Drawing.Point(24, 115);
            this.l.Name = "l";
            this.l.Size = new System.Drawing.Size(79, 16);
            this.l.TabIndex = 3;
            this.l.Text = "Etkinlik Türü";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(361, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tarih";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(364, 60);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Ivory;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.domainUpDown_salon);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTimePicker);
            this.panel1.Controls.Add(this.l);
            this.panel1.Controls.Add(this.domainUpDown_tür);
            this.panel1.Controls.Add(this.textBox_etkinlikAdi);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(46, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(678, 277);
            this.panel1.TabIndex = 6;
            // 
            // button_ekle
            // 
            this.button_ekle.Location = new System.Drawing.Point(637, 359);
            this.button_ekle.Name = "button_ekle";
            this.button_ekle.Size = new System.Drawing.Size(107, 29);
            this.button_ekle.TabIndex = 7;
            this.button_ekle.Text = "EKLE";
            this.button_ekle.UseVisualStyleBackColor = true;
            this.button_ekle.Click += new System.EventHandler(this.button_ekle_Click);
            // 
            // domainUpDown_salon
            // 
            this.domainUpDown_salon.Location = new System.Drawing.Point(143, 177);
            this.domainUpDown_salon.Name = "domainUpDown_salon";
            this.domainUpDown_salon.Size = new System.Drawing.Size(120, 22);
            this.domainUpDown_salon.TabIndex = 6;
            this.domainUpDown_salon.Text = "domainUpDown_salon";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Etkinlik Salon";
            // 
            // EtkinlikEkle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_ekle);
            this.Controls.Add(this.panel1);
            this.Name = "EtkinlikEkle";
            this.Text = "EtkinlikEkle";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DomainUpDown domainUpDown_tür;
        private System.Windows.Forms.TextBox textBox_etkinlikAdi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_ekle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DomainUpDown domainUpDown_salon;
    }
}