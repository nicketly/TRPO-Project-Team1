using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRPO_Project.WPFA.Model
{
    public class MoexResponse
    {
        [JsonProperty("marketdata")]
        public MarketDataContainer MarketDataContainer { get; set; }
    }
}
