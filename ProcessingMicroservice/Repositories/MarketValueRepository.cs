using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class MarketValueRepository : IMarketValueRepository
    {
        private readonly string _connectionString;

        public MarketValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(MarketValue marketvalue)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO MarketValues VALUES (@AssetId, @ValueDate, @Price)";
            dbConnection.Execute(query, marketvalue);
        }

        public void Update(MarketValue marketvalue)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE MarketValues SET AssetId = @AssetId, ValueDate = @ValueDate, Price = @Price WHERE Id = @Id ";
            dbConnection.Query(query, marketvalue);
        }

        public void Delete(MarketValue marketvalue)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM MarketValues where Id = @Id";
            dbConnection.Execute(query, new { Id = marketvalue.Id });
        }
    }
}
