using MongoDB.Driver;
using Vidly.Models;

namespace Vidly.mongoDB
{
    public class DbContext : IDbContext
    {
        public IMongoDatabase Db { get; set; }
        
        public DbContext()
        {
            var client = new MongoClient();
            Db = client.GetDatabase("Vidly");
        }

        public IMongoCollection<Movie> Movies => Db.GetCollection<Movie>("Movies");

        public IMongoCollection<Customer> Customers => Db.GetCollection<Customer>("Customers");

        public IMongoCollection<MembershipType> MembershipTypes => Db.GetCollection<MembershipType>("MembershipTypes");

        public IMongoCollection<Genre> Genres => Db.GetCollection<Genre>("Genres");

        public IMongoCollection<Rental> Rentals => Db.GetCollection<Rental>("Rentals");
    }

    public interface IDbContext
    {
        IMongoCollection<Movie> Movies { get; }
        IMongoCollection<Customer> Customers { get; }
        IMongoCollection<MembershipType> MembershipTypes { get; }
        IMongoCollection<Genre> Genres { get; }
        IMongoCollection<Rental> Rentals { get; }
    }
}
