using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Inventar.DB
{
    public class DBConnection
    {
        MySqlConnection _connection;

        public void Config()
        {
            // пример строки подключения: "userid=student;password=student;database=1125_new_2025;server=192.168.200.13;characterset=utf8mb4";
            // конфигурация берется из файла / из окошка / из настроек / или статично
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.UserID = "student";
            sb.Password = "student";
            sb.Server = "192.168.200.13";
            sb.Database = "Inventar";
            sb.CharacterSet = "utf8mb4";

            // инициализация объекта для подключения к субд
            _connection = new MySqlConnection(sb.ToString());
        }

        public bool OpenConnection()
        {
            if (_connection == null)
                Config();

            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void CloseConnection()
        {
            if (_connection == null)
                return;

            try
            {
                _connection.Close();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
                return;
            }
        }

        public MySqlCommand CreateCommand(string sql)
        {
            return new MySqlCommand(sql, _connection);
        }

        static DBConnection dbConnection;
        private DBConnection() { }
        public static DBConnection GetDbConnection()
        {
            if (dbConnection == null)
                dbConnection = new DBConnection();
            return dbConnection;
        }
    }
}
