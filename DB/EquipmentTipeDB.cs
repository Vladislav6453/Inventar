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
    internal class EquipmentTipeDB
    {
        DBConnection connection;

        private EquipmentTipeDB(DBConnection db)
        {
            connection = db;
        }

        internal List<EquipmentTipe> SelectAll()
        {
            List<EquipmentTipe> equipmenttipe = new List<EquipmentTipe>();
            if (connection == null)
                return equipmenttipe;

            if (connection.OpenConnection())
            {
                var command = connection.CreateCommand("SELECT e.`ID`, e.`Name` from `EquipmentTipe` e ");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        int ID = dr.GetInt32(0);
                        string Name = string.Empty;
                        if (!dr.IsDBNull(1))
                            Name = dr.GetString("Name");
                        equipmenttipe.Add(new EquipmentTipe
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
            return equipmenttipe;
        }

        static EquipmentTipeDB db;
        public static EquipmentTipeDB GetDb()
        {
            if (db == null)
                db = new EquipmentTipeDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
