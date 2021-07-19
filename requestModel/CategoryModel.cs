using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.requestModel
{
    public class CategoryModel
    {
        public string Type { get; set; }
       
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgInternetUrl { get; set; }
        [NotMapped]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[] ImageArray { get; set; }
    }
}
