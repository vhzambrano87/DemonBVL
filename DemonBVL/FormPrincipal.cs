using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Xml.Linq;
using BusinessEntities;
using BusinessLogic;
using HtmlAgilityPack;
using ScrapySharp.Network;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using LiveCharts;
using LiveCharts.Defaults;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace DemonBVL
{
    public partial class FormPrincipal : Form
    {
        private List<string> objListNemonicos = new List<string>();
        private AccionBL objAccionBL = new AccionBL();
        private CategoriaBL objCategoriaBL = new CategoriaBL();
        private EmpresaBL objEmpresaBL = new EmpresaBL();
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("EN-US");
                dtDesde.Value =DateTime.ParseExact(ConfigurationManager.AppSettings["FechaInicio"], "yyyy-MM-dd", new CultureInfo("en-US"),DateTimeStyles.None);

                cargaInicial();
                List<DataBE> objListData = new List<DataBE>();

                dgData.DataSource = objListData;
                dgData.Refresh();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void cargaInicial()
        {
            recargarObjetos();
            CargarPromedios();
            cargarGraficos();
            cargarOperaciones();
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
           Descargar2();
        }

        public void Descargar1()
        {
            try
            {

                ScrapingBrowser Browser = new ScrapingBrowser();
                Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
                Browser.AllowMetaRedirect = true;



                DateTime ultimaFecha;
                DateTime fechaInicio = DateTime.ParseExact(ConfigurationManager.AppSettings["FechaInicio"], "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None);
                string fechaFin = DateTime.Now.ToString("yyyyMMdd");


                List<EmpresaBE> objLisEmpresa = objEmpresaBL.listarEmpresas();
                int contEmpresas = objLisEmpresa.Count();
                decimal partProgress = Math.Truncate(Convert.ToDecimal(100/contEmpresas));
                //LeerXML();
                prgBar.Visible = true;
                prgBar.UseWaitCursor=true;
                foreach (var objEmpresa in objLisEmpresa)
                {
                    prgBar.Increment(Convert.ToInt32(partProgress));
                    prgBar.Refresh();
                    ultimaFecha = objAccionBL.ultimaFecha(objEmpresa.nemonico);
                    if (DateTime.Now.Date > ultimaFecha.Date)
                    {
                        if (ultimaFecha.Date > Convert.ToDateTime(fechaInicio).Date)
                        {
                            fechaInicio = ultimaFecha;
                        }
                        
                        //"yyyyMMdd"
                    }
                    if (DateTime.Now.Date == ultimaFecha.Date)
                    {
                        continue;
                    }

                    string url = "http://www.bvl.com.pe/jsp/cotizacion.jsp?fec_inicio=" + fechaInicio.ToString("yyyyMMdd") + "&fec_fin=" + fechaFin + "&nemonico=" + objEmpresa.nemonico;
                    HtmlAgilityPack.HtmlDocument PageResult = new HtmlWeb().Load(url);
                                      

                    string xpath = "//tr";
                    var rows = PageResult.DocumentNode.SelectNodes(xpath);
                    List<AccionBE> objListAccion = new List<AccionBE>();
                    AccionBE objAccion;
                    foreach (var row in rows)
                    {
                        if (row.SelectNodes("td") != null)
                        {
                            objAccion = new AccionBE();

                            objAccion.nemonico = objEmpresa.nemonico;

                            objAccion.fecha = DateTime.ParseExact((row.SelectNodes("td[1]"))[0].InnerText, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyy-MM-dd");

                            if (row.SelectNodes("td[3]")[0].InnerText != "" && row.SelectNodes("td[3]")[0].InnerText != "&nbsp;")
                                objAccion.valor = Convert.ToDecimal(row.SelectNodes("td[3]")[0].InnerText.Replace(",", ""));

                            if (row.SelectNodes("td[7]")[0].InnerText != "" && row.SelectNodes("td[7]")[0].InnerText != "&nbsp;")
                                objAccion.num_acciones = Convert.ToDecimal(row.SelectNodes("td[7]")[0].InnerText.Replace(",", ""));

                            if (row.SelectNodes("td[8]")[0].InnerText != "" && row.SelectNodes("td[8]")[0].InnerText != "&nbsp;")
                                objAccion.monto_negociado = Convert.ToDecimal(row.SelectNodes("td[8]")[0].InnerText.Replace(",", ""));

                            objListAccion.Add(objAccion);
                        }
                    }

                    objAccionBL.insertarAcciones(objListAccion);

                }
                cargaInicial();
                prgBar.UseWaitCursor = false;
                prgBar.Visible = false;
                prgBar.Value = 0;
                
                MessageBox.Show("La descarga terminó con exito!!!.");
            }
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void Descargar2()
        {
            try
            {

                ScrapingBrowser Browser = new ScrapingBrowser();
                Browser.AllowAutoRedirect = true; // Browser has many settings you can access in setup
                Browser.AllowMetaRedirect = true;

                DateTime ultimaFecha;
                DateTime fechaInicio = DateTime.ParseExact(ConfigurationManager.AppSettings["FechaInicio"], "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None);
                string fechaFin = DateTime.Now.ToString("yyyyMMdd");


                List<EmpresaBE> objLisEmpresa = objEmpresaBL.listarEmpresas();
                int contEmpresas = objLisEmpresa.Count();
                decimal partProgress = Math.Truncate(Convert.ToDecimal(100 / contEmpresas));
                //LeerXML();
                prgBar.Visible = true;
                prgBar.UseWaitCursor = true;
                foreach (var objEmpresa in objLisEmpresa)
                {
                    prgBar.Increment(Convert.ToInt32(partProgress));
                    prgBar.Refresh();
                    ultimaFecha = objAccionBL.ultimaFecha(objEmpresa.nemonico);
                    if (DateTime.Now.Date > ultimaFecha.Date)
                    {
                        if (ultimaFecha.Date > Convert.ToDateTime(fechaInicio).Date)
                        {
                            fechaInicio = ultimaFecha;
                        }

                        //"yyyyMMdd"
                    }
                    if (DateTime.Now.Date == ultimaFecha.Date)
                    {
                        continue;
                    }

                    string servicio = "";

                    var objRootCotizacion = LeerData(objEmpresa.nemonico, fechaInicio.ToString("yyyy"), fechaInicio.ToString("MM"), DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

                    List<AccionBE> objListAccion = new List<AccionBE>();
                    AccionBE objAccion;
                    foreach (var row in objRootCotizacion.data)
                    {

                        objAccion = new AccionBE();

                        objAccion.nemonico = objEmpresa.nemonico;

                        objAccion.fecha = DateTime.ParseExact(row.fecDt, "dd/MM/yyyy", new CultureInfo("en-US")).ToString("yyyy-MM-dd");

                        if (row.valLasts != "" && row.valLasts != null)
                            objAccion.valor = Convert.ToDecimal(row.valLasts.Replace(",", ""));

                        if (row.valVol != "")
                            objAccion.num_acciones = Convert.ToDecimal(row.valVol.Replace(",", ""));

                        if (row.valAmtSol != "")
                            objAccion.monto_negociado = Convert.ToDecimal(row.valAmtSol.Replace(",", ""));

                        objListAccion.Add(objAccion);

                    }

                    objAccionBL.insertarAcciones(objListAccion);

                }
                cargaInicial();
                prgBar.UseWaitCursor = false;
                prgBar.Visible = false;
                prgBar.Value = 0;

                MessageBox.Show("La descarga terminó con exito!!!.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LeerXML()
        {
            StringBuilder result = new StringBuilder();
            foreach (XElement level1Element in XElement.Load("../../Nemonicos.xml").Elements("nemonico"))
            {
                objListNemonicos.Add(level1Element.Attribute("name").Value);

            }
        }

        private void linkCategoria_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormCategoria objFormCat = new FormCategoria(this);
            objFormCat.Show();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            EmpresaBE objEmpresa = new EmpresaBE();
            objEmpresa.categoria = cbCategoria.Text.ToString();
            objEmpresa.nombre = tbNombreEmp.Text;
            objEmpresa.nemonico = tbNemonico.Text;
            objEmpresaBL.InsertarEmpresa(objEmpresa);

            List<EmpresaBE> objListEmpresa = new List<EmpresaBE>();
            objListEmpresa = objEmpresaBL.listarEmpresas();
            var listEmpresa = objEmpresaBL.listarEmpresas().OrderBy(x => x.nemonico).ToList();
            dgNemonico.DataSource = listEmpresa;
            dgNemonico.Refresh();
            lblEmpresas.Text = "Número de Empresas: " + listEmpresa.Count.ToString();
            tbNemonico.Clear();
            tbNombreEmp.Clear();

        }

        public void recargarObjetos()
        {
            List<EmpresaBE> objListEmpresa = new List<EmpresaBE>();
            objListEmpresa = objEmpresaBL.listarEmpresas();
            var listEmpresa = objEmpresaBL.listarEmpresas().OrderBy(x => x.nemonico).ToList();
            dgNemonico.DataSource = listEmpresa;
            dgNemonico.Refresh();
            lblEmpresas.Text = "Número de Empresas: " + listEmpresa.Count.ToString();


            if (dgNemonico.Columns["editar_column"] == null && dgNemonico.Columns["eliminar_column"] == null)
            {
                DataGridViewButtonColumn editarButtonColumn = new DataGridViewButtonColumn();
                editarButtonColumn.Name = "editar_column";
                editarButtonColumn.Text = "Editar";
                editarButtonColumn.UseColumnTextForButtonValue = true;

                dgNemonico.Columns.Insert(4, editarButtonColumn);

                DataGridViewButtonColumn eliminarButtonColumn = new DataGridViewButtonColumn();
                eliminarButtonColumn.Name = "eliminar_column";
                eliminarButtonColumn.Text = "Eliminar";
                eliminarButtonColumn.UseColumnTextForButtonValue = true;

                dgNemonico.Columns.Insert(5, eliminarButtonColumn);
                cambiarHeader();
            }
            
            List<CategoriaBE> objListCategoria = new List<CategoriaBE>();
            objListCategoria = objCategoriaBL.listarCategorias();
            cbCategoria.DataSource = objListCategoria;
            cbCategoria.Refresh();
        
        }

        public void cambiarHeader()
        {
            dgNemonico.Columns[0].HeaderText = "ID";
            dgNemonico.Columns[1].HeaderText = "NEMONICO";
            dgNemonico.Columns[2].HeaderText = "NOMBRE";
            dgNemonico.Columns[3].HeaderText = "CATEGORIA";
            dgNemonico.Columns[4].HeaderText = "";
            dgNemonico.Columns[5].HeaderText = "";

            dgNemonico.Columns[0].Width = 30;
            dgNemonico.Columns[1].Width = 80;
            dgNemonico.Columns[2].Width = 140;
            dgNemonico.Columns[3].Width = 100;
            dgNemonico.Columns[4].Width = 50;
            dgNemonico.Columns[5].Width = 50;
        }

        private void dgNemonico_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgNemonico.Columns["eliminar_column"].Index)
            {

                string nemonico = dgNemonico.CurrentRow.Cells[3].Value.ToString();              
                objEmpresaBL.EliminarEmpresa(nemonico);

                List<EmpresaBE> objListEmpresa = new List<EmpresaBE>();
                objListEmpresa = objEmpresaBL.listarEmpresas().OrderBy(x => x.nemonico).ToList();
                lblEmpresas.Text = "Número de Empresas: " + objListEmpresa.Count.ToString();
                dgNemonico.DataSource = objEmpresaBL.listarEmpresas();
                dgNemonico.Refresh();
       
            }

            if (e.ColumnIndex == dgNemonico.Columns["editar_column"].Index)
            {

                string nemonico = dgNemonico.CurrentRow.Cells[3].Value.ToString();
                FormEmpresa objFormEmpresa = new FormEmpresa(nemonico,this);
                objFormEmpresa.Show();
            }
        }

        private void CargarPromedios()
        {
            List<EmpresaBE> objLisEmpresa = objEmpresaBL.listarEmpresas();
            AccionBE objUltima;
            List<PromedioBE> objListPromedio = new List<PromedioBE>();
            PromedioBE objPromedio;
            int decimales = 3;
            objAccionBL = new AccionBL();

            foreach (var objEmpresa in objLisEmpresa)
            {
                objUltima = new AccionBE();
                objUltima = objAccionBL.UltimaFila(objEmpresa.nemonico);
                objPromedio = new PromedioBE();
                objPromedio.promedio= Math.Round(Convert.ToDecimal(objAccionBL.promedioNemonico(objEmpresa.nemonico,dtDesde.Value.Date.ToString("yyyy-MM-dd"), dtHasta.Value.Date.ToString("yyyy-MM-dd"))), decimales);
                objPromedio.nemonico = objEmpresa.nemonico;
                objPromedio.promvsactual = Math.Round(Convert.ToDecimal((objPromedio.promedio == null || objUltima.valor==null || objPromedio.promedio==0) ? 0 : objUltima.valor/objPromedio.promedio), decimales);
                objPromedio.actual = objUltima.valor;
                objListPromedio.Add(objPromedio);
            }
            objListPromedio= objListPromedio.OrderBy(x => x.promvsactual).ToList();
            dgPromedio.DataSource = objListPromedio;

            dgPromedio.Columns[0].HeaderText = "NEMONICO";
            dgPromedio.Columns[1].HeaderText = "V.ACTUAL";
            dgPromedio.Columns[2].HeaderText = "V.PROMEDIO";
            dgPromedio.Columns[3].HeaderText = "ACT/PROM";

            dgPromedio.Columns[0].Width = 80;
            dgPromedio.Columns[1].Width = 70;
            dgPromedio.Columns[2].Width = 90;
            dgPromedio.Columns[3].Width = 90;
        }

        private void cargarOperaciones()
        {
            List<EmpresaBE> objListEmpresa = objEmpresaBL.listarEmpresas();
            List<AccionBE> objListAccion = new List<AccionBE>();
            AccionBE objAccion = new AccionBE();
            foreach (var objEmpresa in objListEmpresa)
            {
                objAccion = new AccionBE();
                objAccion = objAccionBL.AccionFecha(objEmpresa.nemonico,dtpFecha.Value.ToString("yyyy-MM-dd"));
                objListAccion.Add(objAccion);
            }

            dgOperaciones.DataSource = objListAccion.OrderByDescending(x=>x.monto_negociado).ToList();

            dgOperaciones.Columns[0].Visible = false;
            dgOperaciones.Columns[1].HeaderText = "NEMONICO";
            dgOperaciones.Columns[2].Visible = false;
            dgOperaciones.Columns[3].HeaderText = "V. ACTUAL";
            dgOperaciones.Columns[4].HeaderText = "MONTO NEG.";
            dgOperaciones.Columns[5].HeaderText = "ACCIONES";
            dgOperaciones.Columns[6].Visible = false;
            dgOperaciones.Columns[7].Visible = false;

            dgOperaciones.Columns[1].Width = 80;
            dgOperaciones.Columns[3].Width = 90;
            dgOperaciones.Columns[4].Width = 110;
            dgOperaciones.Columns[5].Width = 80;
        }

        private void cargarGraficos()
        {
            List<EmpresaBE> objListEmpresas = new List<EmpresaBE>();
            EmpresaBE objEmpresaVacia = new EmpresaBE();
            objEmpresaVacia.nombre = "------Seleccionar------";
            objEmpresaVacia.nemonico = "0";
            objListEmpresas.Add(objEmpresaVacia);
            objListEmpresas.AddRange(objEmpresaBL.listarEmpresas());           

            cbNemonico.DataSource = objListEmpresas;
            cbNemonico.SelectedValue = "0";

            cbNemonico2.DataSource = objListEmpresas;
            cbNemonico2.SelectedValue = "0";
        }

        private void cbNemonico_SelectedIndexChanged(object sender, EventArgs e)
        {
            CartesianChart.AxisX.Clear();
            if (cbNemonico.SelectedValue.ToString() != "0")
            {
                var data = objAccionBL.datosGrafico(cbNemonico.SelectedValue.ToString());

                LiveCharts.SeriesCollection objSerie = new LiveCharts.SeriesCollection();
                LineSeries objLine = new LineSeries();
                objLine.Values = new ChartValues<double>();
                foreach (var item in data)
                {
                    objLine.Values.Add(item);
                }
                objLine.PointGeometry = null;
                objLine.Title = "Cotización";
                objSerie.Add(objLine);

                CartesianChart.Series = objSerie;

                CartesianChart.AxisX.Add(
                    new LiveCharts.Wpf.Axis
                    {
                        Title = "Fecha",
                        Labels = objAccionBL.datosGraficoTransaccionFecha(cbNemonico.SelectedValue.ToString())
                    });
                
                AccionBE objAccion = new AccionBE();
                objAccion = objAccionBL.DatosAccionGrafico(cbNemonico.SelectedValue.ToString());
                tbActual.Text = objAccion.valor.ToString();
                tbMax.Text = objAccion.maximo.ToString();
                tbMin.Text = objAccion.minimo.ToString();
                
                tbProm.Text = objAccionBL.promedioNemonico(cbNemonico.SelectedValue.ToString(), "2016-01-01", DateTime.Now.ToShortDateString()).ToString();

            }

        }

        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            cargarOperaciones();
        }

        private void cbNemonico2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CartesianChart2.AxisX.Clear();

            if (cbNemonico2.SelectedValue.ToString() != "0")
            {
                var data = objAccionBL.datosGraficoTransaccion(cbNemonico2.SelectedValue.ToString());

                LiveCharts.SeriesCollection objSerie = new LiveCharts.SeriesCollection();
                LineSeries objLine = new LineSeries();
                objLine.Values = new ChartValues<double>();
                foreach (var item in data)
                {
                    objLine.Values.Add(item);
                }
                objLine.PointGeometry = null;
                objLine.Title = "Monto Negociado";
                objSerie.Add(objLine);

                CartesianChart2.Series = objSerie;

                CartesianChart2.AxisX.Add(
                    new LiveCharts.Wpf.Axis
                    {
                        Title = "Fecha",
                        Labels = objAccionBL.datosGraficoTransaccionFecha(cbNemonico2.SelectedValue.ToString())
                    });

            }
        }

        private void dtDesde_ValueChanged(object sender, EventArgs e)
        {            
            CargarPromedios();
            dgPromedio.Refresh();
        }

        private void dtHasta_ValueChanged(object sender, EventArgs e)
        {
            CargarPromedios();
            dgPromedio.Refresh();
        }

        private void btBuscarData_Click(object sender, EventArgs e)
        {
            var objDataBL = new AccionBL();

            dgData.DataSource = objDataBL.GenerateData(dtDesdeData.Value.ToString("yyyy-MM-dd"), dtHastaData.Value.ToString("yyyy-MM-dd"));
            dgData.Refresh();
        }

        private void btnEliminarData_Click(object sender, EventArgs e)
        {
            AccionBL objAccionBL = new AccionBL();
            objAccionBL.eliminarData();
        }

        private RootCotizacion LeerData(string nemonico, string anoIni, string mesIni, string anoFin, string mesFin)
        {
            RootCotizacion objRootCotizacion = new RootCotizacion();
            try
            {
                WebClient proxy = new WebClient();
                string serviceurl = "https://www.bvl.com.pe/web/guest/informacion-general-empresa?p_p_id=informaciongeneral_WAR_servicesbvlportlet&p_p_lifecycle=2&p_p_state=normal&p_p_mode=view&p_p_cacheability=cacheLevelPage&p_p_col_id=column-2&p_p_col_count=1&_informaciongeneral_WAR_servicesbvlportlet_cmd=getListaHistoricoCotizaciones&_informaciongeneral_WAR_servicesbvlportlet_tabindex=4&_informaciongeneral_WAR_servicesbvlportlet_anoini=" + anoIni + "&_informaciongeneral_WAR_servicesbvlportlet_mesini=" + mesIni + "&_informaciongeneral_WAR_servicesbvlportlet_anofin=" + anoFin + "&_informaciongeneral_WAR_servicesbvlportlet_mesfin=" + mesFin + "&_informaciongeneral_WAR_servicesbvlportlet_nemonicoselect=" + nemonico;
                byte[] data = proxy.DownloadData(serviceurl);
                Stream memory = new MemoryStream(data);
                var reader = new StreamReader(memory);
                var result = reader.ReadToEnd();

                objRootCotizacion = JsonConvert.DeserializeObject<RootCotizacion>(result);

            }
            catch (Exception ex)
            {

            }
            return objRootCotizacion;
        }

        private void btnLlenarReporte_Click(object sender, EventArgs e)
        {
            AccionBL objAccion = new AccionBL();
            objAccion.LlenarReporte();
            MessageBox.Show("El proceso terminó con exito!!!.");
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            //SaveFileDialog sfd = new SaveFileDialog();
            //sfd.Filter = "Excel Documents (*.xls)|*.xls";
            //sfd.FileName = "export.xls";
            //if (sfd.ShowDialog() == DialogResult.OK)
            //{
            //    ExportarExcel(dgData, sfd.FileName);
            //}
            ExportarExcelDet();
        }

        private void ExportarExcel(DataGridView dGV, string filename)
        {
            string stOutput = "";
            // Export titles:
            string sHeaders = "";

            for (int j = 0; j < dGV.Columns.Count; j++)
                sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
            stOutput += sHeaders + "\r\n";
            // Export data.
            for (int i = 0; i < dGV.RowCount - 1; i++)
            {
                string stLine = "";
                for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                    stLine = stLine.ToString() + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                stOutput += stLine + "\r\n";
            }
            Encoding utf16 = Encoding.GetEncoding(1254);
            byte[] output = utf16.GetBytes(stOutput);
            FileStream fs = new FileStream(filename, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(output, 0, output.Length); //write the encoded file
            bw.Flush();
            bw.Close();
            fs.Close();
        }

        private void ExportarExcelDet()
        {
            try
            {

            
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Range chartRange;
            string path = ConfigurationManager.AppSettings["PathExcel"];
            FileInfo fi = new FileInfo(@path);
            if (!fi.Exists)
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            }
            else
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@path);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
            }
                
                //add data 
                xlWorkSheet.Cells[1, 1] = "";
                xlWorkSheet.Cells[1, 2] = "AIHC1";
                xlWorkSheet.Cells[2, 1] = "FECHA";
                xlWorkSheet.Cells[2, 2] = "VALOR";
                xlWorkSheet.Cells[2, 3] = "M. NEG";
                xlWorkSheet.Cells[1, 4] = "ALICORC1";
                xlWorkSheet.Cells[2, 4] = "VALOR";
                xlWorkSheet.Cells[2, 5] = "M. NEG";
                xlWorkSheet.Cells[1, 6] = "BACKUBC1";
                xlWorkSheet.Cells[2, 6] = "VALOR";
                xlWorkSheet.Cells[2, 7] = "M. NEG";
                xlWorkSheet.Cells[1, 8] = "BROCALC1";
                xlWorkSheet.Cells[2, 8] = "VALOR";
                xlWorkSheet.Cells[2, 9] = "M. NEG";
                xlWorkSheet.Cells[1, 10] = "BROCALI1";
                xlWorkSheet.Cells[2, 10] = "VALOR";
                xlWorkSheet.Cells[2, 11] = "M. NEG";
                xlWorkSheet.Cells[1, 12] = "BUENAVC1";
                xlWorkSheet.Cells[2, 12] = "VALOR";
                xlWorkSheet.Cells[2, 13] = "M. NEG";
                xlWorkSheet.Cells[1, 14] = "CASAGRC1";
                xlWorkSheet.Cells[2, 14] = "VALOR";
                xlWorkSheet.Cells[2, 15] = "M. NEG";
                xlWorkSheet.Cells[1, 16] = "CONTINC1";
                xlWorkSheet.Cells[2, 16] = "VALOR";
                xlWorkSheet.Cells[2, 17] = "M. NEG";
                xlWorkSheet.Cells[1, 18] = "CORAREI1";
                xlWorkSheet.Cells[2, 18] = "VALOR";
                xlWorkSheet.Cells[2, 19] = "M. NEG";
                xlWorkSheet.Cells[1, 20] = "CPACASC1";
                xlWorkSheet.Cells[2, 20] = "VALOR";
                xlWorkSheet.Cells[2, 21] = "M. NEG";
                xlWorkSheet.Cells[1, 22] = "CREDITC1";
                xlWorkSheet.Cells[2, 22] = "VALOR";
                xlWorkSheet.Cells[2, 23] = "M. NEG";
                xlWorkSheet.Cells[1, 24] = "CVERDEC1";
                xlWorkSheet.Cells[2, 24] = "VALOR";
                xlWorkSheet.Cells[2, 25] = "M. NEG";
                xlWorkSheet.Cells[1, 26] = "DNT";
                xlWorkSheet.Cells[2, 26] = "VALOR";
                xlWorkSheet.Cells[2, 27] = "M. NEG";
                xlWorkSheet.Cells[1, 28] = "ETERNII1";
                xlWorkSheet.Cells[2, 28] = "VALOR";
                xlWorkSheet.Cells[2, 29] = "M. NEG";
                xlWorkSheet.Cells[1, 30] = "FERREYC1";
                xlWorkSheet.Cells[2, 30] = "VALOR";
                xlWorkSheet.Cells[2, 31] = "M. NEG";
                xlWorkSheet.Cells[1, 32] = "GRAMONC1";
                xlWorkSheet.Cells[2, 32] = "VALOR";
                xlWorkSheet.Cells[2, 33] = "M. NEG";
                xlWorkSheet.Cells[1, 34] = "IFS";
                xlWorkSheet.Cells[2, 34] = "VALOR";
                xlWorkSheet.Cells[2, 35] = "M. NEG";
                xlWorkSheet.Cells[1, 36] = "LAREDOC1";
                xlWorkSheet.Cells[2, 36] = "VALOR";
                xlWorkSheet.Cells[2, 37] = "M. NEG";
                xlWorkSheet.Cells[1, 38] = "LUSURC1";
                xlWorkSheet.Cells[2, 38] = "VALOR";
                xlWorkSheet.Cells[2, 39] = "M. NEG";
                xlWorkSheet.Cells[1, 40] = "MILPOC1";
                xlWorkSheet.Cells[2, 40] = "VALOR";
                xlWorkSheet.Cells[2, 41] = "M. NEG";
                xlWorkSheet.Cells[1, 42] = "MINSURI1";
                xlWorkSheet.Cells[2, 42] = "VALOR";
                xlWorkSheet.Cells[2, 43] = "M. NEG";
                xlWorkSheet.Cells[1, 44] = "MOROCOI1";
                xlWorkSheet.Cells[2, 44] = "VALOR";
                xlWorkSheet.Cells[2, 45] = "M. NEG";
                xlWorkSheet.Cells[1, 46] = "RELAPAC1";
                xlWorkSheet.Cells[2, 46] = "VALOR";
                xlWorkSheet.Cells[2, 47] = "M. NEG";
                xlWorkSheet.Cells[1, 48] = "SCOTIAC1";
                xlWorkSheet.Cells[2, 48] = "VALOR";
                xlWorkSheet.Cells[2, 49] = "M. NEG";
                xlWorkSheet.Cells[1, 50] = "SIDERC1";
                xlWorkSheet.Cells[2, 50] = "VALOR";
                xlWorkSheet.Cells[2, 51] = "M. NEG";
                xlWorkSheet.Cells[1, 52] = "TELEFBC1";
                xlWorkSheet.Cells[2, 52] = "VALOR";
                xlWorkSheet.Cells[2, 53] = "M. NEG";
                xlWorkSheet.Cells[1, 54] = "UNACEMC1";
                xlWorkSheet.Cells[2, 54] = "VALOR";
                xlWorkSheet.Cells[2, 55] = "M. NEG";
                xlWorkSheet.Cells[1, 56] = "VOLCABC1";
                xlWorkSheet.Cells[2, 56] = "VALOR";
                xlWorkSheet.Cells[2, 57] = "M. NEG";


                AccionBL objAccion = new AccionBL();

                var listaDatos = objAccion.GenerateDataDet(dtDesdeData.Value.ToString("yyyy-MM-dd"), dtHastaData.Value.ToString("yyyy-MM-dd"));

                int i = 3;
                foreach (var item in listaDatos)
                {
                    xlWorkSheet.Cells[i, 1] = item.FECHA;
                    xlWorkSheet.Cells[i, 2] = item.AIHC1_VAL;
                    xlWorkSheet.Cells[i, 3] = item.AIHC1_MONTO;
                    xlWorkSheet.Cells[i, 4] = item.ALICORC1_VAL;
                    xlWorkSheet.Cells[i, 5] = item.ALICORC1_MONTO;
                    xlWorkSheet.Cells[i, 6] = item.BACKUBC1_VAL;
                    xlWorkSheet.Cells[i, 7] = item.BROCALC1_MONTO;
                    xlWorkSheet.Cells[i, 8] = item.BROCALC1_VAL;
                    xlWorkSheet.Cells[i, 9] = item.BROCALC1_MONTO;
                    xlWorkSheet.Cells[i, 10] = item.BROCALI1_VAL;
                    xlWorkSheet.Cells[i, 11] = item.BROCALI1_MONTO;
                    xlWorkSheet.Cells[i, 12] = item.BUENAVC1_VAL;
                    xlWorkSheet.Cells[i, 13] = item.BUENAVC1_MONTO;
                    xlWorkSheet.Cells[i, 14] = item.CASAGRC1_VAL;
                    xlWorkSheet.Cells[i, 15] = item.CASAGRC1_MONTO;
                    xlWorkSheet.Cells[i, 16] = item.CONTINC1_VAL;
                    xlWorkSheet.Cells[i, 17] = item.CONTINC1_MONTO;
                    xlWorkSheet.Cells[i, 18] = item.CORAREI1_VAL;
                    xlWorkSheet.Cells[i, 19] = item.CORAREI1_MONTO;
                    xlWorkSheet.Cells[i, 20] = item.CPACASC1_VAL;
                    xlWorkSheet.Cells[i, 21] = item.CPACASC1_MONTO;
                    xlWorkSheet.Cells[i, 22] = item.CREDITC1_VAL;
                    xlWorkSheet.Cells[i, 23] = item.CREDITC1_MONTO;
                    xlWorkSheet.Cells[i, 24] = item.CVERDEC1_VAL;
                    xlWorkSheet.Cells[i, 25] = item.CVERDEC1_MONTO;
                    xlWorkSheet.Cells[i, 26] = item.DNT_VAL;
                    xlWorkSheet.Cells[i, 27] = item.DNT_MONTO;
                    xlWorkSheet.Cells[i, 28] = item.ETERNII1_VAL;
                    xlWorkSheet.Cells[i, 29] = item.ETERNII1_MONTO;
                    xlWorkSheet.Cells[i, 30] = item.FERREYC1_VAL;
                    xlWorkSheet.Cells[i, 31] = item.FERREYC1_MONTO;
                    xlWorkSheet.Cells[i, 32] = item.GRAMONC1_VAL;
                    xlWorkSheet.Cells[i, 33] = item.GRAMONC1_MONTO;
                    xlWorkSheet.Cells[i, 34] = item.IFS_VAL;
                    xlWorkSheet.Cells[i, 35] = item.IFS_MONTO;
                    xlWorkSheet.Cells[i, 36] = item.LAREDOC1_VAL;
                    xlWorkSheet.Cells[i, 37] = item.LAREDOC1_MONTO;
                    xlWorkSheet.Cells[i, 38] = item.LUSURC1_VAL;
                    xlWorkSheet.Cells[i, 39] = item.LUSURC1_MONTO;
                    xlWorkSheet.Cells[i, 40] = item.MILPOC1_VAL;
                    xlWorkSheet.Cells[i, 41] = item.MILPOC1_MONTO;
                    xlWorkSheet.Cells[i, 42] = item.MINSURI1_VAL;
                    xlWorkSheet.Cells[i, 43] = item.MINSURI1_MONTO;
                    xlWorkSheet.Cells[i, 44] = item.MOROCOI1_VAL;
                    xlWorkSheet.Cells[i, 45] = item.MOROCOI1_MONTO;
                    xlWorkSheet.Cells[i, 46] = item.RELAPAC1_VAL;
                    xlWorkSheet.Cells[i, 47] = item.RELAPAC1_MONTO;
                    xlWorkSheet.Cells[i, 48] = item.SCOTIAC1_VAL;
                    xlWorkSheet.Cells[i, 49] = item.SCOTIAC1_MONTO;
                    xlWorkSheet.Cells[i, 50] = item.SIDERC1_VAL;
                    xlWorkSheet.Cells[i, 51] = item.SIDERC1_MONTO;
                    xlWorkSheet.Cells[i, 52] = item.TELEFBC1_VAL;
                    xlWorkSheet.Cells[i, 53] = item.TELEFBC1_MONTO;
                    xlWorkSheet.Cells[i, 54] = item.UNACEMC1_VAL;
                    xlWorkSheet.Cells[i, 55] = item.VOLCABC1_MONTO;
                    xlWorkSheet.Cells[i, 56] = item.VOLCABC1_VAL;
                    xlWorkSheet.Cells[i, 57] = item.VOLCABC1_MONTO;
                    i++;

                }

                xlWorkSheet.get_Range("b1", "c1").Merge(true);
                xlWorkSheet.get_Range("d1", "e1").Merge(true);
                xlWorkSheet.get_Range("f1", "g1").Merge(true);
                xlWorkSheet.get_Range("h1", "i1").Merge(true);
                xlWorkSheet.get_Range("j1", "k1").Merge(true);
                xlWorkSheet.get_Range("l1", "m1").Merge(true);
                xlWorkSheet.get_Range("n1", "o1").Merge(true);
                xlWorkSheet.get_Range("p1", "q1").Merge(true);
                xlWorkSheet.get_Range("r1", "s1").Merge(true);
                xlWorkSheet.get_Range("t1", "u1").Merge(true);
                xlWorkSheet.get_Range("v1", "w1").Merge(true);
                xlWorkSheet.get_Range("x1", "y1").Merge(true);
                xlWorkSheet.get_Range("z1", "aa1").Merge(true);
                xlWorkSheet.get_Range("ab1", "ac1").Merge(true);
                xlWorkSheet.get_Range("ad1", "ae1").Merge(true);
                xlWorkSheet.get_Range("af1", "ag1").Merge(true);
                xlWorkSheet.get_Range("ah1", "ai1").Merge(true);
                xlWorkSheet.get_Range("aj1", "ak1").Merge(true);
                xlWorkSheet.get_Range("al1", "am1").Merge(true);
                xlWorkSheet.get_Range("an1", "ao1").Merge(true);
                xlWorkSheet.get_Range("ap1", "aq1").Merge(true);
                xlWorkSheet.get_Range("ar1", "as1").Merge(true);
                xlWorkSheet.get_Range("at1", "au1").Merge(true);
                xlWorkSheet.get_Range("av1", "aw1").Merge(true);
                xlWorkSheet.get_Range("ax1", "ay1").Merge(true);
                xlWorkSheet.get_Range("az1", "ba1").Merge(true);
                xlWorkSheet.get_Range("bb1", "bc1").Merge(true);
                xlWorkSheet.get_Range("bd1", "be1").Merge(true);

                xlWorkSheet.get_Range("a1", "be2000").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            
            if (!fi.Exists)
            {
                xlWorkBook.SaveAs(@path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            else {
                xlWorkBook.Save();

            }
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();
            MessageBox.Show("Se exportó con éxito!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        }
}
