using HouseSellingBot.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseSellingBot
{
    public class AppDBContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("");//TODO - insert
        }
    }
}
