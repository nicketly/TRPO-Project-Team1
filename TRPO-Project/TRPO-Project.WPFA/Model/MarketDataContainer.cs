using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA.Model
{
    public class MarketDataContainer
    {
        [JsonProperty("data")]
        public List<List<object>> Data { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }
    }
}
