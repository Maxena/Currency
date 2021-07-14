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
        [HttpPost]
        [Authorize]
        [MapToApiVersion("1")]
     
        public IActionResult Post(List<Category>categories) {
            if (categories.Count > 0)
            {
                currencyShopDb.Categories.AddRange(categories.Distinct().ToList());
                return Ok(currencyShopDb.Categories);
            }
            else { return NotFound("There is no brand"); }

        }
        /// <response code="200">Image Uploaded</response>
        /// <response code="404">There is no category</response>
        /// <response code="401">image does not uploded</response>

        [HttpPost("{id}")]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult imgUrl(int id, [FromBody]  byte[] imageArray)
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
            var entity = currencyShopDb.Categories.Find(id);
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
            var entity = currencyShopDb.Categories.Find(id);
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
                             Brands =currencyShopDb.Brands.Where(b=>c.Id==b.categoryId).Select(b=>new RBrand() { Id = b.Id,categoryId=b.categoryId,
                            Name=b.Name
                            }).ToList(),
                            Type=c.Type,
                             ImgUrl = c.ImgUrl,
                             ImgInternetUrl = c.ImgInternetUrl,
                             Objects = currencyShopDb.Objects.Where(b => c.Id == b.CategoryId).Select(b => new RObjects()
                            {
                                 Id = b.Id,
                                CategoryId = b.CategoryId,
                                Name = b.Name
                            }).ToList(),


                         };
            if (entity.Count()>0)
            {
                return Ok(entity);
            }
            else {
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
                         where c.Id==id
                         select new RCategory()
                         {
                             Id = c.Id,
                             Brands = currencyShopDb.Brands.Where(b => c.Id == b.categoryId).Select(b => new RBrand()
                             {
                                 Id = b.Id,
                                 categoryId = b.categoryId,
                                 Name = b.Name
                             }).ToList(),
                             ImgUrl=c.ImgUrl,
                             ImgInternetUrl=c.ImgInternetUrl,
                             Type = c.Type ,
                             Objects = currencyShopDb.Objects.Where(b => c.Id == b.CategoryId).Select(b => new RObjects()
                             {
                                 Id = b.Id,
                                 CategoryId = b.CategoryId,
                                 Name = b.Name
                             }).ToList(),


                         };
            if (entity.Count()>0)
            {
                return Ok(entity);
            }
            else
            {
                return NotFound("There is no data");
            }
      


        }
        /// <response code="200">Change successfull</response>
        /// <response code="404">this Category dose not exist!</response>
        /// <response code="401">No image has been uploaded!</response>
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [ActionName("imgUrl")]

        [Authorize]
        public IActionResult imageUrl(int id, [FromBody] byte[] imageUrl)
        {

            var stream = new MemoryStream(imageUrl);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot/AdsImage";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("No image has been uploaded");
            }
            var entity = currencyShopDb.Categories.Find(id);
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
        /// <response code="404">this Category dose not exist!</response>
        [HttpPatch("internet/{id}")]
        [ActionName("imgUrl")]
        [MapToApiVersion("1")]

        [Authorize]
        public IActionResult imageurlInternet(int id, [FromBody] string imageUrl)
        {


            var entity = currencyShopDb.Categories.Find(id);
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

