using System.Data.Entity;
using Server.Entities;

namespace EntityTest.Properties
{
    class EntityContext : DbContext
    {
        public EntityContext():base("Server=DESKTOP-BQDFB84;Database=fridgesdb;Trusted_Connection=true")
        { }
        public DbSet<Fridge> Fridges { get; set; }
        public DbSet<Product> Products {get; set; }
   
    }

  
}