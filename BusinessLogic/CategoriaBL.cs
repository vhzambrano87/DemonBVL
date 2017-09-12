using System;
using System.Collections.Generic;
using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{
    public class CategoriaBL
    {
        CategoriaDA objCategoriaDA = new CategoriaDA();
        public void insertarCategoria(CategoriaBE objCategoria)
        {
            objCategoriaDA = new CategoriaDA();
            objCategoriaDA.insertarCategoria(objCategoria);
        }

        public List<CategoriaBE> listarCategorias()
        {
            objCategoriaDA = new CategoriaDA();
            return objCategoriaDA.listarCategorias();
        }
    }
}
