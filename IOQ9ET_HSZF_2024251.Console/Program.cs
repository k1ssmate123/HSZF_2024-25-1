using ConsoleTools;
using IOQ9ET_HSZF_2024251.Application;
using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Xml.Serialization;

namespace IOQ9ET_HSZF_2024251
{

    internal class Program
    {
        static void Main(string[] args)
        {
            IActors actorService = Configure();

            var subMenu = new ConsoleMenu(args, level: 1)
             .Add("Show actors", () => ToConsole(actorService.ListByActor(), Console.WriteLine))
             .Add("Show characters", () => ToConsole(actorService.ListByCharacter(), Console.WriteLine))
             .Add("Show movies", () => ToConsole(actorService.ListByMovie(), Console.WriteLine))
             .Add("Back", ConsoleMenu.Close)
             .Configure(config =>
               {
                   config.Selector = ":3 ";
                   config.EnableFilter = true;
                   config.Title = "Kilistázás";
                   config.EnableBreadcrumb = true;
                   config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
               });




            var addMenu = new ConsoleMenu(args, level: 1)
            .Add("Add new actor", () => NewActor(actorService))
            .Add("Add new character", () => NewCharacter(actorService));

            var deleteMenu = new ConsoleMenu(args, level: 1)
            .Add("Delete actor", () => DeleteActor(actorService));




            var menu = new ConsoleMenu(args, level: 0)
            .Add("Show lists", subMenu.Show)
            .Add("Movies of Joe(s)", () => ToConsole(actorService.GetMoviesByDirector("Joe"), x => Console.WriteLine(x.Title + "\n\t" + x.Director)))
            .Add("Three highest grossing movie", () => ToConsole(actorService.ListByMovie().GroupBy(x => x.Title).Select(x => new { Title = x.Key, Grossing = x.Average(x => x.BoxOffice) }).OrderByDescending(x => x.Grossing).Take(3), x => Console.WriteLine("Title: " + x.Title + "\n\tGrossing: " + x.Grossing)))
            .Add("Actors and their characters", () => ToConsole(actorService.ListByActor(), x =>
            {
                Console.WriteLine(x.Name);
                foreach (var item in x.Character)
                {
                    Console.WriteLine("\t" + item.Name);
                }
            }))
            .Add("Movies and the actors", () => Console.WriteLine())
            .Add("Actors that played together the most", () => Console.WriteLine("Three"))
            .Add("Add new items", addMenu.Show)
            .Add("Delete items", deleteMenu.Show)
            .Add("Export movies to XML", () => ExportXml(actorService.ListByMovie()))
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = "+";
                config.EnableFilter = true;
                config.Title = "Marvel movies:";
                config.EnableWriteTitle = true;
                config.EnableBreadcrumb = true;
            });

            menu.Show();




        }

        static void NewActor(IActors update)
        {
            Console.Clear();
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());


            Console.Write("Nationality: ");
            string nationality = Console.ReadLine();

            update.AddActor(name, age, nationality);
            Console.WriteLine("Actor succesfully added!");
            Console.ReadKey();
        }

        static void DeleteActor(IActors operation)
        {
            var menu = new ConsoleMenu();
            HashSet<Actor> actors = operation.ListByActor();
            foreach (Actor actor in actors)
            {
                menu.Add(actor.ToString(), ()=> operation.RemoveActor(actor));
            }
            
            menu.Show();
            Console.ReadKey();
        }

        static void NewCharacter(IActors update)
        {
            Console.Clear();
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Alias: ");
            string alias = Console.ReadLine();

            Console.Write("Abilities: (Press ENTER to stop): ");
            List<string> abilities = new List<string>();
            string? ability = "";
            int count = 1;
            do
            {
                Console.Write("Ability " + "#" + count + ": ");
                ability = Console.ReadLine();
                if (ability != "")
                {
                    abilities.Add(ability);
                }
                count++;

            } while (ability != "");
            Console.Write("Who plays this character?: ");
            string actorName = Console.ReadLine();
            while (update.GetActorByName(actorName) == null)
            {
                Console.Write("Enter existing name: ");
                actorName = Console.ReadLine();
            }
            update.ConnectCharacterToActor(new Character(name, alias, abilities), actorName);
            Console.WriteLine("Character succesfully added!");
            Console.ReadKey();
        }



        static IActors Configure()
        {
            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {

                services.AddScoped<AppDbContext>();
                services.AddSingleton<IActorDataProvider, ActorDataProvider>();
                services.AddSingleton<IActors, ActorsService>();

            }).Build();
            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();

            return host.Services.GetRequiredService<IActors>();

        }

        static void ExportXml(HashSet<Movie> list)
        {
            XmlSerializer writer = new XmlSerializer(typeof(HashSet<Movie>));
            string path = Path.Combine(Directory.GetCurrentDirectory(), "MoviesXml");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream file = File.Create(Path.Combine(path, "data.xml"));

            writer.Serialize(file, list);
            Console.Clear();
            Console.WriteLine("XML file created successfully!");
            Console.ReadKey();
            file.Close();
        }
        static void ToConsole<T>(IEnumerable<T> list, Action<T> action)
        {
            Console.Clear();

            foreach (var item in list)
            {
                action(item);
            }
            Console.ReadKey();
        }


    }


}
