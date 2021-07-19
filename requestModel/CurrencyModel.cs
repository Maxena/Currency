using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.requestModel
{
    public class CurrencyModel
    {
        
        [Required(ErrorMessage = "this parameter cant be null")]
        public string Name { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public int LastPrice { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public DateTime LastUpdated { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte[] ImgArray { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ImgInternetUrl { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public int Type { get; set; }
    }
}
