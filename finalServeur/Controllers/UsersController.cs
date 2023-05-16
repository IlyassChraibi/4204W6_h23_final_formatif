using finalServeur.DTOs;
using finalServeur.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace finalServeur.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UserManager<User> UserManager;
        public UsersController(UserManager<User> userManager)
        {
            this.UserManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO register)
        {
            if (register.Password != register.PasswordConfirm)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "♦♦ Les deux mots de passe spécifiés sont différents. ♦♦" });
            }
            User user = new User()
            {
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult identityResult = await this.UserManager.CreateAsync(user, register.Password);
            if (!identityResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "♦♦ Le username est déjà pris ou bien ton mot de passe est trop simple. On suggère le mot de passe Salut1! par exemple. ♦♦"});
            }
            return Ok(new { message = "Inscription réussie" });
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO login)
        {
            User user = await UserManager.FindByNameAsync(login.Username);
            if (user != null && await UserManager.CheckPasswordAsync(user, login.Password))
            {
                IList<string> roles = await UserManager.GetRolesAsync(user);
                List<Claim> authClaims = new List<Claim>();
                foreach (string role in roles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                authClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Pour avoir 100% dans le final, il suffit de"));
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "http://localhost:4200",
                    claims: authClaims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    validTo = token.ValidTo,
                    message = "Connexion réussie"
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                    new { message = "Le nom d'utilisateur ou le mot de passe est invalide." });
            }
        }
    }
}
