using System;

namespace DBAccessLibrary.Models
{
    public class DataUserModel
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}