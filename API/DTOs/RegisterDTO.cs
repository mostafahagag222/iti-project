using Core.Entities.Enum;

namespace JWTAuth.DTO
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }
}
