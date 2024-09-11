using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly string _connectionString;

        public PortfolioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Portfolio portfolio)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Portfolio VALUES (@Name, @UserId)";
            dbConnection.Execute(query, portfolio);
        }

        public void Update(Portfolio portfolio)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Portfolio SET Name = @Name, UserId = @UserId WHERE Id = @Id ";
            dbConnection.Query(query, portfolio);
        }

        public void Delete(Portfolio portfolio)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Portfolio where Id = @Id";
            dbConnection.Execute(query, new { Id = portfolio.Id });
        }
    }
}
