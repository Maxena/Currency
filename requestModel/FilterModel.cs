using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.requestModel
{
    public class FilterModel

    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int StartPrice { get; set; }
        public int LastPrice { get; set; }
        public int CategoryId { get; set; }
        public string BrandName { get; set; }
    }
}
