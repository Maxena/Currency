using AuthenticationPlugin;
using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{   [ApiVersion("1")]
    [Route("api/app/v{version:apiVersion}/currency/[action]")]
    [ApiController]
    public class AppCurrencyController : ControllerBase {


      
        private IConfiguration _configuration;
        
        private CurrencyShopDb currencyShopDb;

        public AppCurrencyController(IConfiguration configuration, CurrencyShopDb db)
        {
            this.currencyShopDb = db;
            _configuration = configuration;
         }
   

        /// <response code="200">Get List currency successfull</response>
        /// <response code="202">Currency not found</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Currency>))]

        public IActionResult currency()

        {

            var entity = from c in currencyShopDb.Currency select new RCurrency() {
                Id = c.Id,
                ImgInternetUrl = c.ImgInternetUrl,
                ImgUrl = c.ImgUrl,
                LastPrice = c.LastPrice,
                LastUpdated = c.LastUpdated,
                Name = c.Name,
                Prices = c.Prices,
                Type = c.Type,


            };
            if (entity.Count() > 0)
            {
                return Ok(entity);
            }
            else
                return NotFound("Currency not found");
        }
       
        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">prices history not found</response>
        [HttpGet("{name}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult price(string name)
        {
            var prices = from p in currencyShopDb.Prices
                         where p.Name == name
                         select new RPrices()
                         {
                             Id = p.Id,
                             Price = p.Price,
                             Updated = p.Updated,

                         };
            if (prices.Count() > 0)
            {
                return Ok(prices);
            }
            else
            {
                return NotFound("prices history not found");
            }
        }
        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">prices history not found</response>
        [HttpGet("{type}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult type(int type)
        {
            var prices = from c in currencyShopDb.Currency
                         where c.Type == type
                         select new RCurrency()
                         {
                             Id = c.Id,
                             Name=c.Name,
                             LastPrice = c.LastPrice,
                             LastUpdated = c.LastUpdated,
                             Type = c.Type,
                             ImgUrl = c.ImgUrl,
                             ImgInternetUrl = c.ImgInternetUrl,
                         };
            if (prices.Count() > 0)
            {
                return Ok(prices);
            }
            else
            {
                return NotFound("prices history not found");
            }
        }
        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">prices history not found</response>
        [HttpGet("{name}/{day}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult price(string name,int duration)
        {
            var prices = from p in currencyShopDb.Prices
                         where p.Name == name
                         where (DateTime.Now - p.Updated).TotalDays<=duration
                         select new RPrices()
                         {
                             Name = p.Name,
                             Id = p.Id,
                             Price = p.Price,
                             Updated = p.Updated,

                         };
            if (prices.Count() > 0)
            {
                return Ok(prices);
            }
            else
            {
                return NotFound("prices history not found");
            }
        }
        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">max prices not found</response>
        [HttpGet("{name}/max")]
        [MapToApiVersion("1")]
        [ActionName("price")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult maxPrices(string name)
        {
            var prices = currencyShopDb.Prices.Where(p=>p.Name==name).Where(p => (DateTime.Now-p.Updated).TotalDays < 1).Select(c=>new RPrices() {Id= c.Id,Name=c.Name,Price=c.Price,Updated=c.Updated}) .Max(p => p.Price);
                       if (prices > 0)
            {
                return Ok(prices);
            }
            else
            {
                return NotFound("max prices not found");
            }
        }
        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">max prices not found</response>
        [HttpGet("{name}/min")]
        [MapToApiVersion("1")]
        [ActionName("price")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult minPrices(string name)
        {
            var prices = currencyShopDb.Prices.Where(p => p.Name == name).Where(p => (DateTime.Now - p.Updated ).TotalDays < 1).Select(c => new RPrices() { Id = c.Id, Name = c.Name, Price = c.Price, Updated = c.Updated }).Min(p => p.Price);
            if (prices > 0)
            {
                return Ok(prices);
            }
            else
            {
                return NotFound("max prices not found");
            }
        }
  


    }

}
