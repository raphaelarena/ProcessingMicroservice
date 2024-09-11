using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IDividendInterestRepository
    {
        void Save(DividendInterest dividendinterest);
        void Update(DividendInterest dividendinterest);
        void Delete(DividendInterest dividendinterest);
    }
}
