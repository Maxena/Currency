
    using CurrencyShop.Data;
    using CurrencyShop.Models;


using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyShop.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admob")]

    [ApiController]

    public class AdmobController : ControllerBase
    {
        private CurrencyShopDb currencyShopDb;
        public AdmobController(CurrencyShopDb db)
        {
            this.currencyShopDb = db;

        }
        /// <response code="200">Get Admob Token</response>
        /// <response code="404">Admob Token not found</response>
        // GET: api/<AddmobController>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddMobToken))]
        public IActionResult Get()
        {
            var entity = from a in currencyShopDb.AdMob select new RAddMobToken() { 
            Id=a.Id,
            Token=a.Token,
    
            };
            if (entity.Count() > 0)
                return Ok(entity);
            else return NotFound(" admob token does not exist");

        }





        /// <response code="200">Change Admob Token  successfully</response>
        /// <response code="201">create Admob Token  successfully</response>
        /// <response code="404">Admob Token not found</response>
        [HttpPatch]
        [MapToApiVersion("1.0")]
        [Authorize]
        public IActionResult Patch( [FromBody] AddMobToken value)
        {
            var entity = currencyShopDb.AdMob.First();
            if (entity != null)
            {
                entity.Token = value.Token;
                currencyShopDb.SaveChanges();
                return Ok("Change successfully");
            }
            else
            {
                currencyShopDb.AdMob.Add(new AddMobToken()
                {
                    Token = value.Token
                });
                currencyShopDb.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }
       
    }
}


