using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly string _connectionString;

        public AssetRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(Asset asset)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO Assets VALUES (@Name, @Symbol, @Type, @Description)";
            dbConnection.Execute(query, asset);
        }

        public void Update(Asset asset)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE Assets SET Name = @Name, Type = @Type, Description = @Description WHERE Id = @Id ";
            dbConnection.Query(query, asset);
        }

        public void Delete(Asset asset)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Assets where Id = @Id";
            dbConnection.Execute(query, new { Id = asset.Id });
        }
    }
}
