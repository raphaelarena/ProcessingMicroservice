namespace ProcessingMicroservice.Entities
{
    public class MarketValue : Base
    {
        public int AssetId { get; set; }
        public DateTime ValueDate { get; set; }
        public decimal Price { get; set; }
    }
}
