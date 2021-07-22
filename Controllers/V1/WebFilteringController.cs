using CurrencyShop.Data;
using CurrencyShop.Models;
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
    [ApiVersion("1")]
    [Route("api/web/v{version:apiVersion}/filter/[action]")]
    [ApiController]
    public class WebFilteringController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public WebFilteringController(CurrencyShopDb db)
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

                        List<Words> customerList = new List<Words>();

                        for (int i = 2; i <= totalRows; i++)
                        {
                            customerList.Add(new Words
                            {

                                Word = workSheet.Cells[i, 2].Value.ToString(),
                             

                            });
                        }

                        currencyShopDb.words.AddRange(customerList);
                        await currencyShopDb.SaveChangesAsync();
                    }
                }

            }
            return Ok("set data ok");
        }
        /// <response code="200">Words Added</response>
        [HttpPost]
        [Authorize]
        [MapToApiVersion("1")]
        public IActionResult word([FromBody] List<Words> words)
        {
            currencyShopDb.words.AddRange(words);
            currencyShopDb.SaveChanges();
            return Ok("Words Added");
        }

        [HttpGet]
        [MapToApiVersion("1")]
        public IQueryable<object> word()
        {
            var WordList = from w in currencyShopDb.words
                           select new RWords()
                           {
                               Id = w.Id,
                               Word = w.Word,

                           };
            return WordList;
        }

    }
}
