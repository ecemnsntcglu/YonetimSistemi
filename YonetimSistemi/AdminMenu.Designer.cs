namespace YonetimSistemi
{
    partial class AdminMenu
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_guncelle = new System.Windows.Forms.Button();
            this.btn_ekle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(36, 57);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(173, 27);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "ETKİNLİKLER";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(232, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 403);
            this.panel1.TabIndex = 3;
            // 
            // btn_guncelle
            // 
            this.btn_guncelle.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn_guncelle.Location = new System.Drawing.Point(36, 110);
            this.btn_guncelle.Name = "btn_guncelle";
            this.btn_guncelle.Size = new System.Drawing.Size(126, 58);
            this.btn_guncelle.TabIndex = 6;
            this.btn_guncelle.Text = "Etkinliği Güncelle";
            this.btn_guncelle.UseVisualStyleBackColor = true;
            this.btn_guncelle.Click += new System.EventHandler(this.btn_guncelle_Click);
            // 
            // btn_ekle
            // 
            this.btn_ekle.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn_ekle.Location = new System.Drawing.Point(36, 197);
            this.btn_ekle.Name = "btn_ekle";
            this.btn_ekle.Size = new System.Drawing.Size(126, 58);
            this.btn_ekle.TabIndex = 5;
            this.btn_ekle.Text = "Etkinlik Ekle";
            this.btn_ekle.UseVisualStyleBackColor = true;
            this.btn_ekle.Click += new System.EventHandler(this.btn_ekle_Click);
            // 
            // AdminMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_guncelle);
            this.Controls.Add(this.btn_ekle);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.comboBox1);
            this.Name = "AdminMenu";
            this.Text = "Etkinlik_guncel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_guncelle;
        private System.Windows.Forms.Button btn_ekle;
    }
}