namespace API.DTOs
{
    public class UserDto
    {

        public string UserId { get; set; }

        public string username { get; set; }

        public string profilePicture { get; set; }

        public string email { get; set; }

        public string mobileNumber { get; set; }

        public string token { get; set; }

        public List<string> roles { get; set; } = new List<string>() { "Customer"};

    }
}
