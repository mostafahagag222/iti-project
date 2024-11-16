using Core.Entities;

namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsSold { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Weight { get; set; }
        public string Color { get; set; }
        public  string ProductType { get; set; }
        public  string ProductBrand { get; set; }
        public List<string> Pictures { get; set; }

    }
}
