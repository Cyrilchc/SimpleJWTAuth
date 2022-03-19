using System;
using System.Collections.Generic;
using System.Text;

namespace JWTModels
{
    public class HashedPasswordModel
    {
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }
}
