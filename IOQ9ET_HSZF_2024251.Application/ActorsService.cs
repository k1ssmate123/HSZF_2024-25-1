using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;

namespace IOQ9ET_HSZF_2024251.Application
{
    public interface IActors
    {
        Actor GetActorByName(string actorName);
        ICollection<Character> GetCharactersByActorName(string actorName);
        HashSet<Actor> ListByActor();

        void AddActor(string name, int age, string nationality);
        void EditActor(Actor actor, string name, int age, string nationality);

        void Remove<T>(T item);
        void ConnectCharacterToActor(Character character, string actorName);
    }
    public class ActorsService : IActors
    {
        private readonly IActorDataProvider ActorDataProvider;
        public ActorsService(IActorDataProvider actorDataProvider)
        {
            ActorDataProvider = actorDataProvider;
            string path = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.Month.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, DateTime.Now.Day.ToString() + ".txt");
            ActorDataProvider.ListingEventHandler += x => File.WriteAllText(path, x);
        }



        public Actor GetActorByName(string actorName)
        {
            return ActorDataProvider.GetActorByName(actorName);
        }



        public HashSet<Actor> ListByActor()
        {
            return ActorDataProvider.ListByActor();
        }



        public ICollection<Character> GetCharactersByActorName(string actorName)
        {
            return ActorDataProvider.GetCharactersByActorName(actorName);
        }


        public void AddActor(string name, int age, string nationality)
        {
            ActorDataProvider.AddActor(name, age, nationality);
        }
        public void ConnectCharacterToActor(Character character, string actorName)
        {
            ActorDataProvider.ConnectCharacterToActor(character, actorName);
        }



        public void Remove<T>(T item)
        {
            ActorDataProvider.Remove(item);
        }

        public void EditActor(Actor actor, string name, int age, string nationality)
        {
            ActorDataProvider.EditActor(actor, name, age, nationality);
        }
    }
}
