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
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Objects>))]

        public IActionResult type(int id)
        {
            var mobiles = from m in currencyShopDb.Objects
                          where m.CategoryId == id
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
        [HttpGet("{categoryId}/{brandName}")]
        [AllowAnonymous]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Objects>))]
        public IActionResult type(int categoryId, string brandName)
        {
            var mobiles = from m in currencyShopDb.Objects
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

                          };
            if (mobiles.Count() > 0)
                return Ok(mobiles);
            else
                return NotFound("any mobile doesnt exist");
        }

        [HttpGet]
        [MapToApiVersion("1")]
        [ActionName("object")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Objects>))]
        public IActionResult objects()
        {

            var objects = from m in currencyShopDb.Objects
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
        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Objects>))]
        public IActionResult Filter(FilterModel model)
        {
            if (model.CategoryId <= 0)
            {
                if (model.StartPrice > 0 && model.LastPrice <= 0)
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.Price >= model.StartPrice
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
                if (model.StartPrice > 0 && model.LastPrice > 0)
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.Price >= model.StartPrice && m.Price <= model.LastPrice
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
                else
                {
                    var objects = from m in currencyShopDb.Objects
                                  where m.Price <= model.LastPrice
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
            }
            else
            {
                if (model.BrandName != null)
                {
                    if (model.StartPrice > 0 && model.LastPrice <= 0)
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.BrandName == model.BrandName
                                      where m.Price >= model.StartPrice
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

                    else if (model.StartPrice <= 0 && model.LastPrice > 0)
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.BrandName == model.BrandName
                                      where m.Price <= model.LastPrice
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
                    else
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.BrandName == model.BrandName
                                      where m.Price >= model.StartPrice && m.Price <= model.LastPrice
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
                }

                else
                {
                    if (model.StartPrice > 0 && model.LastPrice <= 0)
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.Price >= model.StartPrice
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

                    else if (model.StartPrice <= 0 && model.LastPrice > 0)
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.Price <= model.LastPrice
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
                    else
                    {
                        var objects = from m in currencyShopDb.Objects
                                      where m.CategoryId == model.CategoryId
                                      where m.Price >= model.StartPrice && m.Price <= model.LastPrice
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
                }

            }


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
