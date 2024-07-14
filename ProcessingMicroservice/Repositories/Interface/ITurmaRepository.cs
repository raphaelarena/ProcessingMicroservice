using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface ITurmaRepository
    {
        void Save(Turma turma);
        void Update(Turma turma);
        void Delete(Turma turma);
    }
}
