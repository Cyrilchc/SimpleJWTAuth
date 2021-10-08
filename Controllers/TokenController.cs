using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost, AllowAnonymous]
        public IActionResult CreateToken([FromBody] LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {
            var claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:5001"),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:5002"),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:500etc..."),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
 
            // Audience simple
            // var token = new JwtSecurityToken(
            //     _config["Jwt:Issuer"],
            //     _config["Jwt:Issuer"],
            //     claims,
            //     expires: DateTime.Now.AddMinutes(30),
            //     signingCredentials: creds);

            // Audiences multiples : 
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/39#issuecomment-267233556
            // On ne met pas l'audience dans le contructeur (null ici) mais dans les claims avec la clé aud. (Voir les claims ci-dessus)
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private UserModel Authenticate(LoginModel login)
        {
            // Prévoire une meilleure méthode de vérification
            UserModel user = null;
            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com" };
            }

            return user;
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }
}