namespace DemonBVL
{
    partial class FormPrincipal
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
            this.TabControl = new System.Windows.Forms.TabControl();
            this.PRINCIPAL = new System.Windows.Forms.TabPage();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.dtHasta = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dtDesde = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dgOperaciones = new System.Windows.Forms.DataGridView();
            this.dgPromedio = new System.Windows.Forms.DataGridView();
            this.COTIZACIONES = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.cbNemonico = new System.Windows.Forms.ComboBox();
            this.tbActual = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbMin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMax = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbProm = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CartesianChart = new LiveCharts.WinForms.CartesianChart();
            this.TRANSACCIONES = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.CartesianChart2 = new LiveCharts.WinForms.CartesianChart();
            this.NEMONICOS = new System.Windows.Forms.TabPage();
            this.lblEmpresas = new System.Windows.Forms.Label();
            this.linkCategoria = new System.Windows.Forms.LinkLabel();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.cbCategoria = new System.Windows.Forms.ComboBox();
            this.tbNombreEmp = new System.Windows.Forms.TextBox();
            this.tbNemonico = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dgNemonico = new System.Windows.Forms.DataGridView();
            this.btnDescargar = new System.Windows.Forms.Button();
            this.cbNemonico2 = new System.Windows.Forms.ComboBox();
            this.TabControl.SuspendLayout();
            this.PRINCIPAL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOperaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPromedio)).BeginInit();
            this.COTIZACIONES.SuspendLayout();
            this.TRANSACCIONES.SuspendLayout();
            this.NEMONICOS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNemonico)).BeginInit();
            this.SuspendLayout();
            // 
            // TabControl
            // 
            this.TabControl.Controls.Add(this.PRINCIPAL);
            this.TabControl.Controls.Add(this.COTIZACIONES);
            this.TabControl.Controls.Add(this.TRANSACCIONES);
            this.TabControl.Controls.Add(this.NEMONICOS);
            this.TabControl.Location = new System.Drawing.Point(39, 12);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(1000, 575);
            this.TabControl.TabIndex = 4;
            // 
            // PRINCIPAL
            // 
            this.PRINCIPAL.Controls.Add(this.prgBar);
            this.PRINCIPAL.Controls.Add(this.label10);
            this.PRINCIPAL.Controls.Add(this.dtpFecha);
            this.PRINCIPAL.Controls.Add(this.label9);
            this.PRINCIPAL.Controls.Add(this.dtHasta);
            this.PRINCIPAL.Controls.Add(this.label8);
            this.PRINCIPAL.Controls.Add(this.dtDesde);
            this.PRINCIPAL.Controls.Add(this.label6);
            this.PRINCIPAL.Controls.Add(this.label5);
            this.PRINCIPAL.Controls.Add(this.dgOperaciones);
            this.PRINCIPAL.Controls.Add(this.dgPromedio);
            this.PRINCIPAL.Location = new System.Drawing.Point(4, 22);
            this.PRINCIPAL.Name = "PRINCIPAL";
            this.PRINCIPAL.Padding = new System.Windows.Forms.Padding(3);
            this.PRINCIPAL.Size = new System.Drawing.Size(992, 549);
            this.PRINCIPAL.TabIndex = 19;
            this.PRINCIPAL.Text = "PRINCIPAL";
            this.PRINCIPAL.UseVisualStyleBackColor = true;
            // 
            // prgBar
            // 
            this.prgBar.Location = new System.Drawing.Point(381, 499);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(201, 23);
            this.prgBar.TabIndex = 10;
            this.prgBar.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(637, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(684, 96);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(98, 20);
            this.dtpFecha.TabIndex = 8;
            this.dtpFecha.ValueChanged += new System.EventHandler(this.dtpFecha_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(150, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Hasta:";
            // 
            // dtHasta
            // 
            this.dtHasta.CustomFormat = "dd-MM-yyyy";
            this.dtHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtHasta.Location = new System.Drawing.Point(197, 121);
            this.dtHasta.Name = "dtHasta";
            this.dtHasta.Size = new System.Drawing.Size(98, 20);
            this.dtHasta.TabIndex = 6;
            this.dtHasta.ValueChanged += new System.EventHandler(this.dtHasta_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(150, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Desde:";
            // 
            // dtDesde
            // 
            this.dtDesde.CustomFormat = "dd-MM-yyyy";
            this.dtDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDesde.Location = new System.Drawing.Point(197, 95);
            this.dtDesde.Name = "dtDesde";
            this.dtDesde.Size = new System.Drawing.Size(98, 20);
            this.dtDesde.TabIndex = 4;
            this.dtDesde.ValueChanged += new System.EventHandler(this.dtDesde_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(614, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(184, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Número de Operaciones";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(138, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 17);
            this.label5.TabIndex = 2;
            this.label5.Text = "Promedio Cotizaciones";
            // 
            // dgOperaciones
            // 
            this.dgOperaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgOperaciones.Location = new System.Drawing.Point(513, 148);
            this.dgOperaciones.Name = "dgOperaciones";
            this.dgOperaciones.Size = new System.Drawing.Size(430, 333);
            this.dgOperaciones.TabIndex = 1;
            // 
            // dgPromedio
            // 
            this.dgPromedio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPromedio.Location = new System.Drawing.Point(48, 148);
            this.dgPromedio.Name = "dgPromedio";
            this.dgPromedio.Size = new System.Drawing.Size(395, 333);
            this.dgPromedio.TabIndex = 0;
            // 
            // COTIZACIONES
            // 
            this.COTIZACIONES.Controls.Add(this.label7);
            this.COTIZACIONES.Controls.Add(this.cbNemonico);
            this.COTIZACIONES.Controls.Add(this.tbActual);
            this.COTIZACIONES.Controls.Add(this.label4);
            this.COTIZACIONES.Controls.Add(this.tbMin);
            this.COTIZACIONES.Controls.Add(this.label3);
            this.COTIZACIONES.Controls.Add(this.tbMax);
            this.COTIZACIONES.Controls.Add(this.label2);
            this.COTIZACIONES.Controls.Add(this.tbProm);
            this.COTIZACIONES.Controls.Add(this.label1);
            this.COTIZACIONES.Controls.Add(this.CartesianChart);
            this.COTIZACIONES.Location = new System.Drawing.Point(4, 22);
            this.COTIZACIONES.Name = "COTIZACIONES";
            this.COTIZACIONES.Padding = new System.Windows.Forms.Padding(3);
            this.COTIZACIONES.Size = new System.Drawing.Size(992, 549);
            this.COTIZACIONES.TabIndex = 0;
            this.COTIZACIONES.Text = "COTIZACIONES";
            this.COTIZACIONES.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(268, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Nemónico:";
            // 
            // cbNemonico
            // 
            this.cbNemonico.DisplayMember = "nombre";
            this.cbNemonico.FormattingEnabled = true;
            this.cbNemonico.Location = new System.Drawing.Point(367, 34);
            this.cbNemonico.Name = "cbNemonico";
            this.cbNemonico.Size = new System.Drawing.Size(209, 21);
            this.cbNemonico.TabIndex = 8;
            this.cbNemonico.ValueMember = "nemonico";
            this.cbNemonico.SelectedIndexChanged += new System.EventHandler(this.cbNemonico_SelectedIndexChanged);
            // 
            // tbActual
            // 
            this.tbActual.Location = new System.Drawing.Point(103, 498);
            this.tbActual.Name = "tbActual";
            this.tbActual.ReadOnly = true;
            this.tbActual.Size = new System.Drawing.Size(50, 20);
            this.tbActual.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(57, 501);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Actual:";
            // 
            // tbMin
            // 
            this.tbMin.Location = new System.Drawing.Point(550, 498);
            this.tbMin.Name = "tbMin";
            this.tbMin.ReadOnly = true;
            this.tbMin.Size = new System.Drawing.Size(50, 20);
            this.tbMin.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(499, 501);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Mínimo:";
            // 
            // tbMax
            // 
            this.tbMax.Location = new System.Drawing.Point(406, 498);
            this.tbMax.Name = "tbMax";
            this.tbMax.ReadOnly = true;
            this.tbMax.Size = new System.Drawing.Size(50, 20);
            this.tbMax.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(355, 501);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Máximo:";
            // 
            // tbProm
            // 
            this.tbProm.Location = new System.Drawing.Point(257, 498);
            this.tbProm.Name = "tbProm";
            this.tbProm.ReadOnly = true;
            this.tbProm.Size = new System.Drawing.Size(50, 20);
            this.tbProm.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(197, 501);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Promedio:";
            // 
            // CartesianChart
            // 
            this.CartesianChart.Location = new System.Drawing.Point(36, 68);
            this.CartesianChart.Name = "CartesianChart";
            this.CartesianChart.Size = new System.Drawing.Size(916, 407);
            this.CartesianChart.TabIndex = 1;
            this.CartesianChart.Text = "CartesianChart";
            // 
            // TRANSACCIONES
            // 
            this.TRANSACCIONES.Controls.Add(this.cbNemonico2);
            this.TRANSACCIONES.Controls.Add(this.label14);
            this.TRANSACCIONES.Controls.Add(this.CartesianChart2);
            this.TRANSACCIONES.Location = new System.Drawing.Point(4, 22);
            this.TRANSACCIONES.Name = "TRANSACCIONES";
            this.TRANSACCIONES.Padding = new System.Windows.Forms.Padding(3);
            this.TRANSACCIONES.Size = new System.Drawing.Size(992, 549);
            this.TRANSACCIONES.TabIndex = 21;
            this.TRANSACCIONES.Text = "TRANSACCIONES";
            this.TRANSACCIONES.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(268, 35);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "Nemónico:";
            // 
            // CartesianChart2
            // 
            this.CartesianChart2.Location = new System.Drawing.Point(36, 68);
            this.CartesianChart2.Name = "CartesianChart2";
            this.CartesianChart2.Size = new System.Drawing.Size(916, 407);
            this.CartesianChart2.TabIndex = 2;
            this.CartesianChart2.Text = "CartesianChart2";
            // 
            // NEMONICOS
            // 
            this.NEMONICOS.Controls.Add(this.lblEmpresas);
            this.NEMONICOS.Controls.Add(this.linkCategoria);
            this.NEMONICOS.Controls.Add(this.btnGuardar);
            this.NEMONICOS.Controls.Add(this.cbCategoria);
            this.NEMONICOS.Controls.Add(this.tbNombreEmp);
            this.NEMONICOS.Controls.Add(this.tbNemonico);
            this.NEMONICOS.Controls.Add(this.label13);
            this.NEMONICOS.Controls.Add(this.label12);
            this.NEMONICOS.Controls.Add(this.label11);
            this.NEMONICOS.Controls.Add(this.dgNemonico);
            this.NEMONICOS.Location = new System.Drawing.Point(4, 22);
            this.NEMONICOS.Name = "NEMONICOS";
            this.NEMONICOS.Padding = new System.Windows.Forms.Padding(3);
            this.NEMONICOS.Size = new System.Drawing.Size(992, 549);
            this.NEMONICOS.TabIndex = 20;
            this.NEMONICOS.Text = "NEMONICOS";
            this.NEMONICOS.UseVisualStyleBackColor = true;
            // 
            // lblEmpresas
            // 
            this.lblEmpresas.AutoSize = true;
            this.lblEmpresas.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmpresas.Location = new System.Drawing.Point(46, 484);
            this.lblEmpresas.Name = "lblEmpresas";
            this.lblEmpresas.Size = new System.Drawing.Size(0, 17);
            this.lblEmpresas.TabIndex = 9;
            // 
            // linkCategoria
            // 
            this.linkCategoria.AutoSize = true;
            this.linkCategoria.Location = new System.Drawing.Point(861, 67);
            this.linkCategoria.Name = "linkCategoria";
            this.linkCategoria.Size = new System.Drawing.Size(93, 13);
            this.linkCategoria.TabIndex = 8;
            this.linkCategoria.TabStop = true;
            this.linkCategoria.Text = "Agregar categoría";
            this.linkCategoria.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCategoria_LinkClicked);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(879, 219);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(75, 23);
            this.btnGuardar.TabIndex = 7;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // cbCategoria
            // 
            this.cbCategoria.DisplayMember = "nombre";
            this.cbCategoria.FormattingEnabled = true;
            this.cbCategoria.Location = new System.Drawing.Point(673, 59);
            this.cbCategoria.Name = "cbCategoria";
            this.cbCategoria.Size = new System.Drawing.Size(182, 21);
            this.cbCategoria.TabIndex = 6;
            this.cbCategoria.ValueMember = "id";
            // 
            // tbNombreEmp
            // 
            this.tbNombreEmp.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbNombreEmp.Location = new System.Drawing.Point(673, 164);
            this.tbNombreEmp.Name = "tbNombreEmp";
            this.tbNombreEmp.Size = new System.Drawing.Size(281, 20);
            this.tbNombreEmp.TabIndex = 5;
            // 
            // tbNemonico
            // 
            this.tbNemonico.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.tbNemonico.Location = new System.Drawing.Point(673, 111);
            this.tbNemonico.Name = "tbNemonico";
            this.tbNemonico.Size = new System.Drawing.Size(182, 20);
            this.tbNemonico.TabIndex = 4;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(602, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(47, 13);
            this.label13.TabIndex = 3;
            this.label13.Text = "Nombre:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(592, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Nemónico:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(592, 62);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Categoría:";
            // 
            // dgNemonico
            // 
            this.dgNemonico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgNemonico.Location = new System.Drawing.Point(43, 29);
            this.dgNemonico.Name = "dgNemonico";
            this.dgNemonico.Size = new System.Drawing.Size(527, 447);
            this.dgNemonico.TabIndex = 0;
            this.dgNemonico.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgNemonico_CellClick);
            // 
            // btnDescargar
            // 
            this.btnDescargar.Location = new System.Drawing.Point(28, 593);
            this.btnDescargar.Name = "btnDescargar";
            this.btnDescargar.Size = new System.Drawing.Size(75, 23);
            this.btnDescargar.TabIndex = 3;
            this.btnDescargar.Text = "Descargar!!";
            this.btnDescargar.UseVisualStyleBackColor = true;
            this.btnDescargar.Click += new System.EventHandler(this.btnDescargar_Click);
            // 
            // cbNemonico2
            // 
            this.cbNemonico2.FormattingEnabled = true;
            this.cbNemonico2.Location = new System.Drawing.Point(392, 35);
            this.cbNemonico2.Name = "cbNemonico2";
            this.cbNemonico2.Size = new System.Drawing.Size(121, 21);
            this.cbNemonico2.TabIndex = 12;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 630);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.btnDescargar);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BVL - HUZA";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TabControl.ResumeLayout(false);
            this.PRINCIPAL.ResumeLayout(false);
            this.PRINCIPAL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgOperaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPromedio)).EndInit();
            this.COTIZACIONES.ResumeLayout(false);
            this.COTIZACIONES.PerformLayout();
            this.TRANSACCIONES.ResumeLayout(false);
            this.TRANSACCIONES.PerformLayout();
            this.NEMONICOS.ResumeLayout(false);
            this.NEMONICOS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgNemonico)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage COTIZACIONES;
        private System.Windows.Forms.Button btnDescargar;
        private System.Windows.Forms.TextBox tbActual;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbMin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbProm;
        private System.Windows.Forms.Label label1;
        private LiveCharts.WinForms.CartesianChart CartesianChart;
        private System.Windows.Forms.TabPage PRINCIPAL;
        private System.Windows.Forms.DataGridView dgPromedio;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgOperaciones;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbNemonico;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtHasta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtDesde;
        private System.Windows.Forms.TabPage NEMONICOS;
        private System.Windows.Forms.LinkLabel linkCategoria;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.ComboBox cbCategoria;
        private System.Windows.Forms.TextBox tbNombreEmp;
        private System.Windows.Forms.TextBox tbNemonico;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dgNemonico;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.Label lblEmpresas;
        private System.Windows.Forms.TabPage TRANSACCIONES;
        private System.Windows.Forms.Label label14;
        private LiveCharts.WinForms.CartesianChart CartesianChart2;
        private System.Windows.Forms.ComboBox cbNemonico2;
    }
}

