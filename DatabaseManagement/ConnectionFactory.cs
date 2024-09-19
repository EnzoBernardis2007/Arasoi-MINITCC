using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arasoi_MINITCC.DatabaseManagement
{
    internal class ConnectionFactory
    {
        // This class create connections to the database

        static readonly string path = "Server=localhost;Database=arasoi;User Id=root;Password=";
        public static MySqlConnection GetConnection()
        {
            try
            {
                MySqlConnection connection = new MySqlConnection(path);
                return connection;
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Erro ao tentar criar conexão com o banco de dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
