namespace YonetimSistemi
{
    partial class Menu
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
            this.btnKonser = new System.Windows.Forms.Button();
            this.btnFilm = new System.Windows.Forms.Button();
            this.btnTiyatro = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnKonser
            // 
            this.btnKonser.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnKonser.Location = new System.Drawing.Point(236, 122);
            this.btnKonser.Name = "btnKonser";
            this.btnKonser.Size = new System.Drawing.Size(126, 58);
            this.btnKonser.TabIndex = 0;
            this.btnKonser.Text = "KONSER";
            this.btnKonser.UseVisualStyleBackColor = true;
            this.btnKonser.Click += new System.EventHandler(this.btnKonser_Click);
            // 
            // btnFilm
            // 
            this.btnFilm.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnFilm.Location = new System.Drawing.Point(380, 122);
            this.btnFilm.Name = "btnFilm";
            this.btnFilm.Size = new System.Drawing.Size(126, 58);
            this.btnFilm.TabIndex = 1;
            this.btnFilm.Text = "FİLM";
            this.btnFilm.UseVisualStyleBackColor = true;
            // 
            // btnTiyatro
            // 
            this.btnTiyatro.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnTiyatro.Location = new System.Drawing.Point(305, 186);
            this.btnTiyatro.Name = "btnTiyatro";
            this.btnTiyatro.Size = new System.Drawing.Size(126, 58);
            this.btnTiyatro.TabIndex = 2;
            this.btnTiyatro.Text = "TİYATRO";
            this.btnTiyatro.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(777, 408);
            this.Controls.Add(this.btnTiyatro);
            this.Controls.Add(this.btnFilm);
            this.Controls.Add(this.btnKonser);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnKonser;
        private System.Windows.Forms.Button btnFilm;
        private System.Windows.Forms.Button btnTiyatro;
    }
}