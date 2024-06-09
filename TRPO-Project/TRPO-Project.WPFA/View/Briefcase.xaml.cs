using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using TRPO_Project.WPFA.ViewModel;


namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Briefcase.xaml
    /// </summary>

    
    public partial class Briefcase : UserControl
    {
        private MainViewModel viewModel;
        public Briefcase()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;

            LoadBalance();
            LoadBriefcase();
            LoadProfitExpected();
            LoadProfitFixed();
            LoadIncome();


            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";

            DataService dataService = new DataService();
            List<string> cod = dataService.GetAllAssetCodes();

            var menuStock = new List<SubItem>();
            var menuCurrency = new List<SubItem>();
            var menuMetals = new List<SubItem>();
            var menuBonds = new List<SubItem>();
            var menuPIF = new List<SubItem>();

            using (var connection = new SqlConnection(connectionString))
            {
                List<d_data> m_data = new List<d_data>();
                var connectionString1 = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
                bool name_add = false;
                foreach (string codCode in cod)
                {
                    using (var connection1 = new SqlConnection(connectionString1))
                    {
                        var sqlQueryEnter = $"SELECT Assets.Наименование, History.Дата, History.Цена, History.Количество FROM History JOIN Assets ON History.Код = Assets.Код WHERE History.Код = '{codCode}' ORDER BY History.Дата";

                        m_data.Add(new d_data(codCode));
                        SqlCommand command = new SqlCommand(sqlQueryEnter, connection1);
                        connection1.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                // Создание объекта SubItem на основе данных из выборки
                                string наименование = reader.GetString(0);
                                DateTime дата = reader.GetDateTime(1);
                                decimal стоимость = reader.GetDecimal(2);
                                decimal количество = reader.GetDecimal(3);
                                m_data[^1].Add(дата, стоимость, количество);
                                if (!name_add)
                                {
                                    m_data[^1].Set_name(наименование);
                                    name_add = true;
                                }
                            }
                        }
                    }
                }
                

                var test = m_data[1].cost[^1];
                for (int i = 0; i < cod.Count; i++)
                {
                    string тип = "";
                    var дата = m_data[i].dt[m_data[i].dt.Count-1];
                    string dt = дата.ToString("yyyy-MM-dd HH:mm:ss");
                    decimal цена = m_data[i].cost[m_data[i].dt.Count - 1];
                    decimal количество = m_data[i].count[m_data[i].dt.Count - 1];
                    var sqlQuerySample = @"
SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма,
    History.Дата
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Операция = N'Учет' AND History.Код LIKE  " + $"'%{cod[i]}%' AND History.Дата = '{dt}';";


                    var sqlQuerySamplePay = @"
SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма,
    History.Дата
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE History.Код LIKE  " + $"'%{cod[i]}%' AND History.Дата = '{dt}' AND ( Операция LIKE N'%Зачисление дивидендов%' OR Операция LIKE N'%Зачисление купонов%');";

                    SqlCommand command = new SqlCommand(sqlQuerySample, connection);
                    connection.Open();
                    //command.ExecuteScalar();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            тип = reader.GetString(2).TrimEnd();
                            string наименование = reader.GetString(1);
                            decimal стоимость = reader.GetDecimal(3);
                            string стоимостьК = стоимость.ToString() + " ₽";
                            string изменение = "0";
                            if (тип == "Акция")
                            {
                                menuStock.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимостьК, изменение));
                            }

                            if (тип == "Валюта")
                            {
                                menuCurrency.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимостьК, изменение));
                            }

                            if (тип == "Драгоценный металл")
                            {
                                menuMetals.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимостьК, изменение));
                            }
                            if (тип == "Облигация")
                            {
                                menuBonds.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимостьК, изменение));
                            }
                            if (тип == "ПИФ")
                            {
                                menuPIF.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимостьК, изменение));
                            }
                        }
                    }
                    connection.Close();

                    SqlCommand command2 = new SqlCommand(sqlQuerySamplePay, connection);
                    connection.Open();
                    //command.ExecuteScalar();

                    using (var reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            тип = reader.GetString(2).TrimEnd();
                            string наименование = reader.GetString(1);
                            decimal стоимость = reader.GetDecimal(3);
                            string стоимостьК = стоимость.ToString() + " ₽";

                            Menu2.Children.Add(new UserControlMenuItem(new ItemMenu(PackIconKind.History, наименование, стоимостьК)));
                        }
                    }
                    connection.Close();
                }

                Menu.Children.Add(new UserControlMenuItem(new ItemMenu("Акции", menuStock, PackIconKind.ChartDonut)));
                Menu.Children.Add(new UserControlMenuItem(new ItemMenu("Валюта", menuCurrency, PackIconKind.ChartDonut)));
                Menu.Children.Add(new UserControlMenuItem(new ItemMenu("Драгоценный металл", menuMetals, PackIconKind.ChartDonut)));
                Menu.Children.Add(new UserControlMenuItem(new ItemMenu("Облигация", menuBonds, PackIconKind.ChartDonut)));
                Menu.Children.Add(new UserControlMenuItem(new ItemMenu("ПИФ", menuPIF, PackIconKind.ChartDonut)));


            }





        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LoadBalance()
        {
            DataService dataService = new DataService();
            viewModel.BalanceValue = dataService.GetBalanceValue();
        }

        private void LoadBriefcase()
        {
            DataService dataService = new DataService();
            viewModel.BriefcaseValue = dataService.GetBriefcaseValue();
        }

        private void LoadProfitExpected()
        {
            DataService dataService = new DataService();
            viewModel.ProfitExpectedValue = (decimal)dataService.GetProfitExpected();
        }
        private void LoadProfitFixed()
        {
            DataService dataService = new DataService();
            viewModel.ProfitFixedValue = (decimal)dataService.GetProfitFixed();
        }

        private void LoadIncome()
        {
            DataService dataService = new DataService();
            viewModel.IncomeValue = (decimal)dataService.GetIncome();
        }
        private void ButtonAccount_Click(object sender, RoutedEventArgs e)
        {
            Main parentWindow = (Main)Application.Current.MainWindow;
            parentWindow.RenderPages.Children.Clear();
            parentWindow.RenderPages.Children.Add(new Account());

        }

        private void ButtonAssets_Click(object sender, RoutedEventArgs e)
        {
            Main parentWindow = (Main)Application.Current.MainWindow;
            parentWindow.RenderPages.Children.Clear();
            parentWindow.RenderPages.Children.Add(new Assets());

        }

        private void ButtonPayments_Click(object sender, RoutedEventArgs e)
        {
            Main parentWindow = (Main)Application.Current.MainWindow;
            parentWindow.RenderPages.Children.Clear();
            parentWindow.RenderPages.Children.Add(new Payments());

        }

        private void ButtonAccounting_Click(object sender, RoutedEventArgs e)
        {
            Main parentWindow = (Main)Application.Current.MainWindow;
            parentWindow.RenderPages.Children.Clear();
            parentWindow.RenderPages.Children.Add(new Accounting());

        }
    }
}
