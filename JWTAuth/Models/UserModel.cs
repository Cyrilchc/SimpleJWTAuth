using System;

namespace JWTAuth.Models
{
    public class UserModel
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}