using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class Ads
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string AdsUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}
