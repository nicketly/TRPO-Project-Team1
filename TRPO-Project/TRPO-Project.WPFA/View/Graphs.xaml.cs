using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
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
using System.IO;


namespace TRPO_Project.WPFA.View
{
    /// <summary>
    /// Логика взаимодействия для Graphs.xaml
    /// </summary>
    public partial class Graphs : UserControl
    {
        public SeriesCollection LastHourSeries { get; set; }
        public SeriesCollection LastHourSeries2 { get; set; }
        public SeriesCollection LastHourSeries1 { get; set; }
        public SeriesCollection LastHourSeries4 { get; set; }
        public Graphs()
        {
            InitializeComponent();

            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(9),
                        new ObservableValue(4),
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(7),
                    }
                }
            };

            LastHourSeries4 = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(9),
                        new ObservableValue(4),
                        new ObservableValue(5),
                        new ObservableValue(3),
                        new ObservableValue(1),
                        new ObservableValue(2),
                        new ObservableValue(3),
                        new ObservableValue(7),
                    }
                }
            };

            LastHourSeries1 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Values",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(30),

                    },

                    DataLabels = true, // Показывать значения на секторах
                },

                new PieSeries
                {
                    Title = "Val",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(70),

                    },

                    DataLabels = true, // Показывать значения на секторах
                }


            };
            LastHourSeries2 = new SeriesCollection
            {
                new PieSeries
                {
                    Title = "Values",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(30),

                    },

                    DataLabels = true, // Показывать значения на секторах
                },

                new PieSeries
                {
                    Title = "Val",
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(70),

                    },

                    DataLabels = true, // Показывать значения на секторах
                }


            };



            DataContext = this;
        }


    }


}
