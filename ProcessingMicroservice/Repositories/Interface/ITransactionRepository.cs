using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        void Update(Transaction transaction);
        void Delete(Transaction transaction);
    }
}
