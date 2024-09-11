using Dapper;
using Microsoft.Extensions.Configuration;
using ProcessingMicroservice.Entities;
using ProcessingMicroservice.Repositories.Interface;
using System.Data.SqlClient;

namespace ProcessingMicroservice.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Save(User user)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "INSERT INTO User VALUES (@Name, @Email, @PasswordHash)  ";
            dbConnection.Execute(query, user);
        }

        public void Update(User user)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "UPDATE User SET Name = @Name, Email = @Email, PasswordHash = @PasswordHash WHERE Id = @Id ";
            dbConnection.Query(query, user);
        }

        public void Delete(User user)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            var query = "DELETE FROM User where Id = @Id";
            dbConnection.Execute(query, new { Id = user.Id });
        }
    }
}
