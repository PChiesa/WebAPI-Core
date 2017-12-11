using System;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Database
{
    public class WebApiContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }


    }

}