using AuthenticationPlugin;
using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
        private readonly AuthService _auth;
        private CurrencyShopDb currencyShopDb;

        public AppCurrencyController(IConfiguration configuration, CurrencyShopDb db)
        {
            this.currencyShopDb = db;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }
        // GET: api/<MoneyController>
        /// <response code="200">added data successfuly!</response>
        [HttpPost("prices")]
        [MapToApiVersion("1")]
        [Authorize]
      

        public IActionResult price([FromBody] List<Prices> prices)
        {
            currencyShopDb.Prices.AddRange(prices);
            currencyShopDb.SaveChanges();
            return Ok("added data successfuly!");

        }


        /// <response code="200">added data successfuly!</response>
        [HttpPost("price")]
        [MapToApiVersion("1")]
        [Authorize]
     
        public IActionResult price([FromBody] Prices price)
        {
            currencyShopDb.Prices.Add(price);
            currencyShopDb.SaveChanges();
            return Ok("added data successfuly!");

        }
        /// <response code="200">added data successfuly!</response>
        [HttpPost("currencies")]
        [MapToApiVersion("1")]
        
        [Authorize]
     
        public IActionResult currency([FromBody] List<Currency> currencies)
        {


            currencyShopDb.Currency.AddRange(currencies);
            currencyShopDb.SaveChanges();
            return Ok("added data successfuly!");

        }
        /// <response code="200">added data successfuly!</response>
        [HttpPost("currency")]
        [MapToApiVersion("1")]
        [Authorize]
     
        public IActionResult Currency([FromBody] Currency currency)
        {
            currencyShopDb.Currency.Add(currency);
            currencyShopDb.SaveChanges();
            return Ok("added data successfuly!");
     

        }

        /// <response code="200">Image Uploaded</response>
        /// <response code="404">There is no currency</response>
        /// <response code="401">image does not uploded</response>

        [HttpPost("{id}")]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult imgUrl(int id, [FromBody] imageModel image)
        {


            var stream = new MemoryStream(image.image);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.svg";
            var folder = "wwwroot/currency";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("image does not uploded");
            }
            var entity = currencyShopDb.Currency.Find(id);
            if (entity != null)
            {
                entity.ImgUrl = imageFullPath;
                currencyShopDb.SaveChanges();
                return Ok("Image Uploaded");
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }


        }
        /// <response code="200">Image Uploaded</response>
        /// <response code="404">There is no Image</response>

        [HttpPost("internet/{id}")]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult imgUrl(int id, [FromBody] string url)
        {
            var entity = currencyShopDb.Currency.Find(id);
            if (entity != null)
            {
                entity.ImgInternetUrl = url;
                currencyShopDb.SaveChanges();
                return Ok("Image Uploaded");
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
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
                ImgUrl=c.ImgUrl,
                LastPrice=c.LastPrice,
                LastUpdated=c.LastUpdated,
                Name=c.Name,
                Prices=c.Prices,
                Type=c.Type,
               
                
            } ;
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
        /// <response code="200">save change successfully</response>
        // PUT api/<MoneyController>/5
        [HttpPatch("{id}/{price}")]
        [MapToApiVersion("1")]
        [Authorize]
      

        public IActionResult price(int id, [FromBody]int Price)
        {
            Currency currency = currencyShopDb.Currency.Find(id);
            currency.LastPrice = Price;
            currency.LastUpdated = DateTime.Now;
            Prices price = new Prices()
            {
                Name = currency.Name,
                Price = Price,
                Updated = DateTime.Now
            };
            currencyShopDb.Prices.Add(price);
            currencyShopDb.SaveChanges();
            return Ok("save change successfull");

        }
        /// <response code="200">delete data successfull</response>
        [HttpDelete]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("currency")]
       
        public IActionResult currencyList()
        {

            foreach (var entity in currencyShopDb.Currency) {
                currencyShopDb.Currency.Remove(entity);
            }
               
            currencyShopDb.SaveChanges();
            return Ok("delete data successfully!");

        }
        /// <response code="200">delete data successfull</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [Authorize]
 
        public IActionResult currency(int id)
        {

            var entity = currencyShopDb.Currency.Find(id);
            currencyShopDb.Currency.Remove(entity);
            currencyShopDb.SaveChanges();
            return Ok("delete data successfuly!");

        }

        /// <response code="200">delete data successfull</response>
        [HttpDelete("list")]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("price")]
        public IActionResult priceHistoryList()
        {

            foreach (var entity in currencyShopDb.Prices)
            {
                currencyShopDb.Prices.Remove(entity);
            }

            currencyShopDb.SaveChanges();
            return Ok("delete data successfuly!");

        }
        /// <response code="200">delete data successfull</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("price")]
        
        public IActionResult priceHistory(int id)
        {

            var entity = currencyShopDb.Prices.Find(id);
            currencyShopDb.Prices.Remove(entity);
            currencyShopDb.SaveChanges();
            return Ok("delete data successfuly!");

        }
        /// <response code="200">delete data successfull</response>
        [HttpDelete("{mounth}")]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("price")]
        
        public IActionResult perMonth(int month) {
            currencyShopDb.RemoveRange(currencyShopDb.Prices.Where(p => (DateTime.Now-p.Updated ).TotalDays > (month * 30)));
            currencyShopDb.SaveChanges();
            return Ok("delete data successfuly!");
        }
        /// <response code="200">Change successfull</response>
        /// <response code="404">this currency dose not exist!</response>
        /// <response code="401">No image has been uploaded!</response>
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [ActionName("imgUrl")]

        [Authorize]
        public IActionResult imageUrl(int id, [FromBody]imageModel image)
        {

            var stream = new MemoryStream(image.image);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.svg";
            var folder = "wwwroot/currency";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("No image has been uploaded");
            }
            var entity = currencyShopDb.Currency.Find(id);
            if (entity != null)
            {
                entity.ImgUrl = imageFullPath;
                currencyShopDb.SaveChanges();
                return Ok("Change successfull");
            }
            else
            {
                return NotFound("this brand dose not exist!");
            }
        }
        /// <response code="200">Change successfull</response>
        /// <response code="404">this currency dose not exist!</response>
        [HttpPatch("internet/{id}")]
        [ActionName("imgUrl")]
        [MapToApiVersion("1")]

        [Authorize]
        public IActionResult imageurlInternet(int id, [FromBody] string imageUrl)
        {


            var entity = currencyShopDb.Currency.Find(id);
            if (entity != null)
            {
                entity.ImgUrl = imageUrl;
                currencyShopDb.SaveChanges();
                return Ok("Change successfull");
            }
            else
            {
                return NotFound("this brand dose not exist!");
            }
        }


    }

}
