using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;
using Dapper;

namespace ProcessingMicroservice.Repositories
{
    public class DividendInterestRepository : IDividendInterestRepository
    {
        private readonly string _connectionString;

        public DividendInterestRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(DividendInterest dividendinterest)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO DividendsInterests VALUES (@PortfolioId, @AssetId, @PaymentType, @Amount, @PaymentDate)";
            dbConnection.Execute(query, dividendinterest);
        }

        public void Update(DividendInterest dividendinterest)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE DividendsInterests SET PortfolioId = @PortfolioId, AssetId = @AssetId, PaymentType = @PaymentType, Amount = @Amount, PaymentDate = @PaymentDate WHERE Id = @Id";
            dbConnection.Query(query, dividendinterest);
        }

        public void Delete(DividendInterest dividendinterest)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM DividendsInterests where Id = @Id";
            dbConnection.Execute(query, new { Id = dividendinterest.Id });
        }
    }
}
