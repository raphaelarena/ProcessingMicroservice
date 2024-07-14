using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IMatriculaRepository
    {
        void Save(Matricula matricula);
        void Update(Matricula matricula);
        void Delete(Matricula matricula);
    }
}
