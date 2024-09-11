using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IMarketValueRepository
    {
        void Save(MarketValue marketvalue);
        void Update(MarketValue marketvalue);
        void Delete(MarketValue marketvalue);
    }
}
