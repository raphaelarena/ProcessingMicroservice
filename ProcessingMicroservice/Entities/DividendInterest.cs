namespace ProcessingMicroservice.Entities
{
    public class DividendInterest : Base
    {
        public int PortfolioId { get; set; }
        public int AssetId { get; set; }
        public int PaymentType { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
