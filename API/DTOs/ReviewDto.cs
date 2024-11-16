namespace API.DTOs
{
    public class ReviewDto
    {

        public string Body { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public int Stars { get; set; }

        public string ProductName { get; set; }

        public string UserName { get; set; }
    }
}
