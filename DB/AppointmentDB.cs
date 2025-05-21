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
    internal class AppointmentDB
    {
        DBConnection connection;

        private AppointmentDB(DBConnection db)
        {
            connection = db;
        }

        public bool Insert(Appointment appointment)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Appointment` Values (0, @EmployeeID, @EquipmentID, @EquipmentDate, @ReturnDate);select LAST_INSERT_ID();");
                MySqlParameter employeeID = new MySqlParameter("EmployeeID", appointment.EmployeeID);
                cmd.Parameters.Add(employeeID);
                MySqlParameter equipmentID = new MySqlParameter("EquipmentID", appointment.EquipmentID);
                cmd.Parameters.Add(equipmentID);
                MySqlParameter equipmentDate = new MySqlParameter("EquipmentDate", appointment.EquipmentDate);
                cmd.Parameters.Add(equipmentDate);
                MySqlParameter returnDate = new MySqlParameter("ReturnDate", appointment.ReturnDate);
                cmd.Parameters.Add(returnDate);
                try
                {
                    // выполняем запрос через ExecuteScalar, получаем id вставленной записи
                    // если нам не нужен id, то в запросе убираем часть select LAST_INSERT_ID(); и выполняем команду через ExecuteNonQuery
                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        // назначаем полученный id обратно в объект для дальнейшей работы
                        appointment.ID = id;
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

        internal List<Appointment> SelectAll(string search = null)
        {

            List<Appointment> appointments = new List<Appointment>();
            if (connection == null)
                return appointments;

            if (connection.OpenConnection())
            {
                string searchText = "";
                if (!string.IsNullOrEmpty(search))
                {
                    searchText = $"WHERE q.Name Like '%{search}%' OR e.FirstName Like '%{search}%' OR e.LastName Like '%{search}%'";
                }
                var command = connection.CreateCommand($"SELECT a.`ID`, a.`EmployeeID`, e.`FirstName`, e.`LastName`, a.`EquipmentID`, q.`Name`,  a.`EquipmentDate`, a.`ReturnDate` FROM `Appointment` a JOIN `Employees` e ON a.`EmployeeID` = e.`ID` JOIN `Equipment` q ON a.`EquipmentID` = q.`ID` {searchText}");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        Employee emp = new Employee();
                        Equipment eq = new Equipment();
                        int ID = dr.GetInt32(0);
                        int EmployeeID = dr.GetInt32(0);
                        int EquipmentID = dr.GetInt32(0);
                        string FirstName = string.Empty;
                        string LastName = string.Empty;
                        string Name = string.Empty;
                        DateTime EquipmentDate = new DateTime();
                        DateTime ReturnDate = new DateTime();
                        if (!dr.IsDBNull(1))
                            EmployeeID = dr.GetInt32("EmployeeID");
                        if (!dr.IsDBNull(2))
                            EquipmentID = dr.GetInt32("EquipmentID");
                        if (!dr.IsDBNull(3))
                            FirstName = dr.GetString("FirstName");
                        if (!dr.IsDBNull(4))
                            LastName = dr.GetString("LastName");
                        if (!dr.IsDBNull(5))
                            Name = dr.GetString("Name");
                        if (!dr.IsDBNull(6))
                            EquipmentDate = dr.GetDateTime("EquipmentDate");
                        if (!dr.IsDBNull(7))
                            ReturnDate = dr.GetDateTime("ReturnDate");
                        Appointment appointment = new Appointment { EmployeeID = EmployeeID, EquipmentID = EquipmentID, EquipmentDate = EquipmentDate, ReturnDate = ReturnDate };

                        emp.ID = EmployeeID;
                        emp.FirstName = FirstName;
                        emp.LastName = LastName;
                        eq.ID = EquipmentID;
                        eq.Name = Name;
                        appointments.Add(new Appointment
                        {
                            ID = ID,
                            EmployeeID = EmployeeID,
                            EquipmentID = EquipmentID,
                            EquipmentDate = EquipmentDate,
                            ReturnDate = ReturnDate,
                            Employee = emp,
                            Equipment = eq
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return appointments;
        }

        internal bool Update(Appointment edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Appointment` set `ID`=@ID, `EmployeeID`=@EmployeeID,`EquipmentID`=@EquipmentID,`EquipmentDate`=@EquipmentDate, `ReturnDate`=@ReturnDate where `id` = {edit.ID}");
                mc.Parameters.Add(new MySqlParameter("ID", edit.ID));
                mc.Parameters.Add(new MySqlParameter("EmployeeID", edit.EmployeeID));
                mc.Parameters.Add(new MySqlParameter("EquipmentID", edit.EquipmentID));
                mc.Parameters.Add(new MySqlParameter("EquipmentDate", edit.EquipmentDate));
                mc.Parameters.Add(new MySqlParameter("ReturnDate", edit.ReturnDate));

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


        internal bool Remove(Appointment remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `appointment` where `id` = {remove.ID}");
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

        static AppointmentDB db;
        public static AppointmentDB GetDb()
        {
            if (db == null)
                db = new AppointmentDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}
