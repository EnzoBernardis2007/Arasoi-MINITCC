using Arasoi_MINITCC.DatabaseManagement;
using MySql.Data.MySqlClient;
using System;
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

namespace Arasoi_MINITCC.Tabs.Tournament
{
    public partial class TournamentManagement : Window
    {
        public TournamentManagement()
        {
            InitializeComponent();
        }

        public virtual void Apply_Click(object sender, RoutedEventArgs e) { }

        public void Close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Certeza que quer descartar as alterações?", "Confirmação", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes) this.Close();
        }

        public bool Check()
        {
            return CodeTB.Text.Trim() != ""
                && NameTB.Text.Trim() != ""
                && FiliationTB.Text.Trim() != ""
                && DataPickerStart.SelectedDate.HasValue
                && DataPickerEnd.SelectedDate.HasValue;
        }
    }

    public class AddTournament : TournamentManagement
    {
        public override void Apply_Click(object sender, RoutedEventArgs e) 
        {
            if (Check())
            {
                using (MySqlConnection connection = ConnectionFactory.GetConnection())
                {
                    connection.Open();
                    string command = "INSERT INTO campeonato (cod_campeonato, nome_campeonato, filiacao, data_inicio, data_fim) " +
                        "VALUES (@cod_campeonato, @nome_campeonato, @filiacao, @data_inicio, @data_fim)";
                    MySqlCommand commandINSERT = new MySqlCommand(command, connection);

                    commandINSERT.Parameters.AddWithValue("@cod_campeonato", CodeTB.Text);
                    commandINSERT.Parameters.AddWithValue("@nome_campeonato", NameTB.Text);
                    commandINSERT.Parameters.AddWithValue("@filiacao", FiliationTB.Text);
                    commandINSERT.Parameters.AddWithValue("@data_inicio", DataPickerStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    commandINSERT.Parameters.AddWithValue("@data_fim", DataPickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));

                    commandINSERT.ExecuteNonQuery();
                    MessageBox.Show("Registrado!");
                }
            }
        }
    }

    public class EditTournament : TournamentManagement 
    {

        public EditTournament(string id, string name, string filiation, DateTime dateStart, DateTime dateEnd) 
        { 
            CodeTB.IsReadOnly = false; 

            CodeTB.Text = id;
            NameTB.Text = name;
            FiliationTB.Text = filiation;
            DataPickerStart.SelectedDate = dateStart;
            DataPickerEnd.SelectedDate = dateEnd;
        }

        public override void Apply_Click(object sender, RoutedEventArgs e) 
        {
            if (Check()) 
            { 
                using (MySqlConnection connection = ConnectionFactory.GetConnection())
                {
                    connection.Open();
                    string command = "UPDATE campeonato " + 
                        "SET nome_campeonato = @nome_campeonato, filiacao = @filiacao, data_inicio = @data_inicio, data_fim = @data_fim " +
                        "WHERE cod_campeonato = @cod_campeonato";
                    MySqlCommand commandUPDATE = new MySqlCommand(command, connection);

                    commandUPDATE.Parameters.AddWithValue("@cod_campeonato", CodeTB.Text);
                    commandUPDATE.Parameters.AddWithValue("@nome_campeonato", NameTB.Text);
                    commandUPDATE.Parameters.AddWithValue("@filiacao", FiliationTB.Text);
                    commandUPDATE.Parameters.AddWithValue("@data_inicio", DataPickerStart.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    commandUPDATE.Parameters.AddWithValue("@data_fim", DataPickerEnd.SelectedDate.Value.ToString("yyyy-MM-dd"));

                    commandUPDATE.ExecuteNonQuery();
                    MessageBox.Show("Mudado!");
                }
            }
        }
    }
}
