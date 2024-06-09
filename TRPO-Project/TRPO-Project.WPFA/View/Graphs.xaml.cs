using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
using MaterialDesignThemes.Wpf;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.NativeInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using TRPO_Project.WPFA.ViewModel;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Graphs.xaml
    /// </summary>
    /// 
    public class DataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    
    public struct d_data
    {
        public d_data(String Code) 
        {
            code = Code;
            dt = new List<DateTime>();
            cost = new List<Decimal>();
            count = new List<Decimal>();
            name = "";
        }
        public void Add(DateTime Dt, Decimal Cost, Decimal Count) 
        {
            dt.Add(Dt);
            cost.Add(Cost);
            count.Add(Count);
        }
        public void Set_name(String Name)
        {
            name=Name;
        }
        public String code { get; set; }
        public String name { get; set; }
        public List<DateTime> dt { get; set; }
        public List<Decimal> cost { get; set; }
        public List<Decimal> count { get; set; }
    }
    public partial class Graphs : UserControl
    {

        public SeriesCollection AssetsBriefcase { get; set; }
        public SeriesCollection AssetsTypes { get; set; }

        public Graphs()
        {
            InitializeComponent();

            DatePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;
     

        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
            DataService dataService = new DataService();
            List<string> cod = dataService.GetAllAssetCodes();
            DateTime? selectedDate = DatePicker.SelectedDate;
            string dt = selectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");

            var connectionString1 = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Integrated Security=True;";
            List<d_data> m_data= new List<d_data>();
            bool name_add=false;
            foreach (string codCode in cod) 
            {
                using (var connection = new SqlConnection(connectionString1))
                {
                    var sqlQueryEnter = $"SELECT Assets.Наименование, History.Дата, History.Цена, History.Количество FROM History JOIN Assets ON History.Код = Assets.Код WHERE History.Код = '{codCode}' ORDER BY History.Дата";

                    m_data.Add(new d_data(codCode));
                    SqlCommand command = new SqlCommand(sqlQueryEnter, connection);
                    connection.Open();
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

            var test = m_data[0].cost[^1];

            if (AssetsBriefcase != null)
            {
                AssetsBriefcase.Clear();
            }
            else { AssetsBriefcase = new SeriesCollection(); }
            if (AssetsTypes != null)
            {
                AssetsTypes.Clear();
            }
            else { AssetsTypes = new SeriesCollection(); }
            if (AssetsPrice != null)
            {
                AssetsPrice.Series.Clear();
            }
            else { AssetsPrice.Series = new SeriesCollection(); }
            if (AssetsPrice.AxisX != null)
            {
                AssetsPrice.AxisX.Clear();
            }
            else { AssetsPrice.AxisX = new AxesCollection(); }

            for (int i = 0; i < m_data[0].dt.Count; i++)
            {
                var дата = m_data[0].dt[i];
                var цена = m_data[0].cost[i];
                var количество = m_data[0].count[i];
            }
            AssetsPrice.AxisX.Add(new Axis
            {
                Title = "Дата",
                LabelFormatter = val => new DateTime((long)val).ToString("dd/MM/yyyy")
            });
            if (selectedDate.HasValue)
            {


                for (int j = 0; j < cod.Count; j++)
                {

                    decimal цена = 0;
                    decimal количество = 0;
                    ObservableCollection<DateTimePoint> newDataPoints = new ObservableCollection<DateTimePoint>();
                    for (int i = 0; i < m_data[j].dt.Count; i++)
                    {
                        var дата = m_data[j].dt[i];
                        цена = m_data[j].cost[i];
                        количество = m_data[j].count[i];
                        DateTime? dateTime = дата.ToUniversalTime();
                        string dtn = дата.ToString("yyyy-MM-dd HH:mm:ss");
                        if (dateTime > selectedDate)
                        {
                            break;
                        }
                        newDataPoints.Add(new DateTimePoint((DateTime)dateTime, (double)цена));
                    }
                    if (newDataPoints != null)
                    {
                        var chartValues = new ChartValues<DateTimePoint>(newDataPoints);
                        var newSeries = new LineSeries
                        {
                            Title = cod[j],
                            DataLabels = true,
                            Values = chartValues
                        };
                        AssetsPrice.Series.Add(newSeries);
                    }
                    

                }


                using (var connection = new SqlConnection(connectionString))
                {

                    for (int i = 0; i < cod.Count; i++)
                    {
                        string тип = "";


                        var sqlQuerySample = @"
SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма,
    History.Дата,
    History.Количество
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Операция = N'Учет' AND History.Код LIKE  " + $"'%{cod[i]}%' AND History.Дата < '{dt}' ORDER BY History.Дата;";


                        string наименование = " ";
                        decimal стоимость = 0;
                        decimal количество = 0;

                        SqlCommand command = new SqlCommand(sqlQuerySample, connection);
                        connection.Open();
                        //command.ExecuteScalar();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Создание объекта SubItem на основе данных из выборки
                                тип = reader.GetString(2).TrimEnd();
                                наименование = reader.GetString(1);
                                стоимость = reader.GetDecimal(3);
                                количество = reader.GetDecimal(5);



                            }
                        }
                        if (наименование != " ")
                        {


                            AssetsBriefcase.Add(new PieSeries
                            {
                                Title = cod[i],
                                Values = new ChartValues<ObservableValue>
                                        {
                                            new ObservableValue((double)стоимость),

                                                },

                                DataLabels = true,
                            });

                            AssetsTypes.Add(new PieSeries
                            {
                                Title = cod[i],
                                Values = new ChartValues<ObservableValue>
                                        {
                                            new ObservableValue((double)количество),

                                                },

                                DataLabels = true,
                            });
                        }
                        connection.Close();
                    }
                }
            }
                    var sqlQueryAssetsP = "";
                    var sqlQueryType = "";
                    var sqlQueryAssetsC = "";

                




            if (selectedDate.HasValue)
            {

                sqlQueryAssetsP = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Дата BETWEEN @FromDate AND @ToDate AND  History.Код != 'RUB' 
AND Операция = N'Учет' 
;
";
                sqlQueryType = @"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    History.Количество,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE  Дата = @ПоследняяДата AND History.Код != 'RUB' AND Операция = N'Учет'

";

                sqlQueryAssetsC = @$"
DECLARE @ПоследняяДата smalldatetime;
SET @ПоследняяДата = (SELECT MAX(Дата) FROM [dbo].[History]);

SELECT 
    History.Код,
    Assets.Наименование,
    Assets.Тип,
    History.Сумма
FROM History
JOIN Assets ON History.Код = Assets.Код
WHERE Дата BETWEEN @FromDate AND @ToDate AND History.Код != 'RUB' AND Операция = N'Учет' AND (Тип = N'Акция' OR  Тип = N'Валюта' OR Тип = N'Драгоценный металл');
";


            }


                DataContext = this;
            }
        }

        }



        


    


