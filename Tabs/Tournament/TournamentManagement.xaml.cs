using Arasoi_MINITCC.DatabaseManagement;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /* This class is just a basis for two other classes that modify 
         * information in the database regarding tournaments */

        public TournamentManagement()
        {
            InitializeComponent();
        }

        // Empty class to be overrided
        public virtual void Apply_Click(object sender, RoutedEventArgs e) { }

        // Close the window
        public void Close_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user really wants to close the window
            MessageBoxResult messageBoxResult = MessageBox.Show("Certeza que quer descartar as alterações?", "Confirmação", MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes) this.Close();
        }

        // Check if all fields of the form are filled in (incomplete function)
        public bool Check()
        {
            return !string.IsNullOrWhiteSpace(CodeTB.Text)
                && !string.IsNullOrWhiteSpace(NameTB.Text)
                && !string.IsNullOrWhiteSpace(FiliationTB.Text)
                && DataPickerStart.SelectedDate.HasValue
                && DataPickerEnd.SelectedDate.HasValue;
        }
    }

    public class AddTournament : TournamentManagement
    {
        // This class add new tournaments
        // Add a new tournament
        public override void Apply_Click(object sender, RoutedEventArgs e) 
        {
            if (Check())
            {
                try
                {
                    using (MySqlConnection connection = ConnectionFactory.GetConnection())
                    {
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
                    var viewModel = (TournamentViewModel)Application.Current.MainWindow.DataContext;
                    viewModel.LoadView();
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show($"Erro ao adicionar campeonato: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } else
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public class EditTournament : TournamentManagement 
    {
        // This class edits a tournament's information

        public EditTournament(string id, string name, string filiation, DateTime dateStart, DateTime dateEnd) 
        { 
            CodeTB.IsReadOnly = true; 

            // Show all the data from the selected tournament
            CodeTB.Text = id;
            NameTB.Text = name;
            FiliationTB.Text = filiation;
            DataPickerStart.SelectedDate = dateStart;
            DataPickerEnd.SelectedDate = dateEnd;
        }

        // Edit the data from the selectd tournament
        public override void Apply_Click(object sender, RoutedEventArgs e) 
        {
            if (Check()) 
            { 
                try
                {
                    using (MySqlConnection connection = ConnectionFactory.GetConnection())
                    {
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
                    var viewModel = (TournamentViewModel)Application.Current.MainWindow.DataContext;
                    viewModel.LoadView();
                } catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao editar informações do torneio: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
