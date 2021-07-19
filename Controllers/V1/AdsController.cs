using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using CurrencyShop.requestModel;
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
    [Route("api/v{version:apiVersion}/ads")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AdsController(CurrencyShopDb db)
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
            var entity = from a in currencyShopDb.Ads select new RAds() { Id = a.Id,
            AdsUrl=a.AdsUrl,ImageUrl=a.ImageUrl};
            if (entity.Count()>0)
                return Ok(entity);
            else
                return BadRequest("No ads found");
        }
       /// <response code="200">Get Admob Token</response>
        /// <response code="400">image does not uploded</response>
        /// <response code="201">Ads change successfull </response>
        // PUT api/<AdsController>/5
        [HttpPut]
        [MapToApiVersion("1.0")]
        [Authorize]
        public IActionResult put([FromBody]AdsModel ads)
        {
          
            var stream = new MemoryStream(ads.imageArray);
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

                var entity = currencyShopDb.Ads.First();
                if (entity != null)
                {
                    entity.ImageUrl = imageFullPath;
                    entity.AdsUrl = ads.url;
                    currencyShopDb.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
                else
                {
                    var adsObj = new Ads()
                    {
                        ImageUrl = imageFullPath,
                        AdsUrl = ads.url
                    };
                    currencyShopDb.Ads.Add(adsObj);
                    currencyShopDb.SaveChanges();
                    return StatusCode(StatusCodes.Status201Created);
                }
            }
        }

      
    }
}
