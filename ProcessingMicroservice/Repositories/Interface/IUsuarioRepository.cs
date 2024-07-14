using ProcessingMicroservice.Entities;

namespace ProcessingMicroservice.Repositories.Interface
{
    public interface IUsuarioRepository
    {
        void Save(Usuario usuario);
        void Update(Usuario usuario);
        void Delete(Usuario usuario);
    }
}
