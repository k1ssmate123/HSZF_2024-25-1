using ConsoleTools;
using IOQ9ET_HSZF_2024251.Application;
using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
             .Add("Show actors", () => ToConsole(actorService.ListByActor()))
             .Add("Show characters", () => ToConsole(actorService.ListByCharacter()))
             .Add("Show movies", () => ToConsole(actorService.ListByMovie()))
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
            .Add("Movies of Joe(s)", () => ToConsole(actorService.GetMoviesByDirector("Joe")))
            .Add("Three highest grossing movie", () => Console.WriteLine("Two"))
            .Add("Actors and their characters", () => Console.WriteLine("Three"))
            .Add("Movies and the actors", () => Console.WriteLine("Three"))
            .Add("Actors that played together the most", () => Console.WriteLine("Three"))
            //.Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!") 
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = "+ ";
                config.EnableFilter = true;
                config.Title = "Marvel movies:";
                config.EnableWriteTitle = true;
                config.EnableBreadcrumb = true;
            }); 

            menu.Show();
        }

        static void ToConsole<T>(IEnumerable<T> list)
        {
            Console.Clear();
    
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }
    }


}
