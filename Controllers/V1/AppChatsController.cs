using CurrencyShop.Data;
using CurrencyShop.Models;
using CurrencyShop.requestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CurrencyShop.Controllers

{
    [Produces("application/json")]
    [ApiVersion("1")]
    [Route("api/app/v{version:apiVersion}/chatRoom/[action]")]
    [ApiController]
    public class AppChatsController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AppChatsController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;

        }
        /// <response code="200">Registerd</response>
        /// <response code="202">user exist</response>
        [HttpPost]
        [MapToApiVersion("1")]
      

        public IActionResult authUsers([FromBody] AuthenticateChatUser authenticate)
        {
            var userWithSameEmail = currencyShopDb.authenticateChatUsers.Where(u => u.Email == authenticate.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {

                return StatusCode(StatusCodes.Status202Accepted, userWithSameEmail);
            }

            var authenticateobj = new AuthenticateChatUser()
            {
                FullName = authenticate.FullName,
                Email = authenticate.Email,
            };
            currencyShopDb.authenticateChatUsers.Add(authenticateobj);
            currencyShopDb.SaveChanges();
            return Ok("Registerd!");
        }
        /// <response code="200">Sent!</response>
        /// <response code="400">send Message failed</response>
        [HttpPost]
        [MapToApiVersion("1")]
        
        public IActionResult messge([FromBody] ChatRoom chat)
        {
            var userWithSameEmail = currencyShopDb.authenticateChatUsers.Where(u => u.Email == chat.UserEmail);
            if (userWithSameEmail != null)
            {
                var chatobj = new ChatRoom()
                {
                    UserEmail = chat.UserEmail,
                    Content = chat.Content,
                    Sequence = chat.Sequence,
                    Like = chat.Like,
                    Dislike = chat.Dislike,
                    Reply = chat.Reply,
                    Report = chat.Report,
                };
                var contains = currencyShopDb.words.Where(c => c.Word.Contains(chat.Content));
                if (contains == null)
                {
                    currencyShopDb.chatRooms.Add(chatobj);
                    currencyShopDb.SaveChanges();

                    return Ok("Sent!");
                }
                else return StatusCode(StatusCodes.Status403Forbidden, "403 forbidden");
            }
           
            else { return BadRequest("send Message failed"); }

        }
        /// <response code="200">saved Change</response>
        /// <response code="400">This message did not recive any reports</response>
        /// <response code="403">sign up first please</response>
        [HttpPost("dec")]
        [ActionName("report")]
        [MapToApiVersion("1")]

        public IActionResult dcrementRepot([FromBody]SeqModel reqChat)
        {
            var userWithSameEmail = currencyShopDb.authenticateChatUsers.Where(u => u.Email == reqChat.Email);
            if (userWithSameEmail != null)
            {
                var entity = currencyShopDb.chatRooms.Find(reqChat.Seq);
                if (entity != null)
                {
                    if (entity.ReportSequnce > 1)
                    {
                        entity.ReportSequnce--;
                        currencyShopDb.SaveChanges();
                        return Ok("saved Change");

                    }
                    else if (entity.ReportSequnce == 1)
                    {
                        entity.ReportSequnce = 0;
                        entity.Report = false;
                        currencyShopDb.SaveChanges();
                        return Ok("saved Change");

                    }
                    else
                    {
                        return BadRequest("This message did not recive any reports!!");
                    }

                }
                return StatusCode(StatusCodes.Status404NotFound);
            }
            else return StatusCode(StatusCodes.Status403Forbidden);
        }
        /// <response code="200">saved Change</response>
        /// <response code="400">This message did not recive any reports</response>
        /// /// <response code="403">sign up first please</response>
        [HttpPost("inc")]
        [ActionName("report")]
        [MapToApiVersion("1")]
    
        public IActionResult incrementRepot([FromBody] SeqModel reqChat)
        {
            var userWithSameEmail = currencyShopDb.authenticateChatUsers.Where(u => u.Email == reqChat.Email);
            if (userWithSameEmail != null)
            {

                var entity = currencyShopDb.chatRooms.Find(reqChat.Seq);
                if (entity != null)
                {
                    entity.ReportSequnce++;
                    entity.Report = true;
                    currencyShopDb.SaveChanges();
                    return Ok("saved change");


                }



                return BadRequest("This message did not recive any reports!!");
            }
            else return StatusCode(StatusCodes.Status403Forbidden);
        }
    
        
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ChatRoom>))]
        public IActionResult chat() 
        {
            var ChatList = from c in currencyShopDb.chatRooms
                           select new RChatRoom()
                           {
                               Id = c.Id,
                               UserEmail = c.UserEmail,
                               Content = c.Content,
                               Sequence = c.Sequence,
                               Like = c.Like,
                               Dislike = c.Dislike,
                               Reply = c.Reply,
                               Report = c.Report,
                           };
            return Ok(ChatList);
        }


    }

}
