using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IUserRepository
    {
        void Save(User user);
        void Update(User user);
        void Delete(User user);
    }
}
