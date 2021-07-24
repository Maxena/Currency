using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using CurrencyShop.recievedModel;
using CurrencyShop.requestModel;
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
    [Route("api/web/v{version:apiVersion}/category")]
    [ApiController]
    public class WebCategoryController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public WebCategoryController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }
        /// <response code="200">Get category successfull</response>
        /// <response code="404">There is no category</response>
        [HttpPost]
        [Authorize]
        [MapToApiVersion("1")]

        public IActionResult Post(List<CategoryModel> categories)
        {
            if (categories.Count > 0)
            {
                foreach (var item in categories)
                {
                    if (item.ImageArray.Length > 0)
                    {
                        var stream = new MemoryStream(item.ImageArray);
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.png";
                        var folder = "wwwroot/category";
                        var fullPath = $"{folder}/{file}";
                        var imageFullPath = fullPath.Remove(0, 7);
                        var response = FileHelper.UploadPhoto(stream, folder, file);

                        if (!response)
                        {
                            return BadRequest("image does not uploded");
                        }
                        Category category = new Category();
                        category.ImgUrl = imageFullPath;
                        if (item.ImgInternetUrl != null)
                            category.ImgInternetUrl = item.ImgInternetUrl;
                        category.Type = item.Type;
                        currencyShopDb.Categories.Add(category);
                        currencyShopDb.SaveChanges();
                    }
                    else {
                        Category category = new Category();
                       
                        if (item.ImgInternetUrl != null)
                            category.ImgInternetUrl = item.ImgInternetUrl;
                        category.Type = item.Type;
                        currencyShopDb.Categories.Add(category);
                        currencyShopDb.SaveChanges();
                    }
                   
                }
                
                return Ok(currencyShopDb.Categories);
            }
            else { return NotFound("There is no brand"); }

        }

        [HttpGet("type")]
       
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        public IActionResult Type()
        {
            var entity = from c in currencyShopDb.Categories select new RType() { Id = c.Id, Type = c.Type };
            if (entity.Count() > 0)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound("There is no data");
            }
        }


        [HttpGet]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Category>))]
        public IActionResult Get([FromQuery] int id)
        {
            if (id > 0)
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
            else
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

        }


        /// <response code="200">Change successfull</response>
        /// <response code="404">this Category dose not exist!</response>
        /// <response code="401">No image has been uploaded!</response>
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [ActionName("imgUrl")]

        [Authorize]
        public IActionResult category(int id, [FromBody]CategoryModel model)
        {
            var entity = currencyShopDb.Categories.Find(id);
            if (entity != null)
            {
                if (model.ImageArray.Length > 0)
                {
                    var stream = new MemoryStream(model.ImageArray);
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.png";
                    var folder = "wwwroot/category";
                    var fullPath = $"{folder}/{file}";
                    var imageFullPath = fullPath.Remove(0, 7);
                    var response = FileHelper.UploadPhoto(stream, folder, file);

                    if (!response)
                    {
                        return BadRequest("No image has been uploaded");
                    }
                    entity.ImgUrl = imageFullPath;
                 
                }
                if (model.ImgInternetUrl != null)
                    entity.ImgInternetUrl = model.ImgInternetUrl;
                entity.Type = model.Type;
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

