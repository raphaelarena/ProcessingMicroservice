namespace ProcessingMicroservice.Entities
{
    public class Transaction : Base
    {
        public int PortfolioId { get; set; }
        public int AssetId { get; set; }
        public int TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
