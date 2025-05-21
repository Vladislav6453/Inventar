using Inventar.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventar.DB
{
    internal class EmployeeDB
    {
        DBConnection connection;

        private EmployeeDB(DBConnection db)
        {
            connection = db;
        }

        public bool Insert(Employee employee)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                MySqlCommand cmd = connection.CreateCommand("insert into `Employees` Values (0, @FirstName, @LastName, @SurName, @PhoneNumber, @WorkExperience, @Email, @IDJobTitle);select LAST_INSERT_ID();");
                MySqlParameter firstName = new MySqlParameter("FirstName", employee.FirstName);
                cmd.Parameters.Add(firstName);
                MySqlParameter lastname = new MySqlParameter("LastName", employee.LastName);
                cmd.Parameters.Add(lastname);
                MySqlParameter surname = new MySqlParameter("SurName", employee.SurName);
                cmd.Parameters.Add(surname);
                MySqlParameter phonenumber = new MySqlParameter("PhoneNumber", employee.PhoneNumber);
                cmd.Parameters.Add(phonenumber);
                MySqlParameter workexperience = new MySqlParameter("WorkExperience", employee.WorkExperience);
                cmd.Parameters.Add(workexperience);
                MySqlParameter email = new MySqlParameter("Email", employee.Email);
                cmd.Parameters.Add(email);
                MySqlParameter idjobtitle = new MySqlParameter("IDJobTitle", employee.IDJobTitle);
                cmd.Parameters.Add(idjobtitle);
                try
                {

                    int id = (int)(ulong)cmd.ExecuteScalar();
                    if (id > 0)
                    {
                        MessageBox.Show(id.ToString());
                        employee.ID = id;
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

        internal List<Employee> SelectAll(string search = null)
        {
            List<Employee> employees = new List<Employee>();
            if (connection == null)
                return employees;

            if (connection.OpenConnection())
            {
                string searchText = "";
                if (!string.IsNullOrEmpty(search))
                {
                    searchText = $"WHERE e.LastName Like '%{search}%' OR e.FirstName Like '%{search}%' OR e.SurName Like '%{search}%' ";
                }
                var command = connection.CreateCommand($"SELECT e.`ID`, e.`FirstName`, e.`LastName`, e.`SurName`, e.`PhoneNumber`, e.`WorkExperience`, e.`Email`, e.`IDJobTitle`, j.`Name`  from `Employees` e JOIN `JobTitle` j ON e.`IDJobTitle` = j.`ID` {searchText}");
                try
                {
                    MySqlDataReader dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        JobTitle job = new JobTitle();
                        int ID = dr.GetInt32(0);
                        string FirstName = string.Empty;
                        string LastName = string.Empty;
                        string SurName = string.Empty;
                        string PhoneNumber = string.Empty;
                        double WorkExperience = 0;
                        string Email = string.Empty;
                        int IDJobTitle = 0;
                        string Name = string.Empty;
                        if (!dr.IsDBNull(1))
                            FirstName = dr.GetString("FirstName");
                        if (!dr.IsDBNull(2))
                            LastName = dr.GetString("LastName");
                        if (!dr.IsDBNull(3))
                            SurName = dr.GetString("SurName");
                        if (!dr.IsDBNull(4))
                            PhoneNumber = dr.GetString("PhoneNumber");
                        if (!dr.IsDBNull(5))
                            WorkExperience = dr.GetInt64("WorkExperience");
                        if (!dr.IsDBNull(6))
                            Email = dr.GetString("Email");
                        if (!dr.IsDBNull(7))
                            IDJobTitle = dr.GetInt32("IDJobTitle");
                        if (!dr.IsDBNull(8))
                            Name = dr.GetString("Name");

                        job.ID = ID; job.Name = Name;
                        employees.Add(new Employee
                        {
                            ID = ID,
                            FirstName = FirstName,
                            LastName = LastName,
                            SurName = SurName,
                            PhoneNumber = PhoneNumber,
                            WorkExperience = WorkExperience,
                            Email = Email,
                            IDJobTitle = IDJobTitle,
                            JobTitle = job
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.CloseConnection();
            return employees;
        }

        internal bool Update(Employee edit)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"update `Employees` set `FirstName`=@FirstName, " +
                    $"`LastName`=@LastName, `SurName`=@SurName, " +
                    $"`PhoneNumber`=@PhoneNumber, `WorkExperience`=@WorkExperience, `Email`=@Email," +
                    $" `IDJobTitle`=@IDJobTitle where `ID` = {edit.ID}");

                mc.Parameters.Add(new MySqlParameter("FirstName", edit.FirstName));
                mc.Parameters.Add(new MySqlParameter("LastName", edit.LastName));
                mc.Parameters.Add(new MySqlParameter("SurName", edit.SurName));
                mc.Parameters.Add(new MySqlParameter("PhoneNumber", edit.PhoneNumber));
                mc.Parameters.Add(new MySqlParameter("WorkExperience", edit.WorkExperience));
                mc.Parameters.Add(new MySqlParameter("Email", edit.Email));
                mc.Parameters.Add(new MySqlParameter("IDJobTitle", edit.IDJobTitle));

                try
                {
                    mc.ExecuteNonQuery();
                    result = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + mc);
                }
            }
            connection.CloseConnection();
            return result;
        }


        internal bool Remove(Employee remove)
        {
            bool result = false;
            if (connection == null)
                return result;

            if (connection.OpenConnection())
            {
                var mc = connection.CreateCommand($"delete from `Employees` where `ID` = {remove.ID}");
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

        static EmployeeDB db;
        public static EmployeeDB GetDb()
        {
            if (db == null)
                db = new EmployeeDB(DBConnection.GetDbConnection());
            return db;
        }
    }
}

