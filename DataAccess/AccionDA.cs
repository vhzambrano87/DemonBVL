using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.SQLite;
using System.Security.Permissions;
using System.Configuration;

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
                                           NUM_ACCIONES         INT,
                                           VALOR_MAX            DOUBLE,
                                           VALOR_MIN            DOUBLE
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
                             + " (FECHA, VALOR, MONTO_NEGOCIADO, NUM_ACCIONES, VALOR_MAX, VALOR_MIN) VALUES ('" + objAccion.fecha + "'," + (objAccion.valor==null ? "null" : objAccion.valor.ToString()) + "," + (objAccion.monto_negociado == null ? "null" : objAccion.monto_negociado.ToString()) + "," + (objAccion.num_acciones == null ? "null" : objAccion.num_acciones.ToString()) + "," + (objAccion.maximo == null ? "null" : objAccion.maximo.ToString()) + "," + (objAccion.minimo == null ? "null" : objAccion.minimo.ToString()) + ")";
                        sqlite_cmd.ExecuteNonQuery();

                    }
                }
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    foreach (AccionBE objAccion in objListAccion)
                    {
                        if (objAccion.valor != null)
                        {
                            sqlite_cmd.CommandText = @"UPDATE DATA"
                                 + " SET " + objAccion.nemonico + "_VAL = " + objAccion.valor
                                 + ", " + objAccion.nemonico + "_MONTO = " + objAccion.monto_negociado
                                 + ", " + objAccion.nemonico + "_VAL_MAX = " + (objAccion.maximo==null?"NULL" : objAccion.maximo.ToString())
                                 + ", " + objAccion.nemonico + "_VAL_MIN = " + (objAccion.minimo==null?"NULL": objAccion.minimo.ToString())
                                 + " WHERE FECHA = '" + objAccion.fecha +"'";
                            sqlite_cmd.ExecuteNonQuery();
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<AccionBE> ListarAccion(string nemonico)
        {
            List<AccionBE> result = new List<AccionBE>();
            AccionBE objAccion = new AccionBE();
            decimal tmpvalue;
            using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
            {
                dbConn.Open();
                sqlite_cmd = dbConn.CreateCommand();
                sqlite_cmd.CommandText = @"SELECT * FROM " + nemonico + " ORDER BY FECHA ASC";
                sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    objAccion = new AccionBE();
                    objAccion.nemonico = nemonico;
                    objAccion.fecha = Convert.ToDateTime(sqlite_datareader["fecha"].ToString()).ToString("yyyy-MM-dd");
                    objAccion.valor = decimal.TryParse(sqlite_datareader["valor"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                    objAccion.monto_negociado = decimal.TryParse(sqlite_datareader["monto_negociado"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                    objAccion.maximo = decimal.TryParse(sqlite_datareader["valor_max"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                    objAccion.minimo = decimal.TryParse(sqlite_datareader["valor_min"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                    result.Add(objAccion);
                }                
            }
            return result;
        }

        public void LlenarReporte()
        {
            AccionDA objAccionDA = new AccionDA();
            EmpresaDA objEmpresaDA = new EmpresaDA();
            var listEmpresa = objEmpresaDA.listarEmpresas();
            
           
            foreach (var item in listEmpresa)
            {
                var listAccion = objAccionDA.ListarAccion(item.nemonico);
                foreach (var itemAccion in listAccion)
                {
                    if(itemAccion.valor!=null)
                    {
                        using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                        {
                            dbConn.Open();
                            sqlite_cmd = dbConn.CreateCommand();

                            string query = "UPDATE DATA SET " + itemAccion.nemonico + "_VAL = " + itemAccion.valor.ToString()
                            + ", " + itemAccion.nemonico + "_MONTO = " + (itemAccion.monto_negociado==null ? "NULL" : itemAccion.monto_negociado.ToString())
                            + ", " + itemAccion.nemonico + "_VAL_MAX = " + (itemAccion.maximo == null ? "NULL" : itemAccion.maximo.ToString())
                            + ", " + itemAccion.nemonico + "_VAL_MIN = " + (itemAccion.minimo == null ? "NULL" : itemAccion.minimo.ToString())
                            + " WHERE FECHA = '" + itemAccion.fecha + "'";

                            sqlite_cmd.CommandText = query;
                            sqlite_cmd.ExecuteNonQuery();
                        }
                    }
                }                    

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

                    sqlite_cmd.CommandText = $"select valor from (select fecha,valor from {nemonico} where valor is not null and fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}' order by fecha desc) order by fecha asc";
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

                    sqlite_cmd.CommandText = $"select monto_negociado from (select fecha,monto_negociado from {nemonico} where monto_negociado is not null and where fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}' order by fecha desc) order by fecha asc";
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

                    //sqlite_cmd.CommandText = $"select fecha from (select fecha,monto_negociado from {nemonico} where monto_negociado is not null and fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}' order by fecha desc) order by fecha asc";
                    sqlite_cmd.CommandText = $"select fecha from (select fecha,valor from {nemonico} where valor is not null and fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}' order by fecha desc) order by fecha asc";
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
                    sqlite_cmd.CommandText = $@"select vl.VALOR, mx.VMAX,mn.VMIN from (select max(valor) as VMAX from {nemonico} where fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}' ) as mx ,
                                                (select min(valor) as VMIN from {nemonico} where fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}') as mn,
                                                (select valor from {nemonico} where fecha > '{ConfigurationManager.AppSettings["FechaInicio"]}'  order by fecha desc limit 1) as vl";


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




        public List<DataBE> GenerateData(string desde, string hasta)
        {
            List<DataBE> objLisData = new List<DataBE>();
            DataBE objData = new DataBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = @"SELECT DISTINCT FECHA, AIHC1_VAL,ALICORC1_VAL ,BACKUBC1_VAL ,BROCALC1_VAL ,BROCALI1_VAL ,BUENAVC1_VAL ,CASAGRC1_VAL ,CONTINC1_VAL ,CORAREI1_VAL ,CPACASC1_VAL ,CREDITC1_VAL ,
                                                CVERDEC1_VAL ,DNT_VAL ,ETERNII1_VAL ,FERREYC1_VAL ,GRAMONC1_VAL ,IFS_VAL ,LAREDOC1_VAL ,LUSURC1_VAL ,MILPOC1_VAL ,MINSURI1_VAL ,MOROCOI1_VAL ,RELAPAC1_VAL ,
                                                SCOTIAC1_VAL ,SIDERC1_VAL ,TELEFBC1_VAL ,UNACEMC1_VAL ,VOLCABC1_VAL 
                                                FROM DATA 
                                                WHERE  cast (strftime('%w', FECHA) as integer) not in (0,6) and fecha between '" + desde + "' and '"+ hasta +"'";

                    using (sqlite_datareader = sqlite_cmd.ExecuteReader())
                    {

                        decimal tmpvalue;
                        while (sqlite_datareader.Read())
                        {
                            objData = new DataBE();
                            try
                            {
                                objData.FECHA = sqlite_datareader["FECHA"].ToString();

                                objData.AIHC1 = decimal.TryParse(sqlite_datareader["AIHC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.ALICORC1 = decimal.TryParse(sqlite_datareader["ALICORC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.BACKUBC1 = decimal.TryParse(sqlite_datareader["BACKUBC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.BROCALC1 = decimal.TryParse(sqlite_datareader["BROCALC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.BROCALI1 = decimal.TryParse(sqlite_datareader["BROCALI1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.BUENAVC1 = decimal.TryParse(sqlite_datareader["BUENAVC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CASAGRC1 = decimal.TryParse(sqlite_datareader["CASAGRC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CONTINC1 = decimal.TryParse(sqlite_datareader["CONTINC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CORAREI1 = decimal.TryParse(sqlite_datareader["CORAREI1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CPACASC1 = decimal.TryParse(sqlite_datareader["CPACASC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CREDITC1 = decimal.TryParse(sqlite_datareader["CREDITC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.CVERDEC1 = decimal.TryParse(sqlite_datareader["CVERDEC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.DNT = decimal.TryParse(sqlite_datareader["DNT_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.ETERNII1 = decimal.TryParse(sqlite_datareader["ETERNII1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.FERREYC1 = decimal.TryParse(sqlite_datareader["FERREYC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.GRAMONC1 = decimal.TryParse(sqlite_datareader["GRAMONC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.IFS = decimal.TryParse(sqlite_datareader["IFS_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.LAREDOC1 = decimal.TryParse(sqlite_datareader["LAREDOC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.LUSURC1 = decimal.TryParse(sqlite_datareader["LUSURC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.MILPOC1 = decimal.TryParse(sqlite_datareader["MILPOC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.MINSURI1 = decimal.TryParse(sqlite_datareader["MINSURI1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.MOROCOI1 = decimal.TryParse(sqlite_datareader["MOROCOI1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.RELAPAC1 = decimal.TryParse(sqlite_datareader["RELAPAC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.SCOTIAC1 = decimal.TryParse(sqlite_datareader["SCOTIAC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.SIDERC1 = decimal.TryParse(sqlite_datareader["SIDERC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.TELEFBC1 = decimal.TryParse(sqlite_datareader["TELEFBC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.UNACEMC1 = decimal.TryParse(sqlite_datareader["UNACEMC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                                objData.VOLCABC1 = decimal.TryParse(sqlite_datareader["VOLCABC1_VAL"].ToString(), out tmpvalue) ? tmpvalue: (decimal?)null;
                            }
                            catch
                            {

                            }
                            objLisData.Add(objData);
                        }
                    }
               }
            }
            catch (Exception ex)
            {

            }

            return objLisData;
        }

        public List<DataAccionBE> GenerateDataDet(string desde, string hasta)
        {
            List<DataAccionBE> objListDataAccion = new List<DataAccionBE>();
            DataAccionBE objDataAccion = new DataAccionBE();
            objDataAccion.LIST_DATA_ACCION_DET = new List<DataAccionDetBE>();
            DataAccionDetBE objDataAccionDet = new DataAccionDetBE();

            try
            {
                EmpresaDA objEmpresa = new EmpresaDA();
                var objListEmpresa = objEmpresa.listarEmpresas().Where(x => x.excel == 1).ToList().OrderBy(x => x.nemonico).ToList();

                if (objListEmpresa.Count == 0)
                    return objListDataAccion;

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    string query = "SELECT DISTINCT FECHA, ";

                    foreach (var item in objListEmpresa)
                    {
                        query = query + "'" + item.nemonico + "', ";
                        query = query + item.nemonico + "_VAL, ";
                        query = query + item.nemonico + "_MONTO, ";
                        query = query + item.nemonico + "_VAL_MAX, ";
                        query = query + item.nemonico + "_VAL_MIN, ";
                    }
                    query = query.Substring(0, query.Length - 2);
                    query = query + " FROM DATA";
                    query = query + "  WHERE  cast (strftime('%w', FECHA) as integer) not in (0,6) and fecha between '" + desde + "' and '" + hasta + "'";

                    sqlite_cmd.CommandText = query;

                    using (sqlite_datareader = sqlite_cmd.ExecuteReader())
                    {
                        decimal tmpvalue;
                        while (sqlite_datareader.Read())
                        {
                            try
                            {
                                objDataAccion = new DataAccionBE();
                                objDataAccion.LIST_DATA_ACCION_DET = new List<DataAccionDetBE>();
                                objDataAccion.FECHA = sqlite_datareader.GetValue(0).ToString();
                                
                                for (int i = 1; i < sqlite_datareader.FieldCount; i++)
                                {
                                    objDataAccionDet = new DataAccionDetBE();
                                    
                                    objDataAccionDet.ACCION = sqlite_datareader.GetValue(i).ToString();
                                    i++;
                                    objDataAccionDet.CIERRE = decimal.TryParse(sqlite_datareader.GetValue(i).ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                    i++;
                                    objDataAccionDet.MONTO = decimal.TryParse(sqlite_datareader.GetValue(i).ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                    i++;
                                    objDataAccionDet.MAX = decimal.TryParse(sqlite_datareader.GetValue(i).ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                    i++;
                                    objDataAccionDet.MIN = decimal.TryParse(sqlite_datareader.GetValue(i).ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;

                                    objDataAccion.LIST_DATA_ACCION_DET.Add(objDataAccionDet);
                                }
                                objListDataAccion.Add(objDataAccion);
                                
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return objListDataAccion;
        }


        public void eliminarData()
        {
            EmpresaDA objEmpresa = new EmpresaDA();
            var listEmpresa = objEmpresa.listarEmpresas();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    foreach (var item in listEmpresa)
                    {
                        sqlite_cmd.CommandText = @"DELETE FROM " + item.nemonico + ";";
                        sqlite_cmd.ExecuteNonQuery();
                    }

                    foreach (var item in listEmpresa)
                    {
                        sqlite_cmd.CommandText = @"UPDATE DATA SET " + item.nemonico + "_MONTO = NULL, " + item.nemonico + "_VAL = NULL, " + item.nemonico + "_VAL_MAX = NULL, " + item.nemonico + "_VAL_MIN = NULL;";
                        sqlite_cmd.ExecuteNonQuery();
                    }

                }
                
            }
            catch (Exception ex)
            {

            }
        }



    }
}
