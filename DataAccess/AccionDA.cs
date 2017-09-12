using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using Finisar.SQLite;
using System.Security.Permissions;

namespace DataAccess
{
    public class AccionDA
    {
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
        private SQLiteDataReader sqlite_datareader;
        private SQLiteCommand sqlite_cmd;

        public void createTable(string tableName)
        {
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "CREATE TABLE IF NOT EXISTS " + tableName + @"(
                                           ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                           FECHA                DATE,
                                           VALOR                DOUBLE,
                                           MONTO_NEGOCIADO      DOUBLE,
                                           NUM_ACCIONES         INT
                                        );";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }

        }


        public void insertarAcciones(List<AccionBE> objListAccion)
        {
            try
            {
                createTable(objListAccion[0].nemonico);

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    foreach (AccionBE objAccion in objListAccion)
                    {
                        sqlite_cmd.CommandText = "INSERT INTO "
                             + objAccion.nemonico
                             + " (FECHA, VALOR, MONTO_NEGOCIADO, NUM_ACCIONES) VALUES ('" + objAccion.fecha + "'," + (objAccion.valor==null ? "null" : objAccion.valor.ToString()) + "," + (objAccion.monto_negociado == null ? "null" : objAccion.monto_negociado.ToString()) + "," + (objAccion.num_acciones == null ? "null" : objAccion.num_acciones.ToString()) + ");";
                        sqlite_cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        public List<AccionBE> selectRows(string nemonico,string fechaIni, string fechaFin)
        {
            List<AccionBE> objListAccion = new List<AccionBE>();
            AccionBE objAccion;
            try
            {               
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM " + nemonico + " WHERE FECHA between '" + fechaIni + "' AND '" + fechaFin + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    decimal tmpvalue;
                    while (sqlite_datareader.Read())
                    {
                        objAccion = new AccionBE();
                        objAccion.nemonico = nemonico;
                        objAccion.fecha = sqlite_datareader["FECHA"].ToString();
                        objAccion.valor = decimal.TryParse(sqlite_datareader["VALOR"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.num_acciones = decimal.TryParse(sqlite_datareader["NUM_ACCIONES"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.monto_negociado = decimal.TryParse(sqlite_datareader["MONTO_NEGOCIADO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                        objListAccion.Add(objAccion);
                    }
                }
            }
            catch (Exception ex)
            {

            }
 
            return objListAccion;
        }


        public AccionBE UltimaFila(string nemonico)
        {
            AccionBE objAccion = new AccionBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM " + nemonico + " ORDER BY FECHA DESC LIMIT 1";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    decimal tmpvalue;
                    while (sqlite_datareader.Read())
                    {
                        objAccion.nemonico = nemonico;
                        objAccion.fecha = sqlite_datareader["FECHA"].ToString();
                        objAccion.valor = decimal.TryParse(sqlite_datareader["VALOR"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.num_acciones = decimal.TryParse(sqlite_datareader["NUM_ACCIONES"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.monto_negociado = decimal.TryParse(sqlite_datareader["MONTO_NEGOCIADO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objAccion;
        }

        public AccionBE AccionFecha(string nemonico,string fecha)
        {
            AccionBE objAccion = new AccionBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM " + nemonico + " WHERE FECHA = '" + fecha + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    decimal tmpvalue;
                    while (sqlite_datareader.Read())
                    {
                        objAccion.nemonico = nemonico;
                        objAccion.fecha = sqlite_datareader["FECHA"].ToString();
                        objAccion.valor = decimal.TryParse(sqlite_datareader["VALOR"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.num_acciones = decimal.TryParse(sqlite_datareader["NUM_ACCIONES"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.monto_negociado = decimal.TryParse(sqlite_datareader["MONTO_NEGOCIADO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objAccion;
        }

        public DateTime ultimaFecha(string nemonico)
        {
            DateTime result=new DateTime();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();

                    sqlite_cmd.CommandText = "SELECT FECHA FROM " + nemonico + " ORDER BY 1 DESC LIMIT 1";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        result = Convert.ToDateTime(sqlite_datareader["FECHA"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public decimal? promedioNemonico(string nemonico, string fechaIni, string fechaFin)
        {
            decimal? result = 0;
            List<AccionBE> objListAccion = selectRows(nemonico,fechaIni,fechaFin);

            result = objListAccion.Average(x=>x.valor);

            return result;
        }



        public List<double> datosGrafico(string nemonico)
        {
            List<double> result = new List<double>();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();

                    //sqlite_cmd.CommandText = "select valor from (select fecha,valor from " + nemonico + " where valor is not null order by fecha desc limit 200) order by fecha asc";
                    sqlite_cmd.CommandText = "select valor from (select fecha,valor from " + nemonico + " where valor is not null order by fecha desc) order by fecha asc";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        result.Add(Convert.ToDouble(sqlite_datareader["VALOR"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public List<double> datosGraficoTransaccion(string nemonico)
        {
            List<double> result = new List<double>();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();

                    sqlite_cmd.CommandText = "select monto_negociado from (select fecha,monto_negociado from " + nemonico + " where monto_negociado is not null order by fecha desc) order by fecha asc";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        result.Add(Convert.ToDouble(sqlite_datareader["monto_negociado"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public List<string> datosGraficoTransaccionFecha(string nemonico)
        {
            List<string> result = new List<string>();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();

                    sqlite_cmd.CommandText = "select fecha from (select fecha,monto_negociado from " + nemonico + " where monto_negociado is not null order by fecha desc) order by fecha asc";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        result.Add(Convert.ToDateTime(sqlite_datareader["fecha"].ToString()).ToShortDateString());
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }


        public AccionBE DatosAccionGrafico(string nemonico)
        {
            AccionBE objAccion = new AccionBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = @"select vl.VALOR, mx.VMAX,mn.VMIN from
                                                (select max(valor) as VMAX from " + nemonico + @") as mx ,

                                                (select min(valor) as VMIN from " + nemonico + @") as mn,

                                                (select valor from " + nemonico + @" order by fecha desc limit 1) as vl";


                    sqlite_datareader = sqlite_cmd.ExecuteReader();
                    decimal tmpvalue;
                    while (sqlite_datareader.Read())
                    {
                        objAccion.nemonico = nemonico;
                        objAccion.valor = decimal.TryParse(sqlite_datareader["VALOR"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.maximo = decimal.TryParse(sqlite_datareader["VMAX"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                        objAccion.minimo = decimal.TryParse(sqlite_datareader["VMIN"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objAccion;
        }

    }
}
