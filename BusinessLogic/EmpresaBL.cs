using System;
using System.Collections.Generic;
using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{
    public class EmpresaBL
    {
        EmpresaDA objEmpresaDA = new EmpresaDA();
        public void InsertarEmpresa(EmpresaBE objEmpresa)
        {
            objEmpresaDA = new EmpresaDA();
            objEmpresaDA.InsertarEmpresa(objEmpresa);
        }

        public List<EmpresaBE> listarEmpresas()
        {
            objEmpresaDA = new EmpresaDA();
            return  objEmpresaDA.listarEmpresas();
        }

        public void EliminarEmpresa(string nemonico)
        {
            objEmpresaDA = new EmpresaDA();
            objEmpresaDA.EliminarEmpresa(nemonico);
        }

        public EmpresaBE obtenerEmpresa(string nemonico)
        {
            objEmpresaDA = new EmpresaDA();
            return objEmpresaDA.obtenerEmpresa(nemonico);
        }

        public void ModificarEmpresa(EmpresaBE objEmpresa)
        {
            objEmpresaDA = new EmpresaDA();
            objEmpresaDA.ModificarEmpresa(objEmpresa);
        }
    }
}
