using IOQ9ET_HSZF_2024251.Model;
using Newtonsoft.Json;
using System.Data.Entity;
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
        ICollection<Character> GetCharactersByActorName(string actorName);
        HashSet<Actor> ListByActor();

        void AddActor(string name, int age, string nationality);
        void EditActor(Actor actor, string name, int age, string nationality);

        void Remove<T>(T item);
        void ConnectCharacterToActor(Character character, string actorName);
        public event ListingEvent ListingEventHandler;
    }
    public class ActorDataProvider : IActorDataProvider
    {
        private readonly AppDbContext context;
        public event ListingEvent ListingEventHandler;
        public ActorDataProvider(AppDbContext context)
        {
            this.context = context;
    
        }
       

        #region Get all data in list 
        public HashSet<Actor> ListByActor()
        {
            HashSet<Actor> actors = new HashSet<Actor>();
            ListingEventHandler.Invoke("All actors have been listed at: " + DateTime.Now);
            foreach (var item in context.Actors.Include(t=>t.Character))
            {
                actors.Add(item);
            }
            return actors;
        }




        #endregion
        #region Remove Items
        public void Remove<T>(T item)
        {
            if (item is Actor)
            {
                context.Remove(item);
            }
            else if (item is Character)
            {

                var delete = context.Characters.FirstOrDefault(x => x.Equals(item));

                context.Remove(delete);

            }
            else if (item is Movie)
            {
                var delete = context.Movies.FirstOrDefault(x => x.Equals(item));
                context.Remove(delete);
            }


            context.SaveChanges();
        }
        #endregion
        #region Add items
        public void ConnectCharacterToActor(Character character, string actorName)
        {
            GetActorByName(actorName).Character.Add(character);

            context.SaveChanges();
        }


        public void AddActor(string name, int age, string nationality)
        {
            context.Actors.Add(new Actor(name, age, nationality));
            context.SaveChanges();
        }
        #endregion
        #region Return by condition
        public Actor GetActorByName(string actorName)
        {
            return context.Actors.FirstOrDefault(x => x.Name.ToLower() == actorName.ToLower());
        }

        public ICollection<Character> GetCharactersByActorName(string actorName)
        {

            return GetActorByName(actorName).Character;

            //return context.Actors.Select(x=>GetActorByName(actorName));
        }



        public void EditActor(Actor actor, string name, int age, string nationality)
        {
            if (name != "")
            {
                actor.Name = name;
            }
            if (age > 0 && age < 100)
            {
                actor.Age = age;
            }
            if (nationality != "")
            {
                actor.Nationality = nationality;
            }
        }




        #endregion

    }
}
