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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/app/v{version:apiVersion}/category")]
    [ApiController]
    public class AppCategoryController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AppCategoryController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }
        /// <response code="200">Get category successfull</response>
        /// <response code="404">There is no category</response>
        // GET: api/<ValuesController>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        public IActionResult Get()
        {
            var entity = from c in currencyShopDb.Categories
                         select new RCategory()
                         {
                             Id = c.Id,
                             Brands = currencyShopDb.Brands.Where(b => c.Id == b.categoryId).Select(b => new RBrand()
                             {
                                 Id = b.Id,
                                 categoryId = b.categoryId,
                                 Name = b.Name
                             }).ToList(),
                             Type = c.Type,
                             ImgUrl = c.ImgUrl,
                             ImgInternetUrl = c.ImgInternetUrl,
                             Objects = currencyShopDb.Objects.Where(b => c.Id == b.CategoryId).Select(b => new RObjects()
                             {
                                 Id = b.Id,
                                 CategoryId = b.CategoryId,
                                 Name = b.Name
                             }).ToList(),


                         };
            if (entity.Count() > 0)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound("There is no data");
            }
        }
        /// <response code="200">Get category successfull</response>
        /// <response code="404">There is no category</response>
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Category))]
        public IActionResult Get(int id)
        {
            var entity = from c in currencyShopDb.Categories
                         where c.Id == id
                         select new RCategory()
                         {
                             Id = c.Id,
                             Brands = currencyShopDb.Brands.Where(b => c.Id == b.categoryId).Select(b => new RBrand()
                             {
                                 Id = b.Id,
                                 categoryId = b.categoryId,
                                 Name = b.Name
                             }).ToList(),
                             ImgUrl = c.ImgUrl,
                             ImgInternetUrl = c.ImgInternetUrl,
                             Type = c.Type,
                             Objects = currencyShopDb.Objects.Where(b => c.Id == b.CategoryId).Select(b => new RObjects()
                             {
                                 Id = b.Id,
                                 CategoryId = b.CategoryId,
                                 Name = b.Name
                             }).ToList(),


                         };
            if (entity.Count() > 0)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound("There is no data");
            }



        }
      
    }
}

