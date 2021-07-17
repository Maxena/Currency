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
    [Route("api/web/v{version:apiVersion}/barnd/[action]")]
    [ApiController]
    public class WebBrandsController : ControllerBase
    {
        CurrencyShopDb currencyShopDb;
        public WebBrandsController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;
        }
        /// <response code="200">Get brands successfull</response>
        /// <response code="404">There is no brand</response>
        [HttpPost]
        [Authorize]
        [MapToApiVersion("1")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Brand>))]

        public IActionResult brands(List<Brand> brands)
        {
            if (brands.Count > 0)
            {
                currencyShopDb.Brands.AddRange(brands.Distinct().ToList());
                return Ok(currencyShopDb.Brands);
            }
            else { return NotFound("There is no brand"); }

        }
        /// <response code="200">Image Uploaded</response>
        /// <response code="404">There is no brand</response>
        /// <response code="401">image does not uploded</response>

        [HttpPost("{id}")]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult imgUrl(int id, [FromBody] byte[] imageArray)
        {


            var stream = new MemoryStream(imageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.png";
            var folder = "wwwroot/brand";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("image does not uploded");
            }
            var entity = currencyShopDb.Brands.Find(id);
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
            var entity = currencyShopDb.Brands.Find(id);
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



        /// <response code="200">Change brand successfull</response>
        /// <response code="404">this brand dose not exist!</response>
        [HttpPatch("{id}/{name}")]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult brands(int id, string name)
        {
            var entity = currencyShopDb.Brands.Find(id);
            if (entity != null)
            {
                entity.Name = name;
                currencyShopDb.SaveChanges();
                return Ok("Change successfull");
            }
            else
            {
                return NotFound("this brand dose not exist!");
            }
        }
        /// <response code="200">Change successfull</response>
        /// <response code="404">this brand dose not exist!</response>
        /// <response code="401">No image has been uploaded!</response>
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [ActionName("imgUrl")]

        [Authorize]
        public IActionResult imageUrl(int id, [FromBody] byte[] imageUrl)
        {

            var stream = new MemoryStream(imageUrl);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.png";
            var folder = "wwwroot/brand";
            var fullPath = $"{folder}/{file}";
            var imageFullPath = fullPath.Remove(0, 7);
            var response = FileHelper.UploadPhoto(stream, folder, file);

            if (!response)
            {
                return BadRequest("No image has been uploaded");
            }
            var entity = currencyShopDb.Brands.Find(id);
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
        /// <response code="404">this brand dose not exist!</response>
        [HttpPatch("internet/{id}")]
        [ActionName("imgUrl")]
        [MapToApiVersion("1")]

        [Authorize]
        public IActionResult imageurlInternet(int id, [FromBody] string imageUrl)
        {


            var entity = currencyShopDb.Brands.Find(id);
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
