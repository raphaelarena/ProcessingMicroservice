using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IPriceHistoryRepository
    {
        void Save(PriceHistory pricehistory);
        void Update(PriceHistory pricehistory);
        void Delete(PriceHistory pricehistory);
    }
}
