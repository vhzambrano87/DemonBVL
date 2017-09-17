using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Data.SQLite;
using System.Security.Permissions;
using System.Globalization;

namespace DataAccess
{
    public class FeriadoDA
    {
        private SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");
        private SQLiteDataReader sqlite_datareader;
        private SQLiteCommand sqlite_cmd;

        public bool validarFeriado()
        {
            bool flag = false;                        
            try
            {
                using (var dbConn = new SQLiteConnection("Data Source=database.db;Version=3"))
                {
                    dbConn.Open();
                    sqlite_cmd = dbConn.CreateCommand();
                    sqlite_cmd.CommandText = "SELECT 1 FROM FERIADOS WHERE FERIADO = '" + DateTime.ParseExact(DateTime.Now.ToShortDateString(), "dd/MM/yyyy", new CultureInfo("en-US"), DateTimeStyles.None) + "'";
                    sqlite_datareader = sqlite_cmd.ExecuteReader();

                    while (sqlite_datareader.Read())
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return flag;
        }
    }
}
