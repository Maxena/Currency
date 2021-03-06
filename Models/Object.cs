using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{

    public class Objects
    {
        [JsonIgnore]
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProduceYear { get; set; }
        public double Price { get; set; }
        public DateTime DatePosted { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("Brand")]
        public string BrandName { get; set; }

        }
}