using AuthenticationPlugin;
using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using CurrencyShop.requestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{
    [ApiVersion("1")]
    [Route("api/web/v{version:apiVersion}/currency/[action]")]
    [ApiController]
    public class WebCurrencyController : ControllerBase
    {



        private IConfiguration _configuration;
        private readonly AuthService _auth;
        private CurrencyShopDb currencyShopDb;

        public WebCurrencyController(IConfiguration configuration, CurrencyShopDb db)
        {
            this.currencyShopDb = db;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }
        // GET: api/<MoneyController>
        /// <response code="200">added data successfuly!</response>
        [HttpPost]
        [MapToApiVersion("1")]
        [Authorize]


        public IActionResult price([FromBody] List<Prices> prices)
        {
            currencyShopDb.Prices.AddRange(prices);
            currencyShopDb.SaveChanges();
            return Ok("added data successfuly!");

        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile postedFile)
        {
            if (postedFile == null || postedFile.Length == 0)
            {
                return RedirectToAction("ImportExcel");
            }

            //Get file
            var newfile = new FileInfo(postedFile.FileName);
            var fileExtension = newfile.Extension;

            //Check if file is an Excel File
            if (fileExtension.Contains(".xls"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await postedFile.CopyToAsync(ms);

                    using (ExcelPackage package = new ExcelPackage(ms))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                        int totalRows = workSheet.Dimension.Rows;

                        List<Currency> customerList = new List<Currency>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            customerList.Add(new Currency
                            {
                              
                                Name = workSheet.Cells[i, 2].Value.ToString(),
                                LastPrice = Convert.ToInt16(workSheet.Cells[i, 3].Value),
                                LastUpdated = Convert.ToDateTime(workSheet.Cells[i, 4].Value),
                                ImgInternetUrl = workSheet.Cells[i, 6].Value.ToString(),
                                Type = Convert.ToInt16(workSheet.Cells[i, 7].Value),
                               
                            });
                        }

                        currencyShopDb.Currency.AddRange(customerList);
                        await currencyShopDb.SaveChangesAsync();
                    }
                }

            }
            return Ok("set data ok");
        }
            /// <response code="200">added data successfuly!</response>
            [HttpPost]
        [MapToApiVersion("1")]

        [Authorize]
        [ActionName("")]
        public IActionResult currency([FromBody] List<CurrencyModel> currencies)
        {

            foreach (var item in currencies)
            {
                Currency currency = new Currency();
                if (item.ImgArray.Length > 0)
                {
                    var stream = new MemoryStream(item.ImgArray);
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.png";
                    var folder = "wwwroot/currency";
                    var fullPath = $"{folder}/{file}";
                    var imageFullPath = fullPath.Remove(0, 7);
                    var response = FileHelper.UploadPhoto(stream, folder, file);

                    if (!response)
                    {
                        return BadRequest("image does not uploded");
                    }
                    currency.ImgUrl = imageFullPath;

                }
                if (item.ImgInternetUrl != null) {
                    currency.ImgInternetUrl = item.ImgInternetUrl;
                }
                currency.Name = item.Name;
                currency.LastUpdated = item.LastUpdated;
                currency.LastPrice = item.LastPrice;
                Prices prices = new Prices();
                prices.Name = item.Name;
                prices.Price = item.LastPrice;
                currencyShopDb.Currency.Add(currency);
                currencyShopDb.Prices.Add(prices);
                currencyShopDb.SaveChanges();

            }

            return Ok("added data successfuly!");

        }


        /// <response code="200">Get List currency successfull</response>
        /// <response code="202">Currency not found</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ActionName("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Currency>))]

        public IActionResult currency([FromQuery] bool all = false, int type = 0)

        {
            if (all)
            {
                var entity = from c in currencyShopDb.Currency
                             select new RCurrency()
                             {
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
            if (type > 0)
            {
                var currency = from c in currencyShopDb.Currency
                               where c.Type == type
                               select new RCurrency()
                               {
                                   Id = c.Id,
                                   Name = c.Name,
                                   LastPrice = c.LastPrice,
                                   LastUpdated = c.LastUpdated,
                                   Type = c.Type,
                                   ImgUrl = c.ImgUrl,
                                   ImgInternetUrl = c.ImgInternetUrl,
                               };
                if (currency.Count() > 0)
                {
                    return Ok(currency);
                }
                else
                {
                    return NotFound("prices history not found");
                }
            }

            return NotFound("Currency not found");
        }

        /// <response code="200">Get List of Price successfull</response>
        /// <response code="202">prices history not found</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Prices>))]

        public IActionResult price([FromQuery] string name = "دلار آمریکا", int duration = 0, bool max = false, bool min = false)
        {
            if (max && name.Length > 0)
            {
                var prices = currencyShopDb.Prices.Where(p => p.Name == name).Where(p => (DateTime.Now - p.Updated).TotalDays < 1).Select(c => new RPrices() { Id = c.Id, Name = c.Name, Price = c.Price, Updated = c.Updated }).Max(p => p.Price);
                if (prices > 0)
                {
                    return Ok(prices);
                }
                else
                {
                    return NotFound("max prices not found");
                }
            }
            if (max && name.Length > 0)
            {
                var prices = currencyShopDb.Prices.Where(p => p.Name == name).Where(p => (DateTime.Now - p.Updated).TotalDays < 1).Select(c => new RPrices() { Id = c.Id, Name = c.Name, Price = c.Price, Updated = c.Updated }).Min(p => p.Price);
                if (prices > 0)
                {
                    return Ok(prices);
                }
                else
                {
                    return NotFound("max prices not found");
                }
            }
            else if (duration > 0 && name.Length > 0)
            {
                var pricesObj = from p in currencyShopDb.Prices
                                where p.Name == name
                                where (DateTime.Now - p.Updated).TotalDays <= duration
                                select new RPrices()
                                {
                                    Name = p.Name,
                                    Id = p.Id,
                                    Price = p.Price,
                                    Updated = p.Updated,

                                };
                if (pricesObj.Count() > 0)
                {
                    return Ok(pricesObj);
                }
                else
                {
                    return NotFound("prices history not found");
                }
            }
            else if (name.Length > 0)
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
            return BadRequest("invalid Parameter");
        }




        /// <response code="200">save change successfully</response>
        // PUT api/<MoneyController>/5
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [Authorize]


        public IActionResult price(int id, [FromBody] PriceModel model)
        {
            Currency currency = currencyShopDb.Currency.Find(id);
            currency.LastPrice = model.Price;
            currency.LastUpdated = DateTime.Now;
            Prices price = new Prices()
            {
                Name = currency.Name,
                Price = model.Price,
                Updated = DateTime.Now
            };
            currencyShopDb.Prices.Add(price);
            currencyShopDb.SaveChanges();
            return Ok("save change successfull");

        }
        /// <response code="200">delete data successfull</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("del")]
        
        public IActionResult currencyId(int id)
        {

            var entity = currencyShopDb.Currency.Find(id);
                currencyShopDb.Currency.Remove(entity);
          

            currencyShopDb.SaveChanges();
            return Ok("delete data successfully!");

        }
      

        /// <response code="200">delete data successfull</response>
        [HttpDelete]
        [MapToApiVersion("1")]
        [Authorize]
        [ActionName("price")]

        public IActionResult priceHistory([FromQuery]int id, int month)
        {
            if (month > 0)
            {
                currencyShopDb.RemoveRange(currencyShopDb.Prices.Where(p => (DateTime.Now - p.Updated).TotalDays > (month * 30)));
                currencyShopDb.SaveChanges();
                return Ok("delete data successfuly!");
            }
            else
            {
                var entity = currencyShopDb.Prices.Find(id);
                if (entity != null)
                {
                    currencyShopDb.Prices.Remove(entity);
                    currencyShopDb.SaveChanges();
                    return Ok("delete data successfuly!");
                }
                else { return NotFound("this id invalid"); }
                
            }
        }
     
        /// <response code="200">Change successfull</response>
        /// <response code="404">this currency dose not exist!</response>
        /// <response code="401">No image has been uploaded!</response>
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [ActionName("change")]
      
        [Authorize]
        public IActionResult currencyChanges(int id, [FromBody] CurrencyModel model)
        {
            var entity = currencyShopDb.Currency.Find(id);
            if (entity != null)
            {
                if (model.ImgArray.Length > 0)
                {
                    var stream = new MemoryStream(model.ImgArray);
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.png";
                    var folder = "wwwroot/currency";
                    var fullPath = $"{folder}/{file}";
                    var imageFullPath = fullPath.Remove(0, 7);
                    var response = FileHelper.UploadPhoto(stream, folder, file);

                    if (!response)
                    {
                        return BadRequest("No image has been uploaded");
                    }
                    entity.ImgUrl = imageFullPath;
                }
                else
                {
                    entity.Name = model.Name;
                    entity.LastPrice = model.LastPrice;
                    entity.LastUpdated = DateTime.Now;
                    entity.Type = model.Type;
                    if (model.ImgInternetUrl != null)
                        entity.ImgInternetUrl = model.ImgInternetUrl;
                    currencyShopDb.SaveChanges();
                }
                return Ok("saved Change");

            }
            else
            {
                return NotFound("this brand dose not exist!");
            }
        }

    }

}
