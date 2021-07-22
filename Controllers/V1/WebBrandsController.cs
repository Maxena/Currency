using CurrencyShop.Data;
using CurrencyShop.Helper;
using CurrencyShop.Models;
using CurrencyShop.requestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile postedFile)
        {
            if (postedFile == null || postedFile.Length == 0)
            {
                return RedirectToAction("ImportExcel");
            }

            //Get file
            var newfile = new FileInfo(postedFile.FileName);
            var fileExtension = newfile.Extension;

            //Check if file is an Excel File
            if (fileExtension.Contains(".xls"))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await postedFile.CopyToAsync(ms);

                    using (ExcelPackage package = new ExcelPackage(ms))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                        int totalRows = workSheet.Dimension.Rows;

                        List<Brand> customerList = new List<Brand>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            customerList.Add(new Brand
                            {

                                categoryId = Convert.ToInt16(workSheet.Cells[i, 2].Value),
                                Name = (workSheet.Cells[i, 3].Value).ToString(),
                                ImgInternetUrl = workSheet.Cells[i, 5].Value.ToString(),
                     

                            });
                        }

                        currencyShopDb.Brands.AddRange(customerList);
                        await currencyShopDb.SaveChangesAsync();
                    }
                }

            }
            return Ok("set data ok");
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
        [HttpPatch("{id}")]
        [MapToApiVersion("1")]
        [Authorize]
        public IActionResult brands(int id, [FromBody] BrandModel brandModel)
        {
            var entity = currencyShopDb.Brands.Find(id);
            if (entity != null)
            {
                if (brandModel.Name != null)
                {
                    entity.Name = brandModel.Name;
                }
                if (brandModel.ImageUrl != null)
                {
                    var stream = new MemoryStream(brandModel.ImageUrl);
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
                    entity.ImgUrl = imageFullPath;

                }
                if (brandModel.ImageInternetUrl != null)
                {
                    entity.ImgInternetUrl = brandModel.ImageInternetUrl;
                }
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
