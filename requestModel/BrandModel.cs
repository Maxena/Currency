using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.requestModel
{
    public class BrandModel
    {
        public string Name { get; set; }
        public string ImageInternetUrl { get; set; }
        [NotMapped]
        public byte[] ImageUrl { get; set; }
    }
}
