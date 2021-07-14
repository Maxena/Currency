using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using ImageUploader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/web/v{version:apiVersion}/ads")]
    [ApiController]
    public class WebAppAdsController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public WebAppAdsController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;

        }
        /// <response code="200">Get Ads</response>
        /// <response code="400">No ads found</response>
        // GET: api/<AddmobController>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ads))]
        public IActionResult Get()
        {
            var entity = from a in currencyShopDb.Ads
                         select new RAds()
                         {
                             Id = a.Id,
                             AdsUrl = a.AdsUrl,
                             ImageUrl = a.ImageUrl
                         };
            if (entity.Count() > 0)
                return Ok(entity);
            else
                return BadRequest("No ads found");
        }
        /// <response code="200">Get Ads by Id</response>
        /// <response code="400">No ads found</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ads))]
        public IActionResult Get(int id)
        {
            var entity = from a in currencyShopDb.Ads
                         where a.Id == id
                         select new RAds()
                         {
                             Id = a.Id,
                             AdsUrl = a.AdsUrl,
                             ImageUrl = a.ImageUrl
                         };
            if (entity != null)
            {
                return Ok(entity);
            }
            else
            {
                return BadRequest("No ads found");
            }
        }


        /// <response code="200">Get Admob Token</response>
        /// <response code="400">image does not uploded</response>
        /// <response code="201">Ads change successfull </response>
        // PUT api/<AdsController>/5
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize]
        public IActionResult Put(int id, [FromQueryAttribute] byte[] imageArray, string url)
        {

            var stream = new MemoryStream(imageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/AdsImage";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("image does not uploded");
            }
            else
            {

                var entity = currencyShopDb.Ads.Find(id);
                if (entity != null)
                {
                    entity.ImageUrl = imageFullPath;
                    entity.AdsUrl = url;
                    currencyShopDb.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    var ads = new Ads()
                    {
                        ImageUrl = imageFullPath,
                        AdsUrl = url
                    };
                    currencyShopDb.Ads.Add(ads);
                    currencyShopDb.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
        }

        // DELETE api/<AdsController>/5
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize]



        /// <response code="200">delete successfull</response>
        /// <response code="400">this item does not exist</response>

        public IActionResult Delete(int id)
        {
            var entity = currencyShopDb.Ads.Find(id);
            if (entity != null)
            {
                currencyShopDb.Ads.Remove(entity);
                return Ok("delete successfull");
            }
            else return BadRequest("this item does not exist");
        }
    }
}
