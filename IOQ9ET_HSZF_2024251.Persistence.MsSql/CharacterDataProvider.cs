using IOQ9ET_HSZF_2024251.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Persistence.MsSql
{
    public interface ICharacterDataProvider
    {
        Character GetCharacterByName(string charName);
        HashSet<Character> ListByCharacter();
        void EditCharacter(Character character, string name, string alias);
        void ConnectMovieToCharacter(Movie movie, string charName);
        public event ListingEvent ListingEventHandler;
    }
    public class CharacterDataProvider : ICharacterDataProvider
    {
        private readonly AppDbContext context;
        public event ListingEvent ListingEventHandler;
        public CharacterDataProvider(AppDbContext context)
        {
            this.context = context;
        }
        public HashSet<Character> ListByCharacter() 
        {
            HashSet<Character> characters = new HashSet<Character>();
            ListingEventHandler.Invoke("All characters have been listed at: " + DateTime.Now);
            foreach (var item in context.Characters.Include(x=>x.Movies))
            {

                characters.Add(item);

            }
            return characters;
        }
        public Character GetCharacterByName(string charName)
        {

            var character = context.Characters.FirstOrDefault(x => x.Name.ToLower() == charName.ToLower());

            return character;

        }
        public void EditCharacter(Character character, string name, string alias)
        {
            if (name != "")
            {
                character.Name = name;
            }
            if (alias != "")
            {
                character.Alias = alias;
            }
        }
        public void ConnectMovieToCharacter(Movie movie, string charName)
        {
            GetCharacterByName(charName).Movies.Add(movie);
            context.SaveChanges();
        }

    }
}
