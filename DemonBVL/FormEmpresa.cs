using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessEntities;
using BusinessLogic;

namespace DemonBVL
{
    
    public partial class FormEmpresa : Form
    {
        private EmpresaBE objEmpresa;
        private string nemonico;
        private EmpresaBL objEmpresaBL = new EmpresaBL();
        private FormPrincipal objFormPrincipal;
        private int empresaID;
        public FormEmpresa(string _nemonico, FormPrincipal _objFormPrincipal)
        {
            InitializeComponent();
            nemonico = _nemonico;
            objFormPrincipal = _objFormPrincipal;
            cargar();
        }

        public void cargar()
        {
            CategoriaBL objCategoriaBL = new CategoriaBL();
            cbCategoria.DataSource = objCategoriaBL.listarCategorias();            
        }

        private void FormEmpresa_Load(object sender, EventArgs e)
        {   
            objEmpresa = objEmpresaBL.obtenerEmpresa(nemonico);
            cbCategoria.SelectedValue = objEmpresa.categoria;
            tbNemonico.Text = objEmpresa.nemonico;
            tbNombreEmp.Text = objEmpresa.nombre;
            empresaID = objEmpresa.id;
            chkExcel.Checked = objEmpresa.excel == 1 ? true : false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            objEmpresa = new EmpresaBE();
            objEmpresa.categoria  = cbCategoria.Text.ToString();
            objEmpresa.nemonico = tbNemonico.Text;
            objEmpresa.nombre = tbNombreEmp.Text;
            objEmpresa.id = empresaID;
            objEmpresa.excel = chkExcel.Checked ? 1 : 0;
            objEmpresaBL.ModificarEmpresa(objEmpresa);
            objFormPrincipal.recargarObjetos();
            this.Close();
        }
    }
}
