namespace YonetimSistemi
{
    partial class AdminGiris
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtKullanici = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSifre = new System.Windows.Forms.MaskedTextBox();
            this.btnAdminGiris = new System.Windows.Forms.Button();
            this.btnGeriDon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.DarkGreen;
            this.label1.Location = new System.Drawing.Point(178, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kullanıcı adı:";
            // 
            // txtKullanici
            // 
            this.txtKullanici.ForeColor = System.Drawing.Color.DarkGreen;
            this.txtKullanici.Location = new System.Drawing.Point(181, 139);
            this.txtKullanici.Name = "txtKullanici";
            this.txtKullanici.Size = new System.Drawing.Size(141, 22);
            this.txtKullanici.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.DarkGreen;
            this.label2.Location = new System.Drawing.Point(178, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Şifre:";
            // 
            // txtSifre
            // 
            this.txtSifre.ForeColor = System.Drawing.Color.DarkGreen;
            this.txtSifre.Location = new System.Drawing.Point(181, 223);
            this.txtSifre.Name = "txtSifre";
            this.txtSifre.PasswordChar = '*';
            this.txtSifre.Size = new System.Drawing.Size(141, 22);
            this.txtSifre.TabIndex = 3;
            // 
            // btnAdminGiris
            // 
            this.btnAdminGiris.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnAdminGiris.Location = new System.Drawing.Point(196, 274);
            this.btnAdminGiris.Name = "btnAdminGiris";
            this.btnAdminGiris.Size = new System.Drawing.Size(104, 28);
            this.btnAdminGiris.TabIndex = 4;
            this.btnAdminGiris.Text = "Giriş";
            this.btnAdminGiris.UseVisualStyleBackColor = true;
            this.btnAdminGiris.Click += new System.EventHandler(this.btnAdminGiris_Click);
            // 
            // btnGeriDon
            // 
            this.btnGeriDon.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnGeriDon.Location = new System.Drawing.Point(196, 308);
            this.btnGeriDon.Name = "btnGeriDon";
            this.btnGeriDon.Size = new System.Drawing.Size(116, 50);
            this.btnGeriDon.TabIndex = 11;
            this.btnGeriDon.Text = "Giriş Paneline Dön";
            this.btnGeriDon.UseVisualStyleBackColor = true;
            this.btnGeriDon.Click += new System.EventHandler(this.btnGeriDon_Click);
            // 
            // AdminGiris
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(550, 450);
            this.Controls.Add(this.btnGeriDon);
            this.Controls.Add(this.btnAdminGiris);
            this.Controls.Add(this.txtSifre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKullanici);
            this.Controls.Add(this.label1);
            this.Name = "AdminGiris";
            this.Text = "Admin Girişi";
            this.Load += new System.EventHandler(this.AdminGiris_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtKullanici;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtSifre;
        private System.Windows.Forms.Button btnAdminGiris;
        private System.Windows.Forms.Button btnGeriDon;
    }
}