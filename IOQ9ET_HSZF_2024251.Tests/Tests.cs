using IOQ9ET_HSZF_2024251.Application;
using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace IOQ9ET_HSZF_2024251.Tests
{
    class Tests
    {
        IActors actorService;
        ICharacters characterService;
        IMovies movieService;

        [SetUp]
        public void SetUpFixture()
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

            actorService = host.Services.GetRequiredService<IActors>();
            characterService = host.Services.GetRequiredService<ICharacters>();
            movieService = host.Services.GetRequiredService<IMovies>();
        }


        [Test]
        public void CorrectMoviesByDirector()
        {
            var movies = movieService.GetMoviesByDirector("Joss"); //The Avangers
            Assert.That(movies.Count, Is.EqualTo(1));
        }


        [Test]
        public void CorrectActorByName()
        {
            var actor = actorService.GetActorByName("Chris Evans");
            Assert.That(actor, Is.EqualTo(actorService.ListByActor().First(x => x.Name == "Chris Evans")));

            Assert.That(actor.Name, Is.EqualTo("Chris Evans"));
            Assert.That(actor.Age, Is.EqualTo(42));
            Assert.That(actor.Nationality, Is.EqualTo("American"));

        }

        [Test]
        public void CorrectCharacterByName()
        {
            var actor = characterService.GetCharacterByName("Tony Stark");
            Assert.That(actor, Is.EqualTo(characterService.ListByCharacter().First(x => x.Name == "Tony Stark")));
            Assert.That(actor.Name, Is.EqualTo("Tony Stark"));
            Assert.That(actor.Alias, Is.EqualTo("Iron Man"));
        }

        [Test]
        public void CorrectCharactersByActorName()
        {
            var characters = actorService.GetCharactersByActorName("Robert Downey Jr.");
            Assert.That(characters.Count, Is.EqualTo(1));
            foreach (var item in characters)
            {
                Assert.That(item.Name, Is.EqualTo("Tony Stark"));
            }
        }

        [Test]
        public void DeleteActorCompleted()
        {
            var actors = actorService.ListByActor();
            int count = actors.Count;
            Actor removeactor = actors.First();
            actorService.Remove(removeactor);
            Assert.That(actorService.ListByActor().Count, Is.EqualTo(count - 1));
        }

        [Test]
        public void DeleteCharacterCompleted()
        {
            var characters = characterService.ListByCharacter();
            int count = characters.Count;

            Character removechar = characters.First();
            actorService.Remove(removechar);
            Assert.That(characterService.ListByCharacter().Count, Is.EqualTo(count - 1));
        }


        [Test]
        public void DeleteMovieCompleted()
        {
            var movies = movieService.ListByMovie();
            int count = movies.Count;

            Movie removemovie = movies.First();
            actorService.Remove(removemovie);
            Assert.That(movieService.ListByMovie().Count, Is.EqualTo(count - 1));
        }



        [Test]
        public void AddActorCompleted()
        {
            var actors = actorService.ListByActor();
            int count = actors.Count;
            actorService.AddActor("A", 1, "American");
            Assert.That(actorService.ListByActor().Count, Is.EqualTo(count + 1));
        }
        public void AddCharacterCompleted()
        {
            var actors = characterService.ListByCharacter();
            var charcount = actors.Count;

            var actor = actorService.GetActorByName("Robert Downey Jr.");
            var actorcharcount = actor.Character.Count;


            actorService.ConnectCharacterToActor(new Character("A", "a", new List<string>()), "Robert Downey Jr.");

            Assert.That(characterService.ListByCharacter().Count, Is.EqualTo(charcount + 1));
            Assert.That(actorService.GetActorByName("Robert Downey Jr.").Character.Count, Is.EqualTo(actorcharcount + 1));
        }

        [Test]
        public void AddMovieCompleted()
        {
            var movies = movieService.ListByMovie();
            var moviecount = movies.Count;

            var charmoviecount = characterService.GetCharacterByName("Tony Stark").Movies.Count;


            characterService.ConnectMovieToCharacter(new Movie("A", 2000, "A", 200000000), "Tony Stark");


            Assert.That(movieService.ListByMovie().Count, Is.EqualTo(moviecount + 1));
            Assert.That(characterService.GetCharacterByName("Tony Stark").Movies.Count, Is.EqualTo(charmoviecount + 1));


        }

        [Test]
        public void CorrectToString()
        {
            var character = characterService.GetCharacterByName("Tony Stark");
            string tostring = "Characters Name: Tony Stark\n\tAlias: Iron Man\n\tAbilities:\n\t\tGenius intellect\n\t\tAdvanced technology\n\t\tPowered armor suit\n\tMovies:\n\t\tIron Man\n\t\tIron Man 2\n\t\tAvengers: Endgame";
            Assert.That(character.ToString(), Is.EqualTo(tostring));
        }

    }
}