using IOQ9ET_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{


    public class AppDbContext : DbContext
    {
        [JsonProperty("actors")]

        public DbSet<Actor> Actors { get; set; }
   
        public AppDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
           
        }

        public void ReadJson()
        {
            var rootObject = JsonConvert.DeserializeObject<JsonObject>(File.ReadAllText("movies.json"));
          
            foreach (var item in rootObject.Actors)
            {
                Actors.Add(item);
            }

            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=persondb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
