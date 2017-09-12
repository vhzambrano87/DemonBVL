using System;
using System.Collections.Generic;
using BusinessEntities;
using DataAccess;

namespace BusinessLogic
{
    public class AccionBL
    {
        AccionDA objAccionDA = new AccionDA();

        #region Crear Tabla
        public void createTable(string tableName)
        {
            objAccionDA = new AccionDA();
            objAccionDA.createTable(tableName);
        }
        #endregion

        #region Insertar Registros
        public void insertarAcciones(List<AccionBE> objListAccion)
        {
            objAccionDA = new AccionDA();
            objAccionDA.insertarAcciones(objListAccion);
        }

        #endregion

        #region Seleccionar Registros
        public List<AccionBE> selectRows(string nemonico, string fechaIni, string fechaFin)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.selectRows(nemonico,fechaIni,fechaFin);
        }

        public AccionBE UltimaFila(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.UltimaFila(nemonico);
        }

        public DateTime ultimaFecha(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.ultimaFecha(nemonico);
        }
        public AccionBE AccionFecha(string nemonico, string fecha)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.AccionFecha(nemonico,fecha);
        }
        public decimal? promedioNemonico(string nemonico, string fechaIni, string fechaFin)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.promedioNemonico(nemonico, fechaIni, fechaFin);
        }

        public List<double> datosGrafico(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.datosGrafico(nemonico);
        }

        public AccionBE DatosAccionGrafico(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.DatosAccionGrafico(nemonico);
        }

        public List<double> datosGraficoTransaccion(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.datosGraficoTransaccion(nemonico);
        }

        public List<string> datosGraficoTransaccionFecha(string nemonico)
        {
            objAccionDA = new AccionDA();
            return objAccionDA.datosGraficoTransaccionFecha(nemonico);
        }
        #endregion

    }
}
