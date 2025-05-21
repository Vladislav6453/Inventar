using Inventar;
using Inventar.DB;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventar.Model
{
    public class OborudPoisk
    {
        private readonly DBConnection dbConnection;

        public List<Equipment> SearchOborud(string search)
        {
            List<Equipment> result = new();
            List<EquipmentTipe> tipes = new();

            string query = $"SELECT e.ID AS 'EquipmentID', e.Name AS 'EquipmentName', InventoryNumber, DateOfPurchase, ServiceLife, Price, t.Name AS 'EquipmentTipeName', e.IDEquipmentTipe AS 'IDTipe' FROM Equipment e JOIN EquipmentTipe t ON IDEquipmentTipe = t.ID WHERE e.Name LIKE @search";

            if (dbConnection.OpenConnection())
            {
                using (var mc = dbConnection.CreateCommand(query))
                {

                    mc.Parameters.Add(new MySqlParameter("search", $"%{search}%"));
                    using (var dr = mc.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            var Equipment = new Equipment();
                            Equipment.ID = dr.GetInt32("EquipmentID");
                            Equipment.Name = dr.GetString("EquipmentName");
                            Equipment.InventoryNumber = dr.GetInt32("InventoryNumber");
                            Equipment.DateOfPurchase = dr.GetDateTime("DateOfPurchase");
                            Equipment.ServiceLife = dr.GetInt32("ServiceLife");
                            Equipment.Price = dr.GetInt64("Price");
                            Equipment.IDEquipmentTipe = dr.GetInt32("IDTipe");

                            var EquipmentTipe = tipes.FirstOrDefault(s => s.ID == Equipment.IDEquipmentTipe);
                            if (EquipmentTipe == null)
                            {
                                EquipmentTipe = new EquipmentTipe();
                                EquipmentTipe.ID = Equipment.IDEquipmentTipe;
                                EquipmentTipe.Name = dr.GetString("EquipmentTipeName");

                                tipes.Add(EquipmentTipe);
                            }

                            Equipment.EquipmentTipe = EquipmentTipe;

                            result.Add(Equipment);
                        }
                    }
                }
                dbConnection.CloseConnection();
            }
            return result;

        }

        

        // синглтон start
        static OborudPoisk table;
        private OborudPoisk(DBConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }
        public static OborudPoisk GetTable()
        {
            if (table == null)
                table = new OborudPoisk(DBConnection.GetDbConnection());
            return table;
        }
    }
}
