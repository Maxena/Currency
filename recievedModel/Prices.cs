using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RPrices
    {
  
        public int Id { get; set; }
        public int Price { get; set; }
        [ForeignKey("Currency")]
        public string Name { get; set; }
        public DateTime  Updated { get; set; }
    }
}
