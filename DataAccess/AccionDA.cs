using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.SQLite;
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

                            sqlite_cmd.CommandText = @"UPDATE DATA"
                            + " SET " + itemAccion.nemonico + "_VAL = " + itemAccion.valor
                            + ", " + itemAccion.nemonico + "_MONTO = " + itemAccion.monto_negociado
                            + " WHERE FECHA = '" + itemAccion.fecha + "'";
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

        public List<DataDetBE> GenerateDataDet(string desde, string hasta)
        {
            List<DataDetBE> objLisData = new List<DataDetBE>();
            DataDetBE objData = new DataDetBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = @"SELECT DISTINCT FECHA, AIHC1_VAL,AIHC1_MONTO,ALICORC1_VAL ,ALICORC1_MONTO, BACKUBC1_VAL ,BACKUBC1_MONTO, BROCALC1_VAL ,BROCALC1_MONTO, BROCALI1_VAL ,BROCALI1_MONTO, BUENAVC1_VAL ,BUENAVC1_MONTO, CASAGRC1_VAL ,CASAGRC1_MONTO, CONTINC1_VAL ,CONTINC1_MONTO, CORAREI1_VAL ,CORAREI1_MONTO, CPACASC1_VAL ,CPACASC1_MONTO, CREDITC1_VAL ,CREDITC1_MONTO,
                                                CVERDEC1_VAL ,CVERDEC1_MONTO, DNT_VAL ,DNT_MONTO, ETERNII1_VAL ,ETERNII1_MONTO, FERREYC1_VAL ,FERREYC1_MONTO, GRAMONC1_VAL ,GRAMONC1_MONTO, IFS_VAL ,IFS_MONTO, LAREDOC1_VAL ,LAREDOC1_MONTO, LUSURC1_VAL ,LUSURC1_MONTO, MILPOC1_VAL ,MILPOC1_MONTO, MINSURI1_VAL ,MINSURI1_MONTO, MOROCOI1_VAL ,MOROCOI1_MONTO, RELAPAC1_VAL ,RELAPAC1_MONTO,
                                                SCOTIAC1_VAL, SCOTIAC1_MONTO ,SIDERC1_VAL ,SIDERC1_MONTO, TELEFBC1_VAL ,TELEFBC1_MONTO, UNACEMC1_VAL ,UNACEMC1_MONTO, VOLCABC1_VAL, VOLCABC1_MONTO 
                                                FROM DATA 
                                                WHERE  cast (strftime('%w', FECHA) as integer) not in (0,6) and fecha between '" + desde + "' and '" + hasta + "'";

                    using (sqlite_datareader = sqlite_cmd.ExecuteReader())
                    {

                        decimal tmpvalue;
                        while (sqlite_datareader.Read())
                        {
                            objData = new DataDetBE();
                            try
                            {
                                objData.FECHA = sqlite_datareader["FECHA"].ToString();

                                objData.AIHC1_VAL = decimal.TryParse(sqlite_datareader["AIHC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.AIHC1_MONTO = decimal.TryParse(sqlite_datareader["AIHC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.ALICORC1_VAL = decimal.TryParse(sqlite_datareader["ALICORC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.ALICORC1_MONTO = decimal.TryParse(sqlite_datareader["ALICORC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BACKUBC1_VAL = decimal.TryParse(sqlite_datareader["BACKUBC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BACKUBC1_MONTO = decimal.TryParse(sqlite_datareader["BACKUBC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BROCALC1_VAL = decimal.TryParse(sqlite_datareader["BROCALC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BROCALC1_MONTO = decimal.TryParse(sqlite_datareader["BROCALC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BROCALI1_VAL = decimal.TryParse(sqlite_datareader["BROCALI1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BROCALI1_MONTO = decimal.TryParse(sqlite_datareader["BROCALI1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BUENAVC1_VAL = decimal.TryParse(sqlite_datareader["BUENAVC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.BUENAVC1_MONTO = decimal.TryParse(sqlite_datareader["BUENAVC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CASAGRC1_VAL = decimal.TryParse(sqlite_datareader["CASAGRC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CASAGRC1_MONTO = decimal.TryParse(sqlite_datareader["CASAGRC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CONTINC1_VAL = decimal.TryParse(sqlite_datareader["CONTINC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CONTINC1_MONTO = decimal.TryParse(sqlite_datareader["CONTINC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CORAREI1_VAL = decimal.TryParse(sqlite_datareader["CORAREI1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CORAREI1_MONTO = decimal.TryParse(sqlite_datareader["CORAREI1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CPACASC1_VAL = decimal.TryParse(sqlite_datareader["CPACASC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CPACASC1_MONTO = decimal.TryParse(sqlite_datareader["CPACASC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CREDITC1_VAL = decimal.TryParse(sqlite_datareader["CREDITC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CREDITC1_MONTO = decimal.TryParse(sqlite_datareader["CREDITC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CVERDEC1_VAL = decimal.TryParse(sqlite_datareader["CVERDEC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.CVERDEC1_MONTO = decimal.TryParse(sqlite_datareader["CVERDEC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.DNT_VAL = decimal.TryParse(sqlite_datareader["DNT_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.DNT_MONTO = decimal.TryParse(sqlite_datareader["DNT_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.ETERNII1_VAL = decimal.TryParse(sqlite_datareader["ETERNII1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.ETERNII1_MONTO = decimal.TryParse(sqlite_datareader["ETERNII1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.FERREYC1_VAL = decimal.TryParse(sqlite_datareader["FERREYC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.FERREYC1_MONTO = decimal.TryParse(sqlite_datareader["FERREYC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.GRAMONC1_VAL = decimal.TryParse(sqlite_datareader["GRAMONC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.GRAMONC1_MONTO = decimal.TryParse(sqlite_datareader["GRAMONC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.IFS_VAL = decimal.TryParse(sqlite_datareader["IFS_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.IFS_MONTO = decimal.TryParse(sqlite_datareader["IFS_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.LAREDOC1_VAL = decimal.TryParse(sqlite_datareader["LAREDOC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.LAREDOC1_MONTO = decimal.TryParse(sqlite_datareader["LAREDOC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.LUSURC1_VAL = decimal.TryParse(sqlite_datareader["LUSURC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.LUSURC1_MONTO = decimal.TryParse(sqlite_datareader["LUSURC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MILPOC1_VAL = decimal.TryParse(sqlite_datareader["MILPOC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MILPOC1_MONTO = decimal.TryParse(sqlite_datareader["MILPOC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MINSURI1_VAL = decimal.TryParse(sqlite_datareader["MINSURI1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MINSURI1_MONTO = decimal.TryParse(sqlite_datareader["MINSURI1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MOROCOI1_VAL = decimal.TryParse(sqlite_datareader["MOROCOI1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.MOROCOI1_MONTO = decimal.TryParse(sqlite_datareader["MOROCOI1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.RELAPAC1_VAL = decimal.TryParse(sqlite_datareader["RELAPAC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.RELAPAC1_MONTO = decimal.TryParse(sqlite_datareader["RELAPAC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.SCOTIAC1_VAL = decimal.TryParse(sqlite_datareader["SCOTIAC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.SCOTIAC1_MONTO = decimal.TryParse(sqlite_datareader["SCOTIAC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.SIDERC1_VAL = decimal.TryParse(sqlite_datareader["SIDERC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.SIDERC1_MONTO = decimal.TryParse(sqlite_datareader["SIDERC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.TELEFBC1_VAL = decimal.TryParse(sqlite_datareader["TELEFBC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.TELEFBC1_MONTO = decimal.TryParse(sqlite_datareader["TELEFBC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.UNACEMC1_VAL = decimal.TryParse(sqlite_datareader["UNACEMC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.UNACEMC1_MONTO = decimal.TryParse(sqlite_datareader["UNACEMC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.VOLCABC1_VAL = decimal.TryParse(sqlite_datareader["VOLCABC1_VAL"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                                objData.VOLCABC1_MONTO = decimal.TryParse(sqlite_datareader["VOLCABC1_MONTO"].ToString(), out tmpvalue) ? tmpvalue : (decimal?)null;
                            }
                            catch(Exception ex)
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
                        sqlite_cmd.CommandText = @"UPDATE DATA SET " + item.nemonico + " = NULL;";
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
