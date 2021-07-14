using AuthenticationPlugin;
using CurrencyShop.Data;
using CurrencyShop.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace CurrencyShop.Controllers

{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/web/v{version:apiVersion}/account/[action]")]
    [ApiController]

    public class WebAppAccountController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        private CurrencyShopDb currencyShopDb;
        public WebAppAccountController(IConfiguration configuration, CurrencyShopDb db)
        {
            this.currencyShopDb = db;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        /// <response code="400">this Email Already Exist</response>
        /// <response code="201">create account successfully</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]

        public IActionResult register([FromBody] PannelUser pannelUser)
        {
            var userWithSameEmail = currencyShopDb.PannelUsers.Where(u => u.Email == pannelUser.Email).SingleOrDefault();
            if (userWithSameEmail != null)
            {

                return BadRequest("this Email Already Exist");
            }
            else
            {
                var useObj = new PannelUser()
                {
                    Name = pannelUser.Name,
                    Email = pannelUser.Email,
                    Password = SecurePasswordHasherHelper.Hash(pannelUser.Password)
                };
                currencyShopDb.PannelUsers.Add(useObj);
                currencyShopDb.SaveChanges();
                var claims = new[]
                 {
            new Claim(JwtRegisteredClaimNames.Email, pannelUser.Email),
             new Claim(ClaimTypes.Email,  pannelUser.Email),
              };
                var token = _auth.GenerateAccessToken(claims);
                var result = token.AccessToken;


                return StatusCode(StatusCodes.Status201Created, result);
            }



        }
        /// <response code="200">Get Token successfully</response>
        /// <response code="401">Invalid email or password</response>
        /// <response code="404">the user dosent exist</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]

        public IActionResult login([FromBody] LoginModel pannelUser)
        {
            var EmailUser = currencyShopDb.PannelUsers.FirstOrDefault(u => u.Email == pannelUser.email);
            if (EmailUser == null)
            {
                return NotFound("the user dose not exist");
            }
            if (!SecurePasswordHasherHelper.Verify(pannelUser.password, EmailUser.Password))
            {
                return Unauthorized("Invalid email or password");
            }
            var claims = new[]
             {
            new Claim(JwtRegisteredClaimNames.Email, pannelUser.email),
             new Claim(ClaimTypes.Email,  pannelUser.email),
              };
            var token = _auth.GenerateAccessToken(claims);

            return Ok(

                token.AccessToken
            );

        }

        /// <remarks>simple </remarks>
        /// <response code="200">Password has been changed!</response>
        /// <response code="401">Sorry cant change your password</response>
        /// <response code="404">Invald password</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize]

        public IActionResult changePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = currencyShopDb.PannelUsers.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound("Invald password");
            }
            if (!SecurePasswordHasherHelper.Verify(changePasswordModel.OldPassword, user.Password))
            {
                return Unauthorized("Sorry cant change your password");
            }
            user.Password = SecurePasswordHasherHelper.Hash(changePasswordModel.NewPassword);
            currencyShopDb.SaveChanges();
            return Ok("Password has been changed!");
        }

        /// <response code="200">profile has been changed!</response>
        /// <response code="404">Invald password</response>

        [HttpGet]
        [Authorize]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PannelUser))]
        public IActionResult userProfile()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var user = currencyShopDb.PannelUsers.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return NotFound();
            var responseResult = currencyShopDb.PannelUsers
                .Where(x => x.Email == user.Email)
                .SingleOrDefault();
            return Ok(new RPannelUser()
            {
                Id = responseResult.Id,
                Email = responseResult.Email,
                Name = responseResult.Name,
            }); ;
        }



    }
}

