namespace SistemaBase
{
    partial class FrmCopia
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
            this.BtnCopia = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnCopia
            // 
            this.BtnCopia.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCopia.Location = new System.Drawing.Point(128, 100);
            this.BtnCopia.Name = "BtnCopia";
            this.BtnCopia.Size = new System.Drawing.Size(84, 36);
            this.BtnCopia.TabIndex = 121;
            this.BtnCopia.Text = "Copia";
            this.BtnCopia.UseVisualStyleBackColor = true;
            this.BtnCopia.Click += new System.EventHandler(this.BtnCopia_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(83, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 17);
            this.label1.TabIndex = 122;
            this.label1.Text = "Generar copia de seguridad";
            // 
            // FrmCopia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 242);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCopia);
            this.Name = "FrmCopia";
            this.Text = "Generación de la copia de seguridad";
            this.Load += new System.EventHandler(this.FrmCopia_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCopia;
        private System.Windows.Forms.Label label1;
    }
}