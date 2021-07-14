using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class Category
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Type { get; set; }
        public string ImgUrl { get; set; }
        public string ImgInternetUrl { get; set; }

        public ICollection<Objects> Objects { get; set; }
        public ICollection<Brand> Brands { get; set; }
    }
}
