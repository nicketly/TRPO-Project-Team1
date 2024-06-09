using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using TRPO_Project.WPFA.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        private MainViewModel viewModel;
        public Account()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;

            LoadBalance();
        }
        private void LoadBalance()
        {
            DataService dataService = new DataService();

            viewModel.BalanceValue = dataService.GetBalanceValue();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {

            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

            



            try
            {
                decimal amount = decimal.Parse(TextBoxSum.Text); ;


                DateTime now = DateTime.Now;


                using (var connection = new SqlConnection(connectionString))
                {
                    string dt = now.ToString("yyyy-MM-dd HH:mm:ss");
                    var sqlQueryEnter = $"INSERT INTO History (Код, Дата, Цена, Количество, Операция) VALUES ('RUB','{dt}' , {amount}, 1.00, N'Пополнение счета');";


                    SqlCommand command = new SqlCommand(sqlQueryEnter, connection);
                    connection.Open();
                    command.ExecuteScalar();

                }
                LoadBalance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите правильные данные");
            }
        }

        private void ButtonWithdraw_Click(object sender, RoutedEventArgs e)

        {
            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

            

            try
            {
                decimal amount = decimal.Parse(TextBoxSum.Text); ;


                DateTime now = DateTime.Now;


                using (var connection = new SqlConnection(connectionString))
                {
                    string dt = now.ToString("yyyy-MM-dd HH:mm:ss");
                    var sqlQueryWithdraw = $"INSERT INTO History (Код, Дата, Цена, Количество, Операция) VALUES ('RUB', '{dt}', {amount}, 1.00, N'Вывод средств');";
                    SqlCommand command = new SqlCommand(sqlQueryWithdraw, connection);
                    connection.Open();
                    var test =command.ExecuteScalar();


                }
                LoadBalance();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите правильные данные");
            }
        }
    }
}
