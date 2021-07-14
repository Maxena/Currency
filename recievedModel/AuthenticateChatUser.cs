using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurrencyShop.Models
{
    public class RAuthenticateChatUser
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "This Field Can Not Be Empty!!")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This Field Can Not Be Empty!!")]
        public string Email { get; set; }

        public ICollection<ChatRoom> chatRooms { get; set; }
    }
}
