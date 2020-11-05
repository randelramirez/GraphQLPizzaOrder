using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQLPizzaOrder.Core.Constants;
using GraphQLPizzaOrder.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLPizzaOrder.API.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public UsersController(IConfiguration configuration, SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            // Check user exist in system or not
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return NotFound();
            }

            // Perform login operation
            var signInResult = await signInManager.CheckPasswordSignInAsync(user, model.Password, true);
            if (signInResult.Succeeded)
            {
                // Obtain token
                TokenModel token = await GetJwtSecurityTokenAsync(user);
                return Ok(token);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> CreateDefaultUsers()
        {
            #region Roles

            var rolesDetails = new List<string>
                {
                   Roles.Customer,
                   Roles.Restaurant,
                   Roles.Admin
                };

            foreach (string roleName in rolesDetails)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            #endregion

            #region Users

            var userDetails = new Dictionary<string, IdentityUser>{
                {
                    Roles.Customer,
                    new IdentityUser { Email = "customer@demo.com", UserName = "CustomerUser", EmailConfirmed = true }
                },
                {
                    Roles.Restaurant,
                    new IdentityUser { Email = "restaurant@demo.com", UserName = "RestaurantUser", EmailConfirmed = true }
                },
                {
                    Roles.Admin,
                    new IdentityUser { Email = "admin@demo.com", UserName = "AdminUser", EmailConfirmed = true }
                }
            };

            foreach (var details in userDetails)
            {
                var existingUserDetails = await userManager.FindByEmailAsync(details.Value.Email);
                if (existingUserDetails == null)
                {
                    await userManager.CreateAsync(details.Value);
                    await userManager.AddPasswordAsync(details.Value, "Password");
                    await userManager.AddToRoleAsync(details.Value, details.Key);
                }
            }

            #endregion

            return Ok("Default User has been created");
        }

      
        public async Task<IActionResult> ProtectedPage()
        {
            // Obtain MailId from token
            ClaimsIdentity identity = httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
            var userName = identity?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var user = await userManager.FindByNameAsync(userName);
            return Ok(user);
        }

        private async Task<TokenModel> GetJwtSecurityTokenAsync(IdentityUser user)
        {
            var keyInBytes = System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JwtIssuerOptions:SecretKey").Value);
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(keyInBytes), SecurityAlgorithms.HmacSha256);
            DateTime tokenExpireOn = DateTime.Now.AddDays(3);

            // Obtain Role of User
            IList<string> rolesOfUser = await userManager.GetRolesAsync(user);

            // Add new claims
            List<Claim> tokenClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, rolesOfUser.FirstOrDefault()),
                };

            // Make JWT token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration.GetSection("JwtIssuerOptions:Issuer").Value,
                audience: configuration.GetSection("JwtIssuerOptions:Audience").Value,
                claims: tokenClaims,
                expires: tokenExpireOn,
                signingCredentials: credentials
            );

            // Return it
            var tokenModel = new TokenModel
            {
                UserId = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpireOn = tokenExpireOn,
            };

            // Set current user details for busines & common library
            var currentUser = await userManager.FindByEmailAsync(user.Email);

            // Add new claim details
            var existingClaims = await userManager.GetClaimsAsync(currentUser);
            await userManager.RemoveClaimsAsync(currentUser, existingClaims);
            await userManager.AddClaimsAsync(currentUser, tokenClaims);

            return tokenModel;
        }
    }
}

