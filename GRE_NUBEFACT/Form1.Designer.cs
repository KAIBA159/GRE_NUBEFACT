namespace GRE_NUBEFACT
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConsultarPDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtbox_serie = new System.Windows.Forms.TextBox();
            this.txtbox_correlativo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.descargarPDF = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConsultarPDF
            // 
            this.ConsultarPDF.Location = new System.Drawing.Point(317, 48);
            this.ConsultarPDF.Name = "ConsultarPDF";
            this.ConsultarPDF.Size = new System.Drawing.Size(179, 44);
            this.ConsultarPDF.TabIndex = 0;
            this.ConsultarPDF.Text = "Descargar_PDF";
            this.ConsultarPDF.UseVisualStyleBackColor = true;
            this.ConsultarPDF.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(96, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "SERIE";
            // 
            // txtbox_serie
            // 
            this.txtbox_serie.Location = new System.Drawing.Point(188, 47);
            this.txtbox_serie.Name = "txtbox_serie";
            this.txtbox_serie.Size = new System.Drawing.Size(100, 20);
            this.txtbox_serie.TabIndex = 2;
            // 
            // txtbox_correlativo
            // 
            this.txtbox_correlativo.Location = new System.Drawing.Point(188, 73);
            this.txtbox_correlativo.Name = "txtbox_correlativo";
            this.txtbox_correlativo.Size = new System.Drawing.Size(100, 20);
            this.txtbox_correlativo.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(96, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "CORRELATIVO";
            // 
            // descargarPDF
            // 
            this.descargarPDF.Location = new System.Drawing.Point(317, 98);
            this.descargarPDF.Name = "descargarPDF";
            this.descargarPDF.Size = new System.Drawing.Size(179, 44);
            this.descargarPDF.TabIndex = 5;
            this.descargarPDF.Text = "Descargar_CDR";
            this.descargarPDF.UseVisualStyleBackColor = true;
            this.descargarPDF.Click += new System.EventHandler(this.descargarCDR_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(317, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 44);
            this.button1.TabIndex = 6;
            this.button1.Text = "Descargar_XML";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.descargarXML_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.descargarPDF);
            this.Controls.Add(this.txtbox_correlativo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbox_serie);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConsultarPDF);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConsultarPDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtbox_serie;
        private System.Windows.Forms.TextBox txtbox_correlativo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button descargarPDF;
        private System.Windows.Forms.Button button1;
    }
}

