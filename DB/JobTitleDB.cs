using Inventar.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventar.DB
{
    internal class JobTitleDB
    {
        DBConnection connection;

        private JobTitleDB(DBConnection db)
        {
            connection = db;
        }

        internal List<JobTitle> SelectAll()
        {
            List<JobTitle> jobtitle = new List<JobTitle>();
            if (connection == null)
                return jobtitle;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT j.`ID`, j.`Name` from `JobTitle` j ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int ID = dr.GetInt32(0);
                        string Name = string.Empty;
                        if (!dr.IsDBNull(1))
                            Name = dr.GetString("Name");

                        jobtitle.Add(new JobTitle
                        {
                            ID = ID,
                            Name = Name
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return jobtitle;
        }

        static JobTitleDB db;
        public static JobTitleDB GetDb()
        {
            if (db == null)
                db = new JobTitleDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
