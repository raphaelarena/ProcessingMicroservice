using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Transaction turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Transactions VALUES (@PortfolioId, @AssetId, @TransactionType, @Quantity, @UnitPrice, @TransactionDate)";
            dbConnection.Execute(query, turma);
        }

        public void Update(Transaction turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Transactions SET PortfolioId = @PortfolioId, AssetId = @AssetId, TransactionType = @TransactionType, Quantity = @Quantity, UnitPrice = @UnitPrice, TransactionDate = @TransactionDate WHERE Id = @Id ";
            dbConnection.Query(query, turma);
        }

        public void Delete(Transaction turma)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Transactions where Id = @Id";
            dbConnection.Execute(query, new { Id = turma.Id });
        }
    }
}
