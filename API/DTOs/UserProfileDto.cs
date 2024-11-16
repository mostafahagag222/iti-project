using Core.Entities.Enum;

namespace API.DTOs
{
    public class UserProfileDto
    {
        public string id { get; set; }

        public string userName { get; set; }

        public string email { get; set; }

        public string phoneNumber { get; set; }

        public string profilePicture { get; set; }

        public Gender gender { get; set; }
    }
}
