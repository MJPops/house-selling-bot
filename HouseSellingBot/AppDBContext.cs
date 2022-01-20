using HouseSellingBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseSellingBot
{
    public class AppDBContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<User> Users { get; set; }

        //public AppDBContext() : base()
        //{

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HousesBase;Trusted_Connection=True;");
        }
    }
}
