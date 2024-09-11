using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class PriceHistoryRepository : IPriceHistoryRepository
    {
        private readonly string _connectionString;

        public PriceHistoryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(PriceHistory turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO PriceHistories VALUES (@AssetId, @QuoteDate, @OpeningPrice, @ClosingPrice, @Low, @High, @Volume)";
            dbConnection.Execute(query, turma);
        }

        public void Update(PriceHistory turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE PriceHistories SET AssetId = @AssetId, QuoteDate = @QuoteDate, OpeningPrice = @OpeningPrice, ClosingPrice = @ClosingPrice, Low = @Low, High = @High, Volume = @Volume WHERE Id = @Id ";
            dbConnection.Query(query, turma);
        }

        public void Delete(PriceHistory turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM PriceHistories where Id = @Id";
            dbConnection.Execute(query, new { Id = turma.Id });
        }
    }
}
