using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.SQLite;

namespace DataAccess
{
    public class EmpresaDA
    {
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
        private SQLiteDataReader sqlite_datareader;
        private SQLiteCommand sqlite_cmd;


        public void createTableEmpresa()
        {
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS EMPRESA(
                                           ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                           CATEGORIA      VARCHAR,
                                           NEMONICO       VARCHAR,
                                           NOMBRE         VARCHAR
                                        );";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void InsertarEmpresa(EmpresaBE objEmpresa)
        {
            try
            {
                createTableEmpresa();

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO EMPRESA (CATEGORIA,NEMONICO,NOMBRE) VALUES ('" + objEmpresa.categoria + "','" + objEmpresa.nemonico + "','" + objEmpresa.nombre + "')";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void ModificarEmpresa(EmpresaBE objEmpresa)
        {
            try
            {
                createTableEmpresa();

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "UPDATE EMPRESA SET CATEGORIA = '" + objEmpresa.categoria + "', NEMONICO = '" + objEmpresa.nemonico + "', NOMBRE = '" + objEmpresa.nombre + "' WHERE ID = " + objEmpresa.id.ToString();
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void EliminarEmpresa(string nemonico)
        {
            try
            {

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    try {
                        sqlite_cmd.CommandText = "DROP TABLE " + nemonico;
                        sqlite_cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        sqlite_cmd.CommandText = "DELETE FROM EMPRESA WHERE NEMONICO = '" + nemonico + "'";
                        sqlite_cmd.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public List<EmpresaBE> listarEmpresas()
        {
            List<EmpresaBE> objListEmpresa = new List<EmpresaBE>();
            EmpresaBE objEmpresa;
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM EMPRESA ORDER BY NOMBRE";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        objEmpresa = new EmpresaBE();
                        objEmpresa.id = Convert.ToInt32(sqlite_datareader["ID"].ToString()) ;
                        objEmpresa.nemonico = sqlite_datareader["NEMONICO"].ToString();
                        objEmpresa.categoria = sqlite_datareader["CATEGORIA"].ToString();
                        objEmpresa.nombre = sqlite_datareader["NOMBRE"].ToString();

                        objListEmpresa.Add(objEmpresa);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objListEmpresa;
        }

        public EmpresaBE obtenerEmpresa(string nemonico)
        {
            EmpresaBE objEmpresa = new EmpresaBE();
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM EMPRESA WHERE NEMONICO = '" + nemonico +"'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        objEmpresa.id = Convert.ToInt32(sqlite_datareader["ID"].ToString());
                        objEmpresa.nemonico = sqlite_datareader["NEMONICO"].ToString();
                        objEmpresa.categoria = sqlite_datareader["CATEGORIA"].ToString();
                        objEmpresa.nombre = sqlite_datareader["NOMBRE"].ToString();

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objEmpresa;
        }

    }
}
