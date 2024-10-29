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
            var host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {

                services.AddScoped<AppDbContext>();
                services.AddSingleton<IActorDataProvider, ActorDataProvider>();
                services.AddSingleton<IActors, ActorsService>();

            }).Build();
            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();

            IActors actorService = host.Services.GetRequiredService<IActors>();






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
