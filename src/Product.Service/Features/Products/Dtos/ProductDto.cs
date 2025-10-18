using System.Text.Json.Serialization;

namespace Product.Service.Features.Products.Dtos
{
    public class ProductDto
    {
        [JsonPropertyName("id")]
        public String Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("stockquantity")]
        public int StockQuantity { get; set; }


        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

}
