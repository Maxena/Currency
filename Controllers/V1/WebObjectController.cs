using CurrencyShop.Data;
using CurrencyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{

    [ApiVersion("1")]
    [Route("api/web/v{version:apiVersion}/objects/[action]")]
    [ApiController]
    public class WebObjectController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public WebObjectController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }
        /// <response code="200""send list successfully"</response>
        [HttpPost("list")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [Authorize]
        public async Task<IActionResult> objects(List<Objects> obj)
        {
            await currencyShopDb.Objects.AddRangeAsync(obj);
            currencyShopDb.SaveChanges();
            return Ok("send list successfully");
        }

        /// <response code="200""send object successfully"</response>
        [HttpPost("object")]
        [ActionName("object")]
        [Authorize]
        [MapToApiVersion("1")]


        public IActionResult oneObject(Objects obj)
        {
            currencyShopDb.Objects.Add(obj);
            currencyShopDb.SaveChanges();
            return Ok("send object successfully");
        }
        /// <response code="200">get mobile successfully</response>
        /// <response code="404">any mobile doesnt exist</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]

        public IActionResult mobile()
        {
            var mobiles = from m in currencyShopDb.Objects
                          where m.CategoryId == 1
                          select new RObjects()
                          {

                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,
                          };
            if (mobiles.Count() > 0)
                return Ok(mobiles);
            else
                return NotFound("any mobile doesnt exist");
        }
        /// <response code="200">get mobile successfully</response>
        /// <response code="404">any mobile doesnt exist</response>
        [HttpGet("{brandName}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult mobile(string brandName)
        {
            var mobiles = from m in currencyShopDb.Objects
                          where m.CategoryId == 1
                          where m.BrandName == brandName
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (mobiles.Count() > 0)
                return Ok(mobiles);
            else
                return NotFound("any mobile doesnt exist");
        }
        /// <response code="200">get vehicle successfully</response>
        /// <response code="404">any vehicle doesnt exist</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult vehicle()
        {

            var vehicle = from v in currencyShopDb.Objects
                          where v.CategoryId == 1
                          select new RObjects()
                          {
                              Id = v.Id,
                              BrandName = v.BrandName,
                              Name = v.Name,
                              Price = v.Price,
                              ProduceYear = v.ProduceYear,
                              DatePosted = v.DatePosted,

                          };
            if (vehicle.Count() > 0)
                return Ok(vehicle);
            else
                return NotFound("any mobile doesnt exist");
        }

        /// <response code="200">get vehicle successfully</response>
        /// <response code="404">any vehicle doesnt exist</response>
        [HttpGet("{brandName}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult vehicle(string brandName)
        {

            var vehicle = from v in currencyShopDb.Objects
                          where v.CategoryId == 1
                          where v.BrandName == brandName
                          select new RObjects()
                          {
                              Id = v.Id,
                              BrandName = v.BrandName,
                              Name = v.Name,
                              Price = v.Price,
                              ProduceYear = v.ProduceYear,
                              DatePosted = v.DatePosted,

                          };
            if (vehicle.Count() > 0)
                return Ok(vehicle);
            else
                return NotFound("any mobile doesnt exist");
        }
        /// <response code="200">get object successfully</response>

        [HttpGet]
        [MapToApiVersion("1")]
        [ActionName("object")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult objects()
        {
            return Ok(currencyShopDb.Objects);
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/{startPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult Filter(int startPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.Price >= startPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/last/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]

        public IActionResult priceLastFilter(int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.Price <= lastPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/{startOrice}/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult priceFilter(int startPrice, int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.Price >= startPrice && m.Price <= startPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/category/{categoryId}/{startPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult FilterCategory(int category, int startPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.CategoryId == category
                          where m.Price >= startPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }

        /// <response code="200">get object successfully</response>
        [HttpGet("filter/category/{categoryId}/last/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]

        public IActionResult priceLastCategoryFilter(int categoryId, int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.CategoryId == categoryId
                          where m.Price <= lastPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/category/{categoryId}/{startOrice}/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult priceCategoryFilter(int categoryId, int startPrice, int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.CategoryId == categoryId
                          where m.Price >= startPrice && m.Price <= startPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = m.BrandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }
        /// <response code="200">get object successfully</response>
        [HttpGet("filter/category/{categoryId}/{brandId}/last/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]

        public IActionResult priceLastCategoryBrandFilter(int categoryId, string brandName, int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.CategoryId == categoryId
                          where m.BrandName == brandName
                          where m.Price <= lastPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = brandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }

        /// <response code="200">get object successfully</response>
        [HttpGet("filter/category/{categoryId}/{brandId}/{startOrice}/{lastPrice}")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult priceCategoryBrandFilter(int categoryId, string brandName, int startPrice, int lastPrice)
        {
            var objects = from m in currencyShopDb.Objects
                          where m.CategoryId == categoryId
                          where m.BrandName == brandName
                          where m.Price >= startPrice && m.Price <= startPrice
                          select new RObjects()
                          {
                              Id = m.Id,
                              BrandName = brandName,
                              Name = m.Name,
                              Price = m.Price,
                              ProduceYear = m.ProduceYear,
                              DatePosted = m.DatePosted,

                          };
            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }

        /// <response code="200">get object successfully</response>
        [HttpGet("[action]/{search}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult search(string search)
        {
            var objects = from v in currencyShopDb.Objects
                          where v.Name.StartsWith(search)
                          select new RObjects()
                          {
                              Id = v.Id,
                              BrandName = v.BrandName,
                              Name = v.Name,
                              Price = v.Price,
                              ProduceYear = v.ProduceYear,
                              DatePosted = v.DatePosted,

                          };

            if (objects.Count() > 0)
                return Ok(objects);
            else
                return NotFound("any objects doesnt exist");
        }

        /// <response code="200">saved change</response>
        [HttpPut("list")]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult changeObject([FromBody] List<Objects> objects)
        {
            foreach (var obj in objects)
            {
                var entity = currencyShopDb.Objects.Find(obj.Id);
                entity.Price = obj.Price;
                entity.Name = obj.Name;
                entity.BrandName = obj.BrandName;
                obj.CategoryId = obj.CategoryId;
                currencyShopDb.SaveChanges();

            }
            return Ok("saved Change");
        }
        [HttpPut]
        [ActionName("object")]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult changeObject([FromBody] Objects obj)
        {
            var entity = currencyShopDb.Objects.Find(obj.Id);
            entity.Price = obj.Price;
            entity.Name = obj.Name;
            entity.BrandName = obj.BrandName;
            obj.CategoryId = obj.CategoryId;
            currencyShopDb.SaveChanges();


            return Ok("saved Change");
        }


        /// <response code="200">saved change</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [ActionName("object")]
        public IActionResult objects(int id)
        {
            var entity = currencyShopDb.Objects.Find(id);
            currencyShopDb.Objects.Remove(entity);
            currencyShopDb.SaveChanges();
            return Ok("saved change");
        }
    }
}
