using JWTData;
using JWTModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace JWTServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Context _context;
        private readonly JwtHelpers _jwtHelpers;

        public TokenController(IConfiguration config, Context context, JwtHelpers jwtHelpers)
        {
            _config = config;
            _context = context;
            _jwtHelpers = jwtHelpers;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> CreateToken([FromBody] SignInModel login)
        {
            UserModel user = _jwtHelpers.Authenticate(login, _context);
            if (user is null)
                return Unauthorized();

            var tokenString = _jwtHelpers.BuildToken(user, _config);
            TokenModel tokenModel = new TokenModel
            {
                AccessToken = tokenString,
                RefreshToken = Guid.NewGuid()
            };
            await _context.AccessTokens.AddAsync(tokenModel);
            await _context.SaveChangesAsync();
            return Ok(tokenModel);
        }

        /// <summary>
        /// How to refresh token automatically in a client app
        /// https://stackoverflow.com/a/40637094
        /// </summary>
        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            // Check that token + refreshToken exists
            TokenModel token = _context.AccessTokens.
                Where(x => x.AccessToken == tokenModel.AccessToken && x.RefreshToken == tokenModel.RefreshToken).FirstOrDefault();

            if (token is null)
                return BadRequest("Couple AccessToken et RefreshToken introuvable");

            // Get claims
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(tokenModel.AccessToken);

            // Removes the old token + refreshToken to prevent user to reuse it to ask for another refresh
            _context.AccessTokens.Remove(token);

            // Create new tokens 
            var tokenString = _jwtHelpers.BuildTokenWithClaims(jwtToken.Claims, _config);
            TokenModel newTokenModel = new TokenModel
            {
                AccessToken = tokenString,
                RefreshToken = Guid.NewGuid()
            };

            // Save it
            await _context.AccessTokens.AddAsync(newTokenModel);
            await _context.SaveChangesAsync();

            // Return it
            return Ok(newTokenModel);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] SignInModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Mail))
                return BadRequest("L'email n'est pas renseigné");
            if (string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Le mot de passe n'est pas renseigné");
            if (!_jwtHelpers.IsValidEmail(user.Mail))
                return BadRequest("L'email inséré est invalide");
            if (!_jwtHelpers.IsValidPassword(user.Password))
                return BadRequest("Le mot de passe ne respecte pas les critères de sécurité");

            if (_context.Users.Any(x => x.Email == user.Mail))
                return Conflict("Cette adresse mail est déjà utilisée");

            HashedPasswordModel hashedPasswordModel = PasswordHasher.HashPassword(user.Password);

            UserModel newUser = new UserModel
            {
                Email = user.Mail,
                Password = hashedPasswordModel.HashedPassword,
                Salt = hashedPasswordModel.Salt
            };

            await _context.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
