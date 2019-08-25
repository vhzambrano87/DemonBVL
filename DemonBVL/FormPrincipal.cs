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
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
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
            Descargar1();
        }


        private void btnDescargar2_Click(object sender, EventArgs e)
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

                            if (row.SelectNodes("td[4]")[0].InnerText != "" && row.SelectNodes("td[4]")[0].InnerText != "&nbsp;")
                                objAccion.maximo = Convert.ToDecimal(row.SelectNodes("td[4]")[0].InnerText.Replace(",", ""));

                            if (row.SelectNodes("td[5]")[0].InnerText != "" && row.SelectNodes("td[5]")[0].InnerText != "&nbsp;")
                                objAccion.minimo = Convert.ToDecimal(row.SelectNodes("td[5]")[0].InnerText.Replace(",", ""));

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
                    ultimaFecha =objAccionBL.ultimaFecha(objEmpresa.nemonico); 
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

                        if (row.valAmtSol != "" )
                            objAccion.monto_negociado = Convert.ToDecimal(row.valAmtSol.Replace(",", ""));

                        if (row.valHighs != "" && row.valHighs != null)
                            objAccion.maximo = Convert.ToDecimal(row.valHighs.Replace(",", ""));

                        if (row.valLows != "" && row.valLows != null)
                            objAccion.minimo = Convert.ToDecimal(row.valLows.Replace(",", ""));

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
            objEmpresa.excel = chkExcel.Checked ? 1 : 0;
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

                dgNemonico.Columns.Insert(5, editarButtonColumn);

                DataGridViewButtonColumn eliminarButtonColumn = new DataGridViewButtonColumn();
                eliminarButtonColumn.Name = "eliminar_column";
                eliminarButtonColumn.Text = "Eliminar";
                eliminarButtonColumn.UseColumnTextForButtonValue = true;

                dgNemonico.Columns.Insert(6, eliminarButtonColumn);
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
            dgNemonico.Columns[4].HeaderText = "EXCEL";
            dgNemonico.Columns[5].HeaderText = "";
            dgNemonico.Columns[6].HeaderText = "";

            dgNemonico.Columns[0].Width = 30;
            dgNemonico.Columns[1].Width = 80;
            dgNemonico.Columns[2].Width = 140;
            dgNemonico.Columns[3].Width = 100;
            dgNemonico.Columns[4].Width = 50;
            dgNemonico.Columns[5].Width = 50;
            dgNemonico.Columns[6].Width = 50;
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
                
                tbProm.Text = objAccionBL.promedioNemonico(cbNemonico.SelectedValue.ToString(), ConfigurationManager.AppSettings["FechaInicio"], DateTime.Now.ToShortDateString()).ToString();

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
                EmpresaBL objEmpresaBL = new EmpresaBL();
            
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet2;
            object misValue = System.Reflection.Missing.Value;
            string path = ConfigurationManager.AppSettings["PathExcel"];
            FileInfo fi = new FileInfo(@path);
            if (!fi.Exists)
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkBook.Sheets.Add();
                xlWorkSheet2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(2);

                xlWorkSheet.Name = "COTIZACIONES";
                xlWorkSheet2.Name = "M. NEGOCIADO";
            }
            else
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@path);
                var ws1 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[1];
                var ws2 = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Sheets[2];



                if (ws1.Name == "COTIZACIONES")
                {
                    xlWorkSheet = ws1;
                    xlWorkSheet2 = ws2;
                }
                else
                {
                    xlWorkSheet = ws2;
                    xlWorkSheet2 = ws1;
                }
            }

                var objListEmpresas = objEmpresaBL.listarEmpresas();
                objListEmpresas = objListEmpresas.Where(x => x.excel == 1).ToList().OrderBy(x=>x.nemonico).ToList();

                xlWorkSheet.Cells[1, 1] = "";
                xlWorkSheet.Cells[2, 1] = "FECHA";

                int x1 = 2;
                int y1 = 1;
                int x2 = 2;
                int y2 = 2;                

                foreach (var item in objListEmpresas)
                {
                    xlWorkSheet.Cells[y1,x1] = item.nemonico;
                    xlWorkSheet.Cells[y2, x2] = "CIERRE";
                    x2++;
                    xlWorkSheet.Cells[y2, x2] = "MIN";
                    x2++;
                    xlWorkSheet.Cells[y2, x2] = "MAX";
                    x2++;
                    x1 = x1 + 3;
                } 

                //add data 

                AccionBL objAccion = new AccionBL();

                var objDatos = objAccion.GenerateDataDet(dtDesdeData.Value.ToString("yyyy-MM-dd"), dtHastaData.Value.ToString("yyyy-MM-dd"));

                int i = 3;
                int j = 1;                
                
                foreach(var item in objDatos)
                {
                    xlWorkSheet.Cells[i, j++] = item.FECHA;

                    foreach (var item2 in item.LIST_DATA_ACCION_DET)
                    {
                        xlWorkSheet.Cells[i, j++] = item2.CIERRE;
                        xlWorkSheet.Cells[i, j++] = item2.MIN;
                        xlWorkSheet.Cells[i, j++] = item2.MAX;                                                
                    }
                    i++;
                    j = 1;
                }

                int b = 1;
                for (int r = 0; r < objListEmpresas.Count; r++)
                {
                    string y = ExcelColumnIndexToName(b);
                    b = b + 2;
                    string x = ExcelColumnIndexToName(b);
                    xlWorkSheet.get_Range(y+"1", x+"1").Merge(true);
                    b++;
                }

                xlWorkSheet.get_Range("a1", "y2000").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWorkSheet.get_Range("a1", ExcelColumnIndexToName(objListEmpresas.Count * 3).ToString() + "2").Font.Bold = true;
                xlWorkSheet.get_Range("a1", ExcelColumnIndexToName(objListEmpresas.Count * 3).ToString() + "2").Interior.Color = Color.Yellow;
                
                xlWorkSheet2.Cells[1, 1] = "FECHA";

                j = 2;
                foreach (var item in objListEmpresas)
                {
                    xlWorkSheet2.Cells[1, j++] = item.nemonico;
                }
                
                i = 2;
                j = 1;
                foreach (var item in objDatos)
                {
                    xlWorkSheet2.Cells[i, j++] = item.FECHA;
                    foreach (var item2 in item.LIST_DATA_ACCION_DET)
                    {                       
                        xlWorkSheet2.Cells[i, j++] = item2.MONTO;                        
                    }
                    j = 1;
                    i++;
                }
                
                xlWorkSheet2.get_Range("a1", ExcelColumnIndexToName(objListEmpresas.Count + 1).ToString()+ "1").HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                xlWorkSheet2.get_Range("a1", ExcelColumnIndexToName(objListEmpresas.Count + 1).ToString()+ "1").Font.Bold = true;
                xlWorkSheet2.get_Range("a1", ExcelColumnIndexToName(objListEmpresas.Count + 1).ToString()+ "1").Interior.Color = Color.Yellow;


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

        private string ExcelColumnIndexToName(int Index)
        {
            string range = string.Empty;
            if (Index < 0) return range;
            int a = 26;
            int x = (int)Math.Floor(Math.Log((Index) * (a - 1) / a + 1, a));
            Index -= (int)(Math.Pow(a, x) - 1) * a / (a - 1);
            for (int i = x + 1; Index + i > 0; i--)
            {
                range = ((char)(65 + Index % a)).ToString() + range;
                Index /= a;
            }
            return range;
        }

    }
}
