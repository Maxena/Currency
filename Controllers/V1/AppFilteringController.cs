using CurrencyShop.Data;
using CurrencyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Controllers
{
    [ApiVersion("1")]
    [Route("api/app/v{version:apiVersion}/filter/[action]")]
    [ApiController]
    public class AppFilteringController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AppFilteringController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;

        }
        /// <response code="200">Words Added</response>
        [HttpPost]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult word([FromBody] List<Words> words)
        {
            currencyShopDb.words.AddRange(words);
            currencyShopDb.SaveChanges();
            return Ok("Words Added");
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public IQueryable<object> word()
        {
            var WordList = from w in currencyShopDb.words
                           select new RWords()
                           {
                               Id = w.Id,
                               Word = w.Word,

                           };
            return WordList;
        }

    }
}
