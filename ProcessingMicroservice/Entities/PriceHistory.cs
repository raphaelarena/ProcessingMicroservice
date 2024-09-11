namespace ProcessingMicroservice.Entities
{
    public class PriceHistory : Base
    {
        public int AssetId { get; set; }
        public DateTime QuoteDate { get; set; }
        public decimal OpeningPrice { get; set; }
        public decimal ClosingPrice { get; set; }
        public decimal Low { get; set; }
        public decimal High { get; set; }
        public decimal Volume { get; set; }
    }
}
