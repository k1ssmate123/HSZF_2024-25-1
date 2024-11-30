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


                services.AddSingleton<ICharacterDataProvider, CharacterDataProvider>();
                services.AddSingleton<ICharacters, CharactersService>();

                services.AddSingleton<IMovieDataProvider, MovieDataProvider>();
                services.AddSingleton<IMovies, MoviesService>();


            }).Build();
            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();

            IActors actorService = host.Services.GetRequiredService<IActors>();
            ICharacters characterService = host.Services.GetRequiredService<ICharacters>();
            IMovies movieService = host.Services.GetRequiredService<IMovies>();




            var menu = new ConsoleMenu(args, level: 0);
            var subMenu = new ConsoleMenu(args, level: 1)
             .Add("Show actors", () => ToConsole(actorService.ListByActor(), Console.WriteLine))
             .Add("Show characters", () => ToConsole(characterService.ListByCharacter(), Console.WriteLine))
             .Add("Show movies", () => ToConsole(movieService.ListByMovie(), Console.WriteLine))
             .Add("Back", ConsoleMenu.Close);

            var addMenu = new ConsoleMenu(args, level: 1);
            addMenu.Add("Add new actor", () => NewActor(actorService))
            .Add("Add new character", () => NewCharacter(actorService))
            .Add("Add new movie", () => NewMovie(characterService,movieService))
            .Add("Add character to a movie", () => CharacterToMovie(characterService,movieService))
             .Add("Back", ConsoleMenu.Close);

            var deleteMenu = new ConsoleMenu(args, level: 1);
            deleteMenu.Add("Delete actor", () => DeleteMenu(actorService.ListByActor(), actorService));
            deleteMenu.Add("Delete character", () => DeleteMenu(characterService.ListByCharacter(), actorService));
            deleteMenu.Add("Delete movie", () => DeleteMenu(movieService.ListByMovie(), actorService));
            deleteMenu.Add("Back", ConsoleMenu.Close);

            var editMenu = new ConsoleMenu(args, level: 1);
            editMenu.Add("Edit actor", () => EditMenu(actorService.ListByActor(), actorService));
            editMenu.Add("Edit character", () => EditMenu(characterService.ListByCharacter(), characterService));
            editMenu.Add("Edit movie", () => EditMenu(movieService.ListByMovie(), movieService));
            editMenu.Add("Back", ConsoleMenu.Close);


            menu.Add("Show lists", subMenu.Show)
            .Add("Movies of Joe(s)", () => ToConsole(movieService.GetMoviesByDirector("Joe"), x => Console.WriteLine(x.Title + "\n\t" + x.Director)))
            .Add("Three highest grossing movie", () => ToConsole(movieService.ListByMovie().GroupBy(x => x.Title).Select(x => new { Title = x.Key, Grossing = x.Average(x => x.BoxOffice) }).OrderByDescending(x => x.Grossing).Take(3), x => Console.WriteLine("Title: " + x.Title + "\n\tGrossing: " + x.Grossing)))
            .Add("Actors and their characters", () => ToConsole(actorService.ListByActor(), x =>
            {
                Console.WriteLine(x.Name);
                foreach (var item in x.Character)
                {
                    
                    Console.WriteLine("\t" + item.Name);
                }
            }))
            .Add("Movies and the actors", () => MoviesAndActors(actorService,movieService))
            .Add("Actors that played together the most", () => PlayedTogetherMost(actorService, movieService))
            .Add("Add new items", addMenu.Show)
            .Add("Delete items", deleteMenu.Show)
            .Add("Edit items", editMenu.Show)
            .Add("Export movies to XML", () => ExportXml(movieService.ListByMovie()))
            .Add("Exit", () => Environment.Exit(0))
            .Configure(config =>
            {
                config.Selector = ":3";
                config.EnableFilter = true;
                config.Title = "Marvel movies:";

                config.EnableBreadcrumb = true;
            });

            menu.Show();



        }
        #region Setup (Config, Export, Console etc...)

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
        #endregion
        #region Add new items
        static void NewActor(IActors actorService)
        {
            Console.Clear();

            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Age: ");
            int age = int.Parse(Console.ReadLine());


            Console.Write("Nationality: ");
            string nationality = Console.ReadLine();

            actorService.AddActor(name, age, nationality);
            Console.WriteLine("Actor succesfully added!");
            Console.ReadKey();
        }
        static void NewCharacter(IActors actorService)
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
            while (actorService.GetActorByName(actorName) == null)
            {
                Console.Write("Enter existing name: ");
                actorName = Console.ReadLine();
            }
            actorService.ConnectCharacterToActor(new Character(name, alias, abilities), actorName);
            Console.WriteLine("Character succesfully added!");
            Console.ReadKey();
        }
        static void NewMovie(ICharacters characterService, IMovies movieService)
        {
            Console.Clear();
            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Release date: ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Director: ");
            string director = Console.ReadLine();

            Console.Write("Box Office: ");
            uint boxoffice = uint.Parse(Console.ReadLine());

            Console.Write("What characters are in this movie?: (Press ENTER to stop)");
            string charName = "";
            int count = 1;
            List<Character> characterToConnect = new List<Character>();
            do
            {
                Console.Write("\nCharacter #" + count + ": ");
                charName = Console.ReadLine();
                while (charName != "" && characterService.GetCharacterByName(charName) == null)
                {
                    Console.Write("Enter existing name: ");
                    charName = Console.ReadLine();
                }
                if (charName != "")
                {
                   characterToConnect.Add(characterService.GetCharacterByName(charName));
                }
                count++;
            } while (charName != "");
            Movie final = new Movie(title, year, director, boxoffice);
            movieService.AddMovie(final);
            foreach (var item in characterToConnect)
            {
                characterService.ConnectMovieToCharacter(final, item.Name);
            }
            Console.WriteLine("Movie succesfully added!");
            Console.ReadKey();
        }
        static void CharacterToMovie(ICharacters characterService, IMovies movieService)
        {
            var menu = new ConsoleMenu();
            HashSet<Character> charList = characterService.ListByCharacter();
            foreach (var item in charList)
            {
                menu.Add(item.ToString(), () => ListMovies(item, menu, movieService,characterService));
            }
            menu.Add("Back", ConsoleMenu.Close);
            menu.Show();
        }
        static void ListMovies(Character item, ConsoleMenu closeMenu, IMovies movieService, ICharacters characterService)
        {
            closeMenu.CloseMenu();
            var menu = new ConsoleMenu();
            HashSet<Movie> movielist = movieService.ListByMovie();
            foreach (Movie movie in movielist)
            {
                menu.Add(movie.ToString(), () => AddCharacterToMovie(movie, item.Name, menu, characterService));
            }
            menu.Add("Back", ConsoleMenu.Close);
            menu.Show();
        }

        static void AddCharacterToMovie(Movie movie, string name, ConsoleMenu menu, ICharacters characterService)
        {
            characterService.ConnectMovieToCharacter(movie, name);
            menu.CloseMenu();
        }
        #endregion
        #region Delete items
        static void DeleteMenu<T>(HashSet<T> items, IActors actorService)
        {
            var menu = new ConsoleMenu();
            if (items != null)
            {
                foreach (var item in items)
                {
                    menu.Add(item.ToString(), () => DeleteItem(item, menu,actorService));

                }
            }
            menu.Add("Back", ConsoleMenu.Close);
            menu.Show();
        }
        static void DeleteItem<T>(T item, ConsoleMenu menu, IActors actorService)
        {
            actorService.Remove(item);
            menu.CloseMenu();

        }
        #endregion
        #region Queries
        static void PlayedTogetherMost(IActors actorService, IMovies movieService)
        {
            HashSet<Actor> actors = actorService.ListByActor();
            HashSet<Movie> movies = movieService.ListByMovie();
            int count = 0;
            int max = -1;
            HashSet<string> togetherList = new HashSet<string>();
            HashSet<string> maxList = new HashSet<string>();
            foreach (var movie in movies)
            {


                foreach (var actor in actors)
                {
                    foreach (var item in actor.Character)
                    {
                        if (item.Movies.Contains(movie))
                        {
                            count++;
                            togetherList.Add(actor.Name);
                        }
                    }
                }
                if (count > max)
                {
                    max = count;
                    maxList = togetherList;
                }

                togetherList = new HashSet<string>();

                count = 0;
            }

            Console.Clear();
            Console.WriteLine("Actors that played together the most: ");
            foreach (var item in maxList)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();
        }

        static void MoviesAndActors(IActors actorService, IMovies movieService)
        {
            Console.Clear();
            HashSet<Actor> actors = actorService.ListByActor();
            HashSet<Movie> movies = movieService.ListByMovie();

            foreach (var movie in movies)
            {
                Console.WriteLine(movie.Title);
                foreach (var actor in actors)
                {
                    foreach (var item in actor.Character)
                    {
                        if (item.Movies.Contains(movie))
                        {
                            Console.Write("\t" + actor.Name + "\n");
                        }
                    }
                }
            }
            Console.ReadKey();
        }
        #endregion


        static void EditMenu<T, K>(HashSet<T> items, K service)
        {
            var menu = new ConsoleMenu();
            foreach (var item in items)
            {
                menu.Add(item.ToString(), () => EditItem(item, menu, service));
            }
            menu.Add("Back", ConsoleMenu.Close);
            menu.Show();
        }
        static void EditItem<T, K>(T item, ConsoleMenu menu, K service)
        {
            Console.WriteLine("Editing... Press ENTER to not change the data");
            if (item is Actor)
            {
                Actor actor = item as Actor;
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Age: ");
                string strage = Console.ReadLine();
                int age = 0;
                if (strage != "")
                {
                    age = int.Parse(strage);
                }

                Console.Write("Nationality: ");
                string nationality = Console.ReadLine();
                (service as IActors).EditActor(actor, name, age, nationality);

            }
            if (item is Character)
            {
                Character character = item as Character;
                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.Write("Alias: ");
                string alias = Console.ReadLine();


                (service as ICharacters).EditCharacter(character, name, alias);
            }
            if (item is Movie)
            {
                Movie movie = item as Movie;
                Console.Write("Title: ");
                string title = Console.ReadLine();
                Console.Write("Release year: ");
                string stryear = Console.ReadLine();
                int year = 0;
                if (stryear != "")
                {
                    year = int.Parse(stryear);
                }


                Console.Write("Director: ");
                string director = Console.ReadLine();

                Console.Write("Box office: ");
                string stroffice = Console.ReadLine();
                uint boxoffice = 0;
                if (stroffice != "")
                {
                    boxoffice = uint.Parse(stroffice);
                }

                  (service as IMovies).EditMovie(movie, title, year, director, boxoffice);

            }

            menu.CloseMenu();
        }
    }
}
