﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class Words
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Word { get; set; }
    }
}
