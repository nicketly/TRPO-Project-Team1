using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using TRPO_Project.WPFA.Model;
using TRPO_Project.WPFA.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Assets.xaml
    /// </summary>
    public partial class Assets : UserControl

    {
       
        private MainViewModel viewModel;
        private string sqlQueryCurrency;

        public Assets()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;

            LoadBalance();


            List<string> DataListTypeAssets = new List<string>();

            DataListTypeAssets.Add("Акция");
            DataListTypeAssets.Add("Валюта");
            DataListTypeAssets.Add("Драгоценный металл");
            DataListTypeAssets.Add("Облигация");
            DataListTypeAssets.Add("ПИФ");



            // Установка списка как источника данных для ComboBox
            ComboBoxTypeAssets.ItemsSource = DataListTypeAssets;
        }

        private void LoadBalance()
        {
            DataService dataService = new DataService();
            viewModel.BalanceValue = dataService.GetBalanceValue();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

           

            

            try
            {
                decimal amount = decimal.Parse(textBoxSum.Text); ;
                string ticker = textBoxTicker.Text;
                string name = textBoxName.Text;
                decimal quantity = decimal.Parse(textBoxCount.Text);

                
                string type = "";

                if (ComboBoxTypeAssets.SelectedItem != null )
                {
                    var selectedItem = ComboBoxTypeAssets.SelectedItem;
                    type = selectedItem.ToString();
                }



                DateTime now = DateTime.Now;
                string dt = now.ToString("yyyy-dd-MM HH:mm:ss");
                DataService dataService = new DataService();
                List<string> cod = dataService.GetAllAssetCodes();

                var sqlQueryAdd = $"INSERT INTO History (Код, Дата, Цена, Количество, Операция) VALUES ('{ticker}', '{dt}', {amount}, {quantity}, N'Покупка');";

                var sqlQueryAddAsses = $"INSERT INTO Assets(Код, Наименование, Тип) VALUES('{ticker}', '{name}', '{type}');";
                if (cod.Contains(ticker))
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        
                        SqlCommand command = new SqlCommand(sqlQueryAdd, connection);
                        connection.Open();
                        command.ExecuteScalar();



                    }
                }
                else
                {
                    using (var connection = new SqlConnection(connectionString))
                    {

                        SqlCommand command = new SqlCommand(sqlQueryAddAsses, connection);
                        connection.Open();
                        command.ExecuteScalar();

                        SqlCommand command1 = new SqlCommand(sqlQueryAdd, connection);
                        connection.Open();
                        command1.ExecuteScalar();

                    }

                }
                LoadBalance();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите правильные данные");
            }

            
            
            


            

            

        }

        private void ButtonSell_Click(object sender, RoutedEventArgs e)
        {

            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

            

            try
            {
                decimal amount = decimal.Parse(textBoxSum.Text); ;
                string ticker = textBoxTicker.Text;
                string name = textBoxName.Text;
                decimal quantity = decimal.Parse(textBoxCount.Text);

                ComboBoxItem selectedItem = (ComboBoxItem)ComboBoxTypeAssets.SelectedItem;


                string type = "";

                if (selectedItem != null)
                {
                    type = selectedItem.Content.ToString();
                }


                DateTime now = DateTime.Now;
                string dt = now.ToString("yyyy-dd-MM HH:mm:ss");

                DataService dataService = new DataService();
                List<string> cod = dataService.GetAllAssetCodes();

                var sqlQuerySell = $"INSERT INTO History (Код, Дата, Цена, Количество, Операция) VALUES ('{ticker}', '{dt}', {amount}, '{quantity}', N'Продажа');";

                if (cod.Contains(ticker))
                {
                    using (var connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand(sqlQuerySell, connection);
                        connection.Open();
                        command.ExecuteScalar();

                    }
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
