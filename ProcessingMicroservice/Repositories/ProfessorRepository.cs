using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class ProfessorRepository : IProfessorRepository
    {
        private readonly string _connectionString;

        public ProfessorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Professor professor)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Professoress  VALUES (@Nome, @Email, @Telefone, @Endereco, @UsuarioId, @DataContratacao)";
            dbConnection.Execute(query, professor);
        }

        public void Update(Professor professor)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Professoress SET ENDERECO = @Endereco, TELEFONE = @Telefone, EMAIL = @Email WHERE professor_id = @ProfessorId ";
            dbConnection.Query(query, professor);
        }

        public void Delete(Professor professor)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Professoress where professor_id = @Id";
            dbConnection.Execute(query, new { Id = professor.Id });
        }
    }
}
