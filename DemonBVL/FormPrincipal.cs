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
            }
            catch(Exception ex)
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


                    HtmlAgilityPack.HtmlDocument PageResult = new HtmlWeb().Load("http://www.bvl.com.pe/jsp/cotizacion.jsp?fec_inicio=" + fechaInicio.ToString("yyyyMMdd") + "&fec_fin=" + fechaFin + "&nemonico=" + objEmpresa.nemonico);

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

                SeriesCollection objSerie = new SeriesCollection();
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
                    new Axis {
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

                SeriesCollection objSerie = new SeriesCollection();
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
                    new Axis
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
    }
}
