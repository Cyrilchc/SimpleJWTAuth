using JWTModels;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWTData
{
    public class PasswordHasher
    {
        public static HashedPasswordModel HashPassword(string password)
        {
            HashedPasswordModel hashedPasswordModel = new HashedPasswordModel();

            // Salt
            byte[] salt = new byte[128 / 8];
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetNonZeroBytes(salt);
            hashedPasswordModel.Salt = Convert.ToBase64String(salt);

            // Hash password
            hashedPasswordModel.HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPasswordModel;
        }

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
