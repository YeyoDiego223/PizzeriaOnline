using Microsoft.EntityFrameworkCore;
using PizzeriaOnline.Models;
using System.Collections.Generic;
namespace PizzeriaOnline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingredientes> Ingredientes { get; set; }
    }

}
    
