using System;

namespace JWTModels
{
    public class TokenModel
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public Guid RefreshToken { get; set; } = Guid.NewGuid();
    }
}
