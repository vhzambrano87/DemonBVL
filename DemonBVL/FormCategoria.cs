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
    public partial class FormCategoria : Form
    {
        FormPrincipal _objFormPrincipal;
        private CategoriaBL objCategoriaBL = new CategoriaBL();
        public FormCategoria(FormPrincipal objFormPrincipal)
        {
            _objFormPrincipal = objFormPrincipal;
            InitializeComponent();
        }

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            CategoriaBE objCategoria = new CategoriaBE();
            objCategoria.nombre = tbCategoria.Text;
            objCategoriaBL.insertarCategoria(objCategoria);
            _objFormPrincipal.recargarObjetos();
            this.Close();
        }
    }
}
