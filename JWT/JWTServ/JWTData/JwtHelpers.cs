﻿using JWTModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace JWTData
{
    public class JwtHelpers
    {
        /// <summary>
        /// Build token with user infos
        /// </summary>
        public string BuildToken(UserModel user, IConfiguration config)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.GivenName, user.GivenName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.FamilyName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Aud, config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:5001"),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:5002"),
                new Claim(JwtRegisteredClaimNames.Aud, "http://localhost:500etc..."),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Multiple audiences : 
            // https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/39#issuecomment-267233556
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Build token with provided claims. Useful for refresh token
        /// </summary>
        public string BuildTokenWithClaims(IEnumerable<Claim> claims, IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Authenticates the provided user against the database
        /// </summary>
        public UserModel Authenticate(SignInModel login, Context context)
        {
            UserModel userModel = context.Users.Where(x => x.Email == login.Mail).FirstOrDefault();
            if (userModel != null)
                if (userModel.Password != PasswordHasher.HashPasswordWithSalt(login.Password, userModel.Salt).HashedPassword)
                    return null;
            
            return userModel;
        }

        /// <summary>
        /// Check if provided password meets requirements
        /// </summary>
        public bool IsValidPassword(string password)
        {
            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMinimum8Chars = new Regex(@".{8,}");
            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password);
        }

        /// <summary>
        /// Check if provided mail is valid
        /// </summary>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                    RegexOptions.None, TimeSpan.FromMilliseconds(200));

                string DomainMapper(Match match)
                {
                    var idn = new IdnMapping();
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
