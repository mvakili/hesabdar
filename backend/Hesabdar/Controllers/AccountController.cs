using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Hesabdar.Models.Enums;
using Hesabdar.Models;

namespace Hesabdar.Controllers
{
    public class RegisterInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginInput
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminRegisterInput : RegisterInput
    {

    }

    ///
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> userManager;
        readonly SignInManager<IdentityUser> signInManager;
        readonly RoleManager<Role> roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

        }


        [HttpPost("register/Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterInput credentials)
        {
            var user = new IdentityUser { UserName = credentials.Email, Email = credentials.Email };

            var result = await userManager.CreateAsync(user, credentials.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var roleName = await CreateOrGetRole(RoleType.Admin);

            await userManager.AddToRoleAsync(user, roleName);
            await signInManager.SignInAsync(user, isPersistent: false);

            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInput credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Username, credentials.Password, false, false);

            if (!result.Succeeded)
                return BadRequest();

            var user = await userManager.FindByNameAsync(credentials.Username);

            return Ok(CreateToken(user));
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task SignOut()
        {
            await signInManager.SignOutAsync();
        }

        string CreateToken(IdentityUser user)
        {
            var claims = new Claim[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
           };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HesabdarSigningKeyxy"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private async Task<string> CreateOrGetRole(RoleType roleType)
        {
            bool studentRole = await roleManager.RoleExistsAsync(roleType.ToString());
            if (!studentRole)
            {
                var role = new Role
                {
                    Name = roleType.ToString(),
                    Type = roleType
                };
                await roleManager.CreateAsync(role);
            }
            return roleType.ToString();
        }
    }
}