using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Product.Service.Features.Products.Dtos
{
    public class ProductCreationDto
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Price must be a positive number")]

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockquantity")]
        public int StockQuantity { get; set; }

    }

   
}
