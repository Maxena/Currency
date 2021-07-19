using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/app/v{version:apiVersion}/barnd/[action]")]
    [ApiController]
    public class AppBrandsController : ControllerBase
    {
        CurrencyShopDb currencyShopDb;
        public AppBrandsController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }
       
        /// <response code="200">Get brands successfull</response>
        /// <response code="404">There is no brand</response>
        [HttpGet("{categoryId}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Brand>))]

        public IActionResult brands(int categoryId)
        {
            var brandsObj = from b in currencyShopDb.Brands
                           where b.categoryId == categoryId
                           select new RBrand() 
                           {
                               Id = b.Id,
                               Name = b.Name,
                               categoryId = b.categoryId,

                               ImgUrl = b.ImgUrl,
                               ImgInternetUrl = b.ImgInternetUrl,

                           };
            if (brandsObj.Count() > 0)
                return Ok(brandsObj);
            else { return NotFound("There is no brand"); }
        
        }



       
    }
}
