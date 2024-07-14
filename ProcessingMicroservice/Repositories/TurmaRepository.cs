using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class TurmaRepository : ITurmaRepository
    {
        private readonly string _connectionString;

        public TurmaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Turma turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Turmas  VALUES (@CursoId, @Nome, @Horario, @Local)";
            dbConnection.Execute(query, turma);
        }

        public void Update(Turma turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Turmas SET NOME = @Nome, Horario = @Horario, Local = @Local WHERE turma_id = @TurmaId ";
            dbConnection.Query(query, turma);
        }

        public void Delete(Turma turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Turmas where turma_id = @Id";
            dbConnection.Execute(query, new { Id = turma.Id });
        }
    }
}
