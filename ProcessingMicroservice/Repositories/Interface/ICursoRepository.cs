using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface ICursoRepository
    {
        void Save(Curso curso);
        void Update(Curso curso);
        void Delete(Curso curso);
    }
}
