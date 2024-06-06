using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
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
            var sqlQueryStock = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Дата = @ПоследняяДата AND History.Код != 'RUB' AND Операция = N'Учет' AND Тип = N'Акция';
";

            var sqlQueryCurrency = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Дата = @ПоследняяДата AND History.Код != 'RUB' AND Операция = N'Учет' AND Тип = N'Валюта';
";

            var sqlQueryMetals = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Дата = @ПоследняяДата AND History.Код != 'RUB' AND Операция = N'Учет' AND Тип = N'Драгоценный металл';
";

            var menuStock = new List<SubItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sqlQueryStock, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            string наименование = reader.GetString(1);
                            decimal стоимость = reader.GetDecimal(3);
                            string изменение = "0";

                            menuStock.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимость, изменение));
                        }
                    }
                }
            }

            var item6 = new ItemMenu("Акции", menuStock, PackIconKind.ChartDonut);

            var menuCurrency = new List<SubItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sqlQueryCurrency, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            string наименование = reader.GetString(1);
                            decimal стоимость = reader.GetDecimal(3);
                            string изменение = "0";

                            menuCurrency.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимость, изменение));
                        }
                    }
                }
            }
            var item1 = new ItemMenu("Валюта", menuCurrency, PackIconKind.ChartDonut);


            var menuMetals = new List<SubItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(sqlQueryMetals, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            string наименование = reader.GetString(1);
                            decimal стоимость = reader.GetDecimal(3);
                            string изменение = "0";

                            menuMetals.Add(new SubItem(PackIconKind.CheckboxMarkedCircleOutline, наименование, стоимость, изменение));
                        }
                    }
                }
            }
            var item4 = new ItemMenu("Драгоценные металлы", menuMetals, PackIconKind.ChartDonut);

            //var item0 = new ItemMenu("Dashboard", new UserControl(), PackIconKind.ViewDashboard);

            //Menu.Children.Add(new UserControlMenuItem(item0));
            Menu.Children.Add(new UserControlMenuItem(item6));
            Menu.Children.Add(new UserControlMenuItem(item1));
            Menu.Children.Add(new UserControlMenuItem(item4));




            var item10 = new ItemMenu(PackIconKind.History, "SBER", "1379.50 ₽");
            var item11 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");
            var item12 = new ItemMenu(PackIconKind.History, "SBER", "768 ₽");

            Menu2.Children.Add(new UserControlMenuItem(item10));
            Menu2.Children.Add(new UserControlMenuItem(item11));
            Menu2.Children.Add(new UserControlMenuItem(item12));

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
