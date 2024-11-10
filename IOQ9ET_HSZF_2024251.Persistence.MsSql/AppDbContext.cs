using IOQ9ET_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{


    public class AppDbContext : DbContext
    {


        public DbSet<Actor> Actors { get; set; }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Movie> Movies { get; set; }



        public AppDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            ReadJson();

        }
        public void ReadJson()
        {
            if (Actors.Any())
            {
                return;
            }
            string path = Directory.GetCurrentDirectory();
            path = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(path).FullName).FullName).FullName).FullName;
            path = Path.Combine(path, "movies.json");
            var rootObject = JsonConvert.DeserializeObject<JsonObject>(File.ReadAllText(path));

            foreach (var item in rootObject.Actors)
            {
                foreach (var chars in item.Character)
                {
                    foreach (var movies in chars.Movies)
                    {
                        if (!Movies.Any(x => x.Title == movies.Title))
                        {
                            Movies.Add(movies);
                        }
                    }
                    Characters.Add(new Character(chars.Name, chars.Alias, chars.Abilities));

                }
                Actors.Add(new Actor(item.Name, item.Age, item.Nationality));
                SaveChanges();
            }

            foreach (var item in rootObject.Actors)
            {
                foreach (var chars in item.Character)
                {

                    foreach (var movies in Movies)
                    {
                        if (chars.Movies.Any(x=>x.Title==movies.Title))
                        {
                            Characters.First(x => x.Name == chars.Name).Movies.Add(movies);
                        }
                    }

                }
                SaveChanges();
            }

            foreach (var item in rootObject.Actors)
            {
                foreach (var chars in Characters)
                {
                    if (item.Character.Any(x=>x.Name==chars.Name))
                    {
                        Actors.First(x => x.Name == item.Name).Character.Add(chars);
                    }
                }
                SaveChanges();

            }




        }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=persondb;Integrated Security=True;MultipleActiveResultSets=true";
            optionsBuilder.UseSqlServer(connStr);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
