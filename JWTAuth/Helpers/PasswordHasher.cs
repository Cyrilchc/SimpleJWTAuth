using System;
using System.Security.Cryptography;
using System.Security.Policy;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace JWTAuth.Helpers
{
    public static class PasswordHasher
    {
        /// <summary>
        /// Sécurise un mot de passe avec l'algorithme PBKDF2
        /// Source : https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0
        /// Renvoie le hash et le salt (Au cas ou on voudrait l'utiliser pour retrouver le même sel pour comparer un mot de passe hashé)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static HashedPasswordModel HashPassword(string password)
        {
            HashedPasswordModel hashedPasswordModel = new HashedPasswordModel(); 
            
            // Salt
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
                hashedPasswordModel.Salt = Convert.ToBase64String(salt);
            }
            
            // Hash password
            hashedPasswordModel.HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPasswordModel;
        }

        /// <summary>
        /// Hash un mot de passe avec un salt existant
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static HashedPasswordModel HashPasswordWithSalt(string password, string salt)
        {
            HashedPasswordModel hashedPasswordModel = new HashedPasswordModel();
            hashedPasswordModel.Salt = salt;
            hashedPasswordModel.HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(hashedPasswordModel.Salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashedPasswordModel;
        }
    }
}