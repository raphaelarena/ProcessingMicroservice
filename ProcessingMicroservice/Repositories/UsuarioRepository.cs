using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Usuario usuario)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO USUARIO VALUES (@UserName, @Senha, @TipoUsuario, @Idreferente)  ";
            dbConnection.Execute(query, usuario);
        }

        public void Update(Usuario usuario)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE USUARIO SET USERNAME = @UserName WHERE USUARIO_ID = @UsuarioId ";
            dbConnection.Query(query, usuario);
        }

        public void Delete(Usuario usuario)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM USUARIO where usuario_id = @Id";
            dbConnection.Execute(query, new { Id = usuario.Id });
        }
    }
}
