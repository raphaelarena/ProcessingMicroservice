using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IAlunoRepository
    {
        void Save(Aluno aluno);
        void Update(Aluno aluno);
        void Delete(Aluno aluno);
    }
}
