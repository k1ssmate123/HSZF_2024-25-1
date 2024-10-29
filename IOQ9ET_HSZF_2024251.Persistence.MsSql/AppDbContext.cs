using IOQ9ET_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{


    public class AppDbContext : DbContext
    {
        [JsonProperty("actors")]
      
        public DbSet<Actor> Actors { get; set; }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=persondb;Integrated Security=True;MultipleActiveResultSets=true";

            optionsBuilder.UseSqlServer(connStr);

            base.OnConfiguring(optionsBuilder);

        }
    }
}
