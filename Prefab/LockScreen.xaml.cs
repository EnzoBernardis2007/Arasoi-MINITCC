using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Arasoi_MINITCC.Prefab
{
        /// <summary>
        /// Lógica interna para LockScreen.xaml
        /// </summary>
    public partial class LockScreen : Window
    {
        private string Password { get; set; }

        public LockScreen()
        {
            InitializeComponent();
            string filePath = "C:\\Users\\berna\\source\\repos\\Arasoi-MINITCC\\JSON\\Credentials.json";
            Credential credentials = JSONmanagement.LoadCredentials(filePath);

            Password = credentials != null ? credentials.Password : null;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordText.Password == Password)
            {
                string filePath = "C:\\Users\\berna\\source\\repos\\Arasoi-MINITCC\\JSON\\Credentials.json";
                string jsonContent = File.ReadAllText(filePath);
                JObject jsonObject = JObject.Parse(jsonContent);
                jsonObject["Entered"] = true;
                string updatedJsonContent = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                File.WriteAllText(filePath, updatedJsonContent);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else MessageBox.Show("Senha errada.", "Erro");
        }
    }
}
