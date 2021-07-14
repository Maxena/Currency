using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RPannelUser
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
  


    }
}
