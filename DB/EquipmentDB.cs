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
    internal class EquipmentDB
    {
        DBConnection connection;

        private EquipmentDB(DBConnection db)
        {
            connection = db;
        }

        public bool Insert(Equipment equipment)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Equipment` Values (0, @Name, @InventoryNumber, @DateOfPurchase, @ServiceLife, @Price, @IDEquipmentTipe);select LAST_INSERT_ID();");
                MySqlParameter name = new MySqlParameter("Name", equipment.Name);
                cmd.Parameters.Add(name);
                MySqlParameter inventorynumber = new MySqlParameter("InventoryNumber", equipment.InventoryNumber);
                cmd.Parameters.Add(inventorynumber);
                MySqlParameter dateOfpurchase = new MySqlParameter("DateOfPurchase", equipment.DateOfPurchase);
                cmd.Parameters.Add(dateOfpurchase);
                MySqlParameter servicelife = new MySqlParameter("ServiceLife", equipment.ServiceLife);
                cmd.Parameters.Add(servicelife);
                MySqlParameter price = new MySqlParameter("Price", equipment.Price);
                cmd.Parameters.Add(price);
                MySqlParameter idequipmenttipe = new MySqlParameter("IDEquipmentTipe", equipment.IDEquipmentTipe);
                cmd.Parameters.Add(idequipmenttipe);
                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        equipment.ID = id;
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Запись не добавлена");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        internal List<Equipment> SelectAll(string search = null)
        {
            List<Equipment> equipments = new List<Equipment>();
            if (connection == null)
                return equipments;

            if (connection.OpenConnection())
            {
                string searchText = "";
                if (!string.IsNullOrEmpty(search))
                {
                    searchText = $"WHERE e.Name Like '%{search}%'";
                }
                var command = connection.CreateCommand($"SELECT e.`ID`, e.`Name`, e.`InventoryNumber`, e.`DateOfPurchase`, e.`ServiceLife`, e.`Price`, e.`IDEquipmentTipe`, t.`Name` EqName  from `Equipment` e JOIN `EquipmentTipe` t ON e.`IDEquipmentTipe` = t.`ID` {searchText}");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        EquipmentTipe Eq = new EquipmentTipe();
                        int ID = dr.GetInt32(0);
                        string Name = string.Empty;
                        int InventoryNumber = 0;
                        DateTime DateOfPurchase = new DateTime();
                        double ServiceLife = 0;
                        decimal Price = 0;
                        int IDEquipmentTipe = 0;
                        string EqName = null;
                        if (!dr.IsDBNull(1))
                            Name = dr.GetString("Name");
                        if (!dr.IsDBNull(2))
                            InventoryNumber = dr.GetInt32("InventoryNumber");
                        if (!dr.IsDBNull(3))
                            DateOfPurchase = dr.GetDateTime("DateOfPurchase");
                        if (!dr.IsDBNull(4))
                            ServiceLife = dr.GetInt32("ServiceLife");
                        if (!dr.IsDBNull(5))
                            Price = dr.GetDecimal("Price");
                        if (!dr.IsDBNull(6))
                            IDEquipmentTipe = dr.GetInt32("IDEquipmentTipe");
                        if (!dr.IsDBNull(7))
                            EqName = dr.GetString("EqName");

                        Eq.Name = EqName;
                        Eq.ID = IDEquipmentTipe;
                        equipments.Add(new Equipment
                        {
                            ID = ID,
                            Name = Name,
                            InventoryNumber = InventoryNumber,
                            DateOfPurchase = DateOfPurchase,
                            ServiceLife = ServiceLife,
                            Price = Price,
                            IDEquipmentTipe = IDEquipmentTipe,
                            EquipmentTipe = Eq
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return equipments;
        }

        internal bool Update(Equipment edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Equipment` set `Name`=@Name, `InventoryNumber`=@InventoryNumber, `DateOfPurchase`=@DateOfPurchase, `ServiceLife`=@ServiceLife, `Price`=@Price, `IDEquipmentTipe`=@IDEquipmentTipe where `ID` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("Name", edit.Name));
                mc.Parameters.Add(new MySqlParameter("InventoryNumber", edit.InventoryNumber));
                mc.Parameters.Add(new MySqlParameter("DateOfPurchase", edit.DateOfPurchase));
                mc.Parameters.Add(new MySqlParameter("ServiceLife", edit.ServiceLife));
                mc.Parameters.Add(new MySqlParameter("Price", edit.Price));
                mc.Parameters.Add(new MySqlParameter("IDEquipmentTipe", edit.IDEquipmentTipe));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Equipment remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Equipment` where `ID` = {remove.ID}");
                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return result;
        }

        static EquipmentDB db;
        public static EquipmentDB GetDb()
        {
            if (db == null)
                db = new EquipmentDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
