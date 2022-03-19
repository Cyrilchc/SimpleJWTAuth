using System;
using System.Collections.Generic;
using System.Text;

namespace JWTModels
{
    public class UserModel
    {
        public int Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
