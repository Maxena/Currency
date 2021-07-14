using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class AddMobToken
    {
      [JsonIgnore]
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
