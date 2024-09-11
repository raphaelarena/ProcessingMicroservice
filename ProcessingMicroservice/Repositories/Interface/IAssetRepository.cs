using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IAssetRepository
    {
        void Save(Asset aluno);
        void Update(Asset aluno);
        void Delete(Asset aluno);
    }
}
