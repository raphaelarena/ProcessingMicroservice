using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IProfessorRepository
    {
        void Save(Professor professor);
        void Update(Professor professor);
        void Delete(Professor professor);
    }
}
