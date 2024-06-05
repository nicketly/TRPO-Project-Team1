using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TRPO_Project.WPFA.Model;

namespace TRPO_Project.WPFA.ViewModel
{
    public class AssetViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Asset> assets;
        public ObservableCollection<Asset> Assets
        {
            get => assets;
            set
            {
                assets = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; }

        public AssetViewModel()
        {
            Assets = new ObservableCollection<Asset>();
            LoadDataCommand = new RelayCommand(async () => await LoadData());
        }

        private async Task LoadData()
        {
            string connectionString = "Server=(localdb)\\ProjectModels;Database=TRPO-Project.Database;Trusted_Connection=True;";
            string apiUrl = "https://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities.json";

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(apiUrl);
                var json = JObject.Parse(response);
                var securities = (JArray)json["securities"]["data"];
                var columns = (JArray)json["securities"]["columns"];

                int codeIndex = columns.Select((col, index) => new { col, index })
                                       .First(x => x.col.ToString() == "SECID").index;
                int nameIndex = columns.Select((col, index) => new { col, index })
                                       .First(x => x.col.ToString() == "SHORTNAME").index;
                int typeIndex = columns.Select((col, index) => new { col, index })
                                       .First(x => x.col.ToString() == "GROUP").index;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    foreach (var security in securities)
                    {
                        var code = security[codeIndex].ToString();
                        var name = security[nameIndex].ToString();
                        var group = security[typeIndex].ToString();

                        string type = group switch
                        {
                            "stock_shares" => "акция",
                            "stock_bonds" => "облигация",
                            "stock_etf" => "ПИФ",
                            "currency" => "валюта",
                            "precious_metals" => "драгоценный металл",
                            _ => "прочее"
                        };

                        var asset = new Asset
                        {
                            Code = code,
                            Name = name,
                            Type = type
                        };

                        Assets.Add(asset);

                        var command = new SqlCommand("INSERT INTO [dbo].[Code] (Code, Name, Type) VALUES (@Code, @Name, @Type)", conn);
                        command.Parameters.AddWithValue("@Code", asset.Code);
                        command.Parameters.AddWithValue("@Name", asset.Name);
                        command.Parameters.AddWithValue("@Type", asset.Type);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
