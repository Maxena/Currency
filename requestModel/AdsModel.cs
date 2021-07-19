using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.requestModel
{
    public class AdsModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[] imageArray{ get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url{ get; set; }
    }
}
