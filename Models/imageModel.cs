using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class imageModel
    {
        [NotMapped]
        public byte[] image { get; set; }

    }
}
