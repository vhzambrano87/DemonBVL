namespace DemonBVL
{
    partial class FormEmpresa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmpresa));
            this.btnGuardar = new System.Windows.Forms.Button();
            this.cbCategoria = new System.Windows.Forms.ComboBox();
            this.tbNombreEmp = new System.Windows.Forms.TextBox();
            this.tbNemonico = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chkExcel = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(304, 203);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 15;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // cbCategoria
            // 
            this.cbCategoria.DisplayMember = "nombre";
            this.cbCategoria.FormattingEnabled = true;
            this.cbCategoria.Location = new System.Drawing.Point(98, 12);
            this.cbCategoria.Name = "cbCategoria";
            this.cbCategoria.Size = new System.Drawing.Size(182, 21);
            this.cbCategoria.TabIndex = 14;
            this.cbCategoria.ValueMember = "nombre";
            // 
            // tbNombreEmp
            // 
            this.tbNombreEmp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbNombreEmp.Location = new System.Drawing.Point(98, 117);
            this.tbNombreEmp.Name = "tbNombreEmp";
            this.tbNombreEmp.Size = new System.Drawing.Size(281, 20);
            this.tbNombreEmp.TabIndex = 13;
            // 
            // tbNemonico
            // 
            this.tbNemonico.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbNemonico.Location = new System.Drawing.Point(98, 64);
            this.tbNemonico.Name = "tbNemonico";
            this.tbNemonico.Size = new System.Drawing.Size(182, 20);
            this.tbNemonico.TabIndex = 12;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(27, 120);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 11;
            this.label13.Text = "Nombre:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(17, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Nemónico:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Categoría:";
            // 
            // chkExcel
            // 
            this.chkExcel.AutoSize = true;
            this.chkExcel.Location = new System.Drawing.Point(98, 170);
            this.chkExcel.Name = "chkExcel";
            this.chkExcel.Size = new System.Drawing.Size(15, 14);
            this.chkExcel.TabIndex = 16;
            this.chkExcel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 170);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Excel:";
            // 
            // FormEmpresa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 249);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkExcel);
            this.Controls.Add(this.btnGuardar);
            this.Controls.Add(this.cbCategoria);
            this.Controls.Add(this.tbNombreEmp);
            this.Controls.Add(this.tbNemonico);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormEmpresa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Empresa";
            this.Load += new System.EventHandler(this.FormEmpresa_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.ComboBox cbCategoria;
        private System.Windows.Forms.TextBox tbNombreEmp;
        private System.Windows.Forms.TextBox tbNemonico;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkExcel;
        private System.Windows.Forms.Label label1;
    }
}