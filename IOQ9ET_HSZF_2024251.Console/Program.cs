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


            ToConsole<Actor>(actorService.ListByActor());

            Actor a = actorService.GetActorByName("SEGG");

           /* var subMenu = new ConsoleMenu(args, level: 1)
             .Add("Színészek kilistázása", () => ToConsole(actorService.ListByActor()))
             .Add("Karakterek kilistázása", () => ToConsole(actorService.ListByCharacter()))
             .Add("Filmek kilistázása", () => ToConsole(actorService.ListByMovie()))
             .Configure(config =>
               {
                   config.Selector = ":3 ";
                   config.EnableFilter = true;
                   config.Title = "Kilistázás";
                   config.EnableBreadcrumb = true;
                   config.WriteBreadcrumbAction = titles => Console.WriteLine(string.Join(" / ", titles));
               });

            var menu = new ConsoleMenu(args, level: 0)
            .Add("Adatok kilistázása", subMenu.Show)
            .Add("Joe filmjei", () => Console.WriteLine("One"))
            .Add("Három legnagyobb bevételű film", () => Console.WriteLine("Two"))
            .Add("Színészek és karaktereik", () => Console.WriteLine("Three"))
            .Add("Filmek és hozzájuk tartozó színészek", () => Console.WriteLine("Three"))
            .Add("Legtöbbet együttjátszó színészek", () => Console.WriteLine("Three"))
            //.Add("Change me", (thisMenu) => thisMenu.CurrentItem.Name = "I am changed!") 
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = "+ ";
                config.EnableFilter = true;
                config.Title = "Marvel filmek:";
                config.EnableWriteTitle = true;
                config.EnableBreadcrumb = true;
            });

            menu.Show();*/
        }

         static void ToConsole<T>(IEnumerable<T> list)
        {
           
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
