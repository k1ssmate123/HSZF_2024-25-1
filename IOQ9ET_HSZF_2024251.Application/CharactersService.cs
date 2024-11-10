using IOQ9ET_HSZF_2024251.Model;
using IOQ9ET_HSZF_2024251.Persistence.MsSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Application
{
    public interface ICharacters
    {
        Character GetCharacterByName(string charName);
        HashSet<Character> ListByCharacter();
        void EditCharacter(Character character, string name, string alias);
        void ConnectMovieToCharacter(Movie movie, string charName);
    }

    public class CharactersService : ICharacters
    {
        private readonly ICharacterDataProvider CharacterDataProvider;

        public CharactersService(ICharacterDataProvider CharacterDataProvider)
        {
            this.CharacterDataProvider = CharacterDataProvider;
            string path = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.Month.ToString());
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, DateTime.Now.Day.ToString() + ".txt");
            CharacterDataProvider.ListingEventHandler += x => File.WriteAllText(path, x);
        }


        public Character GetCharacterByName(string charName)
        {
            return CharacterDataProvider.GetCharacterByName(charName);
        }

        public HashSet<Character> ListByCharacter()
        {
            return CharacterDataProvider.ListByCharacter();
        }
        public void EditCharacter(Character character, string name, string alias)
        {
            CharacterDataProvider.EditCharacter(character, name, alias);
        }

        public void ConnectMovieToCharacter(Movie movie, string charName)
        {
            CharacterDataProvider.ConnectMovieToCharacter(movie, charName);
        }
    }
}
