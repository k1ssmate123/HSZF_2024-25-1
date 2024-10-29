using IOQ9ET_HSZF_2024251.Model;
using Newtonsoft.Json;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql

{

    public class JsonObject
    {
        [JsonProperty("actors")]
        public List<Actor> Actors { get; set; }
    }

    public delegate void ListingEvent(string message);
    public interface IActorDataProvider
    {
        Actor GetActorByName(string actorName);
        //Movie GetMovieByDirector(string directorName);
        ICollection<Character> GetCharactersByActorName(string actorName);
        HashSet<Character> ListByCharacter();
        HashSet<Actor> ListByActorName();
        HashSet<Movie> ListByMovie();

        public event ListingEvent ListingEventHandler;
        List<Movie> GetMoviesByDirector(string directorName);

    }
    public class ActorDataProvider : IActorDataProvider
    {
        private readonly AppDbContext context;


        public event ListingEvent ListingEventHandler;
        public ActorDataProvider(AppDbContext context)
        {
            this.context = context;
            var rootObject = JsonConvert.DeserializeObject<JsonObject>(File.ReadAllText("movies.json"));
            foreach (var item in rootObject.Actors)
            {
                context.Actors.Add(item);
            }
            try { context.SaveChanges(); }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
                Console.ReadKey();
            }
        }


        #region Get all data in list 
        public HashSet<Actor> ListByActorName()
        {
            HashSet<Actor> actors = new HashSet<Actor>();
            ListingEventHandler.Invoke("All actors have been listed at: " + DateTime.Now);
            foreach (var item in context.Actors)
            {
                actors.Add(item);
            }
            return actors;
        }


        public HashSet<Character> ListByCharacter()
        {
            HashSet<Character> characters = new HashSet<Character>();
            ListingEventHandler.Invoke("All characters have been listed at: " + DateTime.Now);
            foreach (var item in context.Actors)
            {
                foreach (var chars in item.Character)
                {
                    characters.Add(chars as Character);
                }
            }
            return characters;
        }

        public HashSet<Movie> ListByMovie()
        {
            HashSet<Movie> movieList = new HashSet<Movie>();
            ListingEventHandler.Invoke("All movies have been listed at: " + DateTime.Now);

            foreach (var item in context.Actors)
            {
                foreach (var chars in item.Character)
                {
                    foreach (var movies in chars.Movie)
                    {
                        movieList.Add(movies as Movie);
                    }
                }
            }
            return movieList;
        }
        #endregion


        public Actor GetActorByName(string actorName)
        {

            return context.Actors.First(x => x.Name.Contains(actorName));
        }

        public ICollection<Character> GetCharactersByActorName(string actorName)
        {

            return GetActorByName(actorName).Character;

            //return context.Actors.Select(x=>GetActorByName(actorName));
        }

        public List<Movie> GetMoviesByDirector(string directorName)
        {

            return ListByMovie().Where(x => x.Director.Split(' ')[0] == directorName).ToList();

            //return context.Actors.Select(x => x.Characters.Select(y => y.Movies.Where(z => z.Director.Contains(directorName))).Select(a=>a));
        }
    }
}
