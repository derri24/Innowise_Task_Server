using System.Data.Entity;
using System.Data.SqlClient;
using Server.Entities;

namespace EntityTest.Properties
{
    class EntityContext : DbContext
    {
        private static string connectionString = "Server=DESKTOP-BQDFB84;Database=fridgesdb;Trusted_Connection=true";

        public EntityContext() : base(connectionString){
        }
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<Product> Products {get; set; }

        public static string  GetConnectionString()
        {
            return connectionString;
        }
   
    }

  
}