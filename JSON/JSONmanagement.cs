using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arasoi_MINITCC.Prefab
{
    internal class JSONmanagement
    {
        public static Credential LoadCredentials(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonData = File.ReadAllText(filePath);
                    Credential credentials = JsonConvert.DeserializeObject<Credential>(jsonData);
                    return credentials;
                }
                else
                {
                    throw new FileNotFoundException("O arquivo JSON não foi encontrado.");
                }
            }
            catch (Exception ex)
            {
                // Trate exceções conforme necessário
                MessageBox.Show($"Erro ao carregar as credenciais: {ex.Message}");
                return null;
            }
        }
    }

    public class Credential
    {
        public string Password { get; set; }
        public bool Entered { get; set; }
    }
}
