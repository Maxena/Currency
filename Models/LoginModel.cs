using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class LoginModel
    {
        [DefaultValue("admin@example.com")]
        public string email { get; set; }
        [DefaultValue("123456")]
        public string password { get; set; }
    }
}
