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

namespace Arasoi_MINITCC.Tabs.Athlete
{
    public partial class AthleteManagement : Window
    {
        /* This class is just a basis for two other classes that modify 
         * information in the database regarding tournaments */

        public AthleteManagement()
        {
            InitializeComponent();
            LoadTournamentIds();
        }

        // Empty class to be overrided
        public virtual void Apply_Click(object sender, RoutedEventArgs e) { }

        private void LoadTournamentIds()
        {
            using (MySqlConnection connection = ConnectionFactory.GetConnection()) 
            {
                try
                {
                    string command = "SELECT cod_campeonato, nome_campeonato FROM campeonato";

                    using (MySqlCommand commandSELECT = new MySqlCommand(command, connection)) 
                    { 
                        MySqlDataReader reader = commandSELECT.ExecuteReader();

                        while (reader.Read()) 
                        {
                            TournamentIdCB.Items.Add(new ComboBoxItem
                            {
                                Content = reader["nome_campeonato"],
                                Tag = reader["cod_campeonato"]
                            });
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Erro ao carregas os códigos dos torneios: {ex.Message}");
                }
            }
        }

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

            return !string.IsNullOrWhiteSpace(CPFTB.Text)
                && !string.IsNullOrWhiteSpace(NameTB.Text)
                && !string.IsNullOrWhiteSpace(AgeTB.Text)
                && !string.IsNullOrWhiteSpace(HeightTB.Text)
                && !string.IsNullOrWhiteSpace(KyuTB.Text)
                && !string.IsNullOrWhiteSpace(DanTB.Text)
                && !string.IsNullOrWhiteSpace(DojoTB.Text)
                && !string.IsNullOrWhiteSpace(CityTB.Text);
        }
    }

    public class AddAthlete : AthleteManagement
    {
        // This class add new tournaments
        // Add a new tournament
        public override void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                var selectedTournamentId = (ComboBoxItem)TournamentIdCB.SelectedItem;

                try
                {
                    using (MySqlConnection connection = ConnectionFactory.GetConnection())
                    {
                        string command = "INSERT INTO atleta (CPF, nome_atleta, idade, altura, kyu, dan, dojo, cidade, cod_campeonato) " +
                            "VALUES (@CPF, @nome_atleta, @idade, @altura, @kyu, @dan, @dojo, @cidade, @cod_campeonato)";

                        MySqlCommand commandINSERT = new MySqlCommand(command, connection);

                        commandINSERT.Parameters.AddWithValue("@CPF", CPFTB.Text);
                        commandINSERT.Parameters.AddWithValue("@nome_atleta", NameTB.Text);
                        commandINSERT.Parameters.AddWithValue("@idade", Convert.ToInt32(AgeTB.Text));
                        commandINSERT.Parameters.AddWithValue("@altura", Convert.ToDouble(HeightTB.Text));
                        commandINSERT.Parameters.AddWithValue("@kyu", Convert.ToInt32(KyuTB.Text));
                        commandINSERT.Parameters.AddWithValue("@dan", Convert.ToInt32(DanTB.Text));
                        commandINSERT.Parameters.AddWithValue("@dojo", DojoTB.Text);
                        commandINSERT.Parameters.AddWithValue("@cidade", CityTB.Text);
                        commandINSERT.Parameters.AddWithValue("cod_campeonato", Convert.ToInt32(selectedTournamentId.Tag));

                        commandINSERT.ExecuteNonQuery();
                        MessageBox.Show("Registrado!");
                    }
                    var viewModel = (AthleteViewModel)Application.Current.MainWindow.DataContext;
                    viewModel.LoadView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao adicionar atleta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Preencha todos os campos", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

    public class EditAthlete : AthleteManagement
    {
        // This class edits a tournament's information

        public EditAthlete(string cpf, string athleteName, double height, int age, int kyu, int dan, string dojo, string city)
        {
            CPFTB.Text = cpf;
            NameTB.Text = athleteName;
            HeightTB.Text = height.ToString();
            AgeTB.Text = age.ToString();
            KyuTB.Text = kyu.ToString();
            DanTB.Text = dan.ToString();
            DojoTB.Text = dojo.ToString();
            CityTB.Text = city.ToString();
        }

        // Edit the data from the selectd tournament
        public override void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (Check())
            {
                var selectedTournamentId = (ComboBoxItem)TournamentIdCB.SelectedItem;

                try
                {
                    using (MySqlConnection connection = ConnectionFactory.GetConnection())
                    {
                        string command = "UPDATE atleta " +
                            "SET CPF = @CPF, nome_atleta = @nome_atleta, altura = @altura, idade = @idade, kyu = @kyu, dan = @dan, dojo = @dojo, cidade = @cidade, cod_campeonato = @cod_campeonato " +
                            "WHERE CPF = @CPF";
                        MySqlCommand commandUPDATE = new MySqlCommand(command, connection);

                        commandUPDATE.Parameters.AddWithValue("@CPF", CPFTB.Text);
                        commandUPDATE.Parameters.AddWithValue("@nome_atleta", NameTB.Text);
                        commandUPDATE.Parameters.AddWithValue("@idade", Convert.ToInt32(AgeTB.Text));
                        commandUPDATE.Parameters.AddWithValue("@altura", Convert.ToDouble(HeightTB.Text));
                        commandUPDATE.Parameters.AddWithValue("@kyu", Convert.ToInt32(KyuTB.Text));
                        commandUPDATE.Parameters.AddWithValue("@dan", Convert.ToInt32(DanTB.Text));
                        commandUPDATE.Parameters.AddWithValue("@dojo", DojoTB.Text);
                        commandUPDATE.Parameters.AddWithValue("@cidade", CityTB.Text);
                        commandUPDATE.Parameters.AddWithValue("cod_campeonato", selectedTournamentId.Content.ToString());

                        commandUPDATE.ExecuteNonQuery();
                        MessageBox.Show("Mudado!");
                    }
                    var viewModel = (AthleteViewModel)Application.Current.MainWindow.DataContext;
                    viewModel.LoadView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao editar informações do atleta: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
