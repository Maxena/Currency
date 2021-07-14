using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{ 


    public class RCurrency
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public string Name { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public int LastPrice { get; set; }
        [Required(ErrorMessage = "this parameter cant be null")]
        public DateTime LastUpdated { get; set; }
        public string ImgUrl { get; set; }
        public string ImgInternetUrl { get; set; }
        public int Type { get; set; }
        [NotMapped]
        [JsonIgnore]
        public ICollection<Prices> Prices { get; set; }
        
    }
}
