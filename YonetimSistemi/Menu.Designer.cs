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
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn1
            // 
            this.btn1.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn1.Location = new System.Drawing.Point(129, 88);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(126, 58);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "KONSER";
            this.btn1.UseVisualStyleBackColor = true;
            // 
            // btn2
            // 
            this.btn2.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn2.Location = new System.Drawing.Point(305, 88);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(126, 58);
            this.btn2.TabIndex = 1;
            this.btn2.Text = "FİLM";
            this.btn2.UseVisualStyleBackColor = true;
            // 
            // btn3
            // 
            this.btn3.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn3.Location = new System.Drawing.Point(484, 88);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(126, 58);
            this.btn3.TabIndex = 2;
            this.btn3.Text = "TİYATRO";
            this.btn3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(107, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Etkinliğe göre arama:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.Firebrick;
            this.label2.Location = new System.Drawing.Point(107, 188);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Şehre göre arama :";
            // 
            // btn6
            // 
            this.btn6.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn6.Location = new System.Drawing.Point(484, 222);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(126, 58);
            this.btn6.TabIndex = 7;
            this.btn6.Text = "İZMİR";
            this.btn6.UseVisualStyleBackColor = true;
            // 
            // btn5
            // 
            this.btn5.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn5.Location = new System.Drawing.Point(305, 222);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(126, 58);
            this.btn5.TabIndex = 6;
            this.btn5.Text = "İSTANBUL";
            this.btn5.UseVisualStyleBackColor = true;
            // 
            // btn4
            // 
            this.btn4.ForeColor = System.Drawing.Color.DarkGreen;
            this.btn4.Location = new System.Drawing.Point(129, 222);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(126, 58);
            this.btn4.TabIndex = 5;
            this.btn4.Text = "ANKARA";
            this.btn4.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PaleGreen;
            this.ClientSize = new System.Drawing.Size(777, 408);
            this.Controls.Add(this.btn6);
            this.Controls.Add(this.btn5);
            this.Controls.Add(this.btn4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn3);
            this.Controls.Add(this.btn2);
            this.Controls.Add(this.btn1);
            this.Name = "Menu";
            this.Text = "Menu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn4;
    }
}