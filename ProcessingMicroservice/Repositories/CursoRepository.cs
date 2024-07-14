using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;
using Dapper;

namespace ProcessingMicroservice.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly string _connectionString;

        public CursoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Curso curso)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Cursos VALUES (@Nome, @Descricao, @DataInicio, @DataFim)";
            dbConnection.Execute(query, curso);
        }

        public void Update(Curso curso)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Cursos SET DESCRICAO = @Descricao, Data_inicio = @DataInicio, Data_fim = @DataFim, NOME = @Nome WHERE aluno_id = @AlunoId ";
            dbConnection.Query(query, curso);
        }

        public void Delete(Curso curso)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Cursos where curso_id = @Id";
            dbConnection.Execute(query, new { Id = curso.Id });
        }
    }
}
