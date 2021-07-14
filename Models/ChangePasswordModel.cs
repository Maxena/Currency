using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class ChangePasswordModel
    {
        public String OldPassword { get; set; }
        public String NewPassword { get; set; }
        [Compare("NewPassword",ErrorMessage ="the new Password and confirm password dosnt match")]
        public String ConfirmPassword { get; set; }
    }
}
