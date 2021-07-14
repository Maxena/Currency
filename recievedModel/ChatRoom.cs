using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RChatRoom
    {
      
        public int Id { get; set; }
        [Required(ErrorMessage = "This Field Can Not Be Empty!!")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "This Field Can Not Be Empty!!")]
        public string Content { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }
        public int Sequence { get; set; }
        public int ReportSequnce { get; set; }
        public int Reply { get; set; }
        public bool Report { get; set; }

    }
}
