using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IPortfolioRepository
    {
        void Save(Portfolio portfolio);
        void Update(Portfolio portfolio);
        void Delete(Portfolio portfolio);
    }
}
