using System.Windows.Controls;
using TRPO_Project.WPFA.ViewModel;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System;
using System.Text.RegularExpressions;
using TRPO_Project.WPFA.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Accounting.xaml
    /// </summary>
    public partial class Accounting : UserControl
    {
        private MainViewModel viewModel;
        public List<string> DataListAssets { get; set; }
        public Accounting()
        {
            InitializeComponent();

            viewModel = new MainViewModel();
            DataContext = viewModel;




            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";



            DataService dataService = new DataService();
            List<string> cod = dataService.GetAllAssetCodes();


            DataListAssets = new List<string>();

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
                    var дата = m_data[i].dt[m_data[i].dt.Count - 1];
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


                            DataListAssets.Add(наименование + " " + стоимость.ToString());

                        }
                    }
                    connection.Close();
                }
            }
            ComboBoxAssets.ItemsSource = DataListAssets;


        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {

            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";




            try
            {
                decimal amount = decimal.Parse(TextBoxSum.Text); ;

                DateTime? selectedDate = DatePicker.SelectedDate;
                string dt = selectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string asset = "";
                string assetName = "";
                decimal assetAmount = 0;
                if (ComboBoxAssets.SelectedItem != null)
                {
                    var selectedItemAssets = ComboBoxAssets.SelectedItem;
                    asset = selectedItemAssets.ToString();
                    int count = 0;
                    string[] temp_asset = new string[2];
                    int temp_ind = 0;
                    foreach (var el in asset) 
                    {
                        if (el == ' ') { count++; }
                        else { count = 0; }
                        if (temp_ind == 0 && count > 1) { temp_ind = 1; }
                        if (count < 2) { temp_asset[temp_ind]+=el; }
                    }
                    for (int i=0;i< temp_asset.Length;i++) { temp_asset[i] = temp_asset[i].TrimEnd(); }
                    //string[] parts = asset.Split(' ');
                    //assetName = string.Join(" ", parts.Take(parts.Length - 1)).TrimEnd();
                    assetName = temp_asset[0];
                    assetAmount = decimal.Parse(temp_asset[1]);
                    //assetAmount = decimal.Parse(parts[parts.Length - 1]);
                    // Далее выполняем необходимые действия с переменной selectedItemAssets
                }

                string код = "";
                decimal количество = 0;

                using (var connection = new SqlConnection(connectionString))
                {
                    var sqlQueryAssets = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма,
    History.Операция,
    History.Количество
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE History.Код != 'RUB' AND " + $"Assets.Наименование LIKE N'%{assetName}%' AND History.Сумма = {assetAmount};";
                    SqlCommand command = new SqlCommand(sqlQueryAssets, connection);
                    connection.Open();


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Создание объекта SubItem на основе данных из выборки
                            количество = reader.GetDecimal(5);
                            код = reader.GetString(0);

                        }
                    }

                }
                using (var connection = new SqlConnection(connectionString))
                {
                    var sqlQueryEnter = $"INSERT INTO History (Код, Дата, Цена, Количество, Операция) VALUES ('{код}', '{dt}', {amount}, {количество}, N'Учет');";

                    SqlCommand command = new SqlCommand(sqlQueryEnter, connection);
                    connection.Open();
                    command.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Введите правильные данные");
            }
            LoadBalance();
        }
        private void LoadBalance()
        {
            DataService dataService = new DataService();

            viewModel.BalanceValue = dataService.GetBalanceValue();
        }
    }
}
