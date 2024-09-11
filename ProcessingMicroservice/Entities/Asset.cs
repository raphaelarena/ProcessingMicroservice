namespace ProcessingMicroservice.Entities
{
    public class Asset : Base
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
    }
}
