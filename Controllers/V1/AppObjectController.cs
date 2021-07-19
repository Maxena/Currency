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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{

    [ApiVersion("1")]
    [Route("api/app/v{version:apiVersion}/objects/[action]")]
    [ApiController]
    public class AppObjectController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AppObjectController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }

        /// <response code="200">get Objects successfully</response>
        /// <response code="404">any Objects doesnt exist</response>
        /// <response code="400">request invalid</response>
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Objects>))]
        [ActionName("")]
        public IActionResult objects([FromQuery]bool all=false,int categoryId=0,string brandName="ساپیا",int startPrice=0,int lastPrice=0)
        {
            if (all)
            {
                if (startPrice > 0 && lastPrice <= 0)
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
                else if (startPrice > 0 && lastPrice > 0)
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.Price >= startPrice && m.Price <= lastPrice
                                  select new RObjects()
                                  {
                                      Id = m.Id,
                                      BrandName = m.BrandName,
                                      Name = m.Name,
                                      Price = m.Price,
                                      ProduceYear = m.ProduceYear,
                                      DatePosted = m.DatePosted,
                                      CategoryId = m.CategoryId,

                                  };

                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }
                else if(startPrice <= 0 && lastPrice >0)
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
                                      CategoryId = m.CategoryId,

                                  };

                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }
                  else{  var objects = from m in currencyShopDb.Objects
                                  select new RObjects()
                                  {
                                      Id = m.Id,
                                      BrandName = m.BrandName,
                                      Name = m.Name,
                                      Price = m.Price,
                                      ProduceYear = m.ProduceYear,
                                      DatePosted = m.DatePosted,
                                      CategoryId = m.CategoryId,

                                  };
                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }
            }
            if (brandName.Length > 0 && categoryId > 0)
            {

                if (startPrice > 0 && lastPrice <= 0)
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.CategoryId == categoryId
                                  where m.BrandName == brandName
                                  where m.Price >= startPrice
                                  select new RObjects()
                                  {
                                      Id = m.Id,
                                      BrandName = m.BrandName,
                                      Name = m.Name,
                                      Price = m.Price,
                                      ProduceYear = m.ProduceYear,
                                      DatePosted = m.DatePosted,
                                      CategoryId=m.CategoryId,

                                  };

                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }

                else if (startPrice <= 0 && lastPrice > 0)
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.CategoryId == categoryId
                                  where m.BrandName == brandName
                                  where m.Price <= lastPrice
                                  select new RObjects()
                                  {
                                      Id = m.Id,
                                      BrandName = m.BrandName,
                                      Name = m.Name,
                                      Price = m.Price,
                                      ProduceYear = m.ProduceYear,
                                      DatePosted = m.DatePosted,
                                      CategoryId = m.CategoryId,

                                  };

                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }
                else if ((startPrice > 0 && lastPrice > 0))
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.CategoryId == categoryId
                                  where m.BrandName == brandName
                                  where m.Price >= startPrice && m.Price <= lastPrice
                                  select new RObjects()
                                  {
                                      Id = m.Id,
                                      BrandName = m.BrandName,
                                      Name = m.Name,
                                      Price = m.Price,
                                      ProduceYear = m.ProduceYear,
                                      DatePosted = m.DatePosted,
                                      CategoryId = m.CategoryId,

                                  };

                    if (objects.Count() > 0)
                        return Ok(objects);
                    else
                        return NotFound("any objects doesnt exist");
                }
                else
                {
                    var objectsObj = from m in currencyShopDb.Objects
                                     where m.CategoryId == categoryId
                                     where m.BrandName == brandName
                                     select new RObjects()
                                     {
                                         Id = m.Id,
                                         BrandName = m.BrandName,
                                         Name = m.Name,
                                         Price = m.Price,
                                         ProduceYear = m.ProduceYear,
                                         DatePosted = m.DatePosted,
                                         CategoryId = m.CategoryId,

                                     };
                    if (objectsObj.Count() > 0)
                        return Ok(objectsObj);
                    else
                        return NotFound("any mobile doesnt exist");
                }
            }
            if (categoryId > 0)
            {
                var objectsObj = from m in currencyShopDb.Objects
                                 where m.CategoryId == categoryId
                                 select new RObjects()
                                 {

                                     Id = m.Id,
                                     BrandName = m.BrandName,
                                     Name = m.Name,
                                     Price = m.Price,
                                     ProduceYear = m.ProduceYear,
                                     DatePosted = m.DatePosted,
                                     CategoryId = m.CategoryId,
                                 };
                if (objectsObj.Count() > 0)
                    return Ok(objectsObj);
                else
                    return NotFound("any Objects doesnt exist");
            }
           
            
            return BadRequest("request invalid");
        }

   
      
       
        /// <response code="200">get object successfully</response>
        [HttpGet("{search}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Object>))]
        public IActionResult search(string search)
        {
            var objects = from v in currencyShopDb.Objects
                          where v.Name.Contains(search)
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

    }
}
