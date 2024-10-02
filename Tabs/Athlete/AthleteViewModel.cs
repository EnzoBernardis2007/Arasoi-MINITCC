using Arasoi_MINITCC.DatabaseManagement;
using Arasoi_MINITCC.Tabs.Tournament;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Arasoi_MINITCC.Tabs.Athlete
{
    internal class AthleteViewModel : INotifyPropertyChanged
    {
        // this class takes care of viewing the athletes tab in the MainWindow
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<AthleteCard> _athleteCards;
        public ObservableCollection<AthleteCard> AthleteCards
        {
            get { return _athleteCards; }
            set
            {
                if (_athleteCards != value)
                {
                    _athleteCards = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AthleteCards)));
                }
            }
        }

        public AthleteViewModel()
        {
            LoadView();
        }

        // Get data from the database and add that data to a collection of 'TournamentCard'
        public void LoadView()
        {
            AthleteCards = new ObservableCollection<AthleteCard>();

            using (MySqlConnection connection = ConnectionFactory.GetConnection())
            {
                try
                {
                    connection.Open();
                    string command = "SELECT * FROM atletas";

                    using (MySqlCommand commandSELECT = new MySqlCommand(command, connection))
                    {
                        using (MySqlDataReader reader = commandSELECT.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string cpf = reader["CPF"].ToString();
                                string name = reader["nome_atleta"].ToString();

                                AthleteCards.Add(new AthleteCard(cpf, name));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar os campeonatos: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class AthleteCard
    {
        public string Name { get; set; }
        public string CPF { get; set; }

        public AthleteCard(string cpf, string name)
        {
            CPF = cpf;
            Name = name;
        }

        // deletes a record in the database that has the same CPF
        public void Delete()
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Tem certeza que quer deletar?", "Confirmação", MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.No) return;

                using (MySqlConnection connection = ConnectionFactory.GetConnection())
                {
                    connection.Open();
                    string command = "DELETE FROM atleta WHERE CPF = @CPF";
                    MySqlCommand commandDELETE = new MySqlCommand(command, connection);

                    commandDELETE.Parameters.AddWithValue("@CPF", CPF);

                    commandDELETE.ExecuteNonQuery();
                }
                var viewModel = (AthleteViewModel)Application.Current.MainWindow.DataContext;
                viewModel.LoadView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao deletar um atleta: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
