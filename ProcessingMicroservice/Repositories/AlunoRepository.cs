using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly string _connectionString;

        public AlunoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Aluno aluno)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Alunoss  VALUES (@Nome, @DataNascimento, @Email, @Telefone, @Endereco, @UsuarioId)";
            dbConnection.Execute(query, aluno);
        }

        public void Update(Aluno aluno)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Alunoss SET ENDERECO = @Endereco, TELEFONE = @Telefone, EMAIL = @Email WHERE aluno_id = @AlunoId ";
            dbConnection.Query(query, aluno);
        }

        public void Delete(Aluno aluno)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Alunoss where Aluno_id = @Id";
            dbConnection.Execute(query, new { Id = aluno.Id });
        }
    }
}
