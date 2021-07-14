using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RCategory
    {

        public int Id { get; set; }
        public string Type { get; set; }
        public string ImgUrl { get; set; }
        public string ImgInternetUrl { get; set; }

        public ICollection<RObjects> Objects { get; set; }
        public ICollection<RBrand> Brands { get; set; }
    }
}
