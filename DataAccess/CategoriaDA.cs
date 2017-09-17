using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.SQLite;

namespace DataAccess
{
    public class CategoriaDA
    {
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
        private SQLiteDataReader sqlite_datareader;
        private SQLiteCommand sqlite_cmd;

        public void createTableCategoria()
        {
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS CATEGORIA(
                                           ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                           NOMBRE         VARCHAR
                                        );";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void insertarCategoria(CategoriaBE objCategoria)
        {
            try
            {
                createTableCategoria();

                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "INSERT INTO CATEGORIA (NOMBRE) VALUES ('" + objCategoria.nombre + "')";
                    sqlite_cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
        }


        public List<CategoriaBE> listarCategorias()
        {
            List<CategoriaBE> objListCategoria = new List<CategoriaBE>();
            CategoriaBE objCategoria;
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT * FROM CATEGORIA ORDER BY NOMBRE";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        objCategoria = new CategoriaBE();
                        objCategoria.id = Convert.ToInt32(sqlite_datareader["ID"].ToString());
                        objCategoria.nombre = sqlite_datareader["NOMBRE"].ToString();

                        objListCategoria.Add(objCategoria);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return objListCategoria;
        }
    }
}
