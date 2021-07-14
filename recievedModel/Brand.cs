using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RBrand
    {

        public int Id { get; set; }
        [ForeignKey("Category")]
        public int categoryId { get; set; }

        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string ImgInternetUrl { get; set; }
        [NotMapped]
        ICollection<Objects> objects { get;set; }

    }
}
