using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly string _connectionString;

        public MatriculaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Matricula matricula)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Matriculas  VALUES (@MatriculaId, @AlunoId, @TurmaId, @DataMatricula)";
            dbConnection.Execute(query, matricula);
        }

        public void Update(Matricula matricula)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Matriculas SET aluno_id = @AlunoId, turma_id = @TurmaId, data_matricula = @DataMatricula WHERE matricula_id = @MatriculaId ";
            dbConnection.Query(query, matricula);
        }

        public void Delete(Matricula matricula)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Matriculas where turma_id = @Id";
            dbConnection.Execute(query, new { Id = matricula.Id });
        }
    }
}
