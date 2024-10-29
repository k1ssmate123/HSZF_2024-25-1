using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOQ9ET_HSZF_2024251.Application
{
    public interface IActors
    {
        Actor GetActorByName(string actorName);
        //Movie GetMovieByDirector(string directorName);
        ICollection<Character> GetCharactersByActorName(string actorName);
        HashSet<Actor> ListByActor();
        HashSet<Character> ListByCharacter();     
        HashSet<Movie> ListByMovie();

        ICollection<Movie> GetMoviesByDirector(string directorName);
    }
    public class ActorsService : IActors
    {
        private readonly IActorDataProvider ActorDataProvider;
        public ActorsService(IActorDataProvider actorDataProvider)
        {
            ActorDataProvider = actorDataProvider;
            string path = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.Month.ToString());
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, DateTime.Now.Day.ToString()+".txt");
            ActorDataProvider.ListingEventHandler += x=>File.WriteAllText(path,x);
        }

     

        public Actor GetActorByName(string actorName)
        {
            return ActorDataProvider.GetActorByName(actorName); 
        }

        public HashSet<Actor> ListByActor()
        {
            return ActorDataProvider.ListByActorName();
        }
        public HashSet<Character> ListByCharacter()
        {
            return ActorDataProvider.ListByCharacter();
        }
        public HashSet<Movie> ListByMovie()
        {
            return ActorDataProvider.ListByMovie();
        }

        public ICollection<Character> GetCharactersByActorName(string actorName)
        {
            return ActorDataProvider.GetCharactersByActorName(actorName);
        }

        public ICollection<Movie> GetMoviesByDirector(string directorName)
        {
            return ActorDataProvider.GetMoviesByDirector(directorName);
        }
    }
}
