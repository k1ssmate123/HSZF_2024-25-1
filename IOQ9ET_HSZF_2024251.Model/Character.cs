using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOQ9ET_HSZF_2024251.Model
{
    public class Character
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("abilities")]
        public List<string> Abilities { get; set; }

        [JsonProperty("movies")]
        public List<Movie> Movies { get; set; }

        public Character(string name, string alias, List<string> abilities, List<Movie> movies)
        {
            Name = name;
            Alias = alias;
            Abilities = abilities;
            Movies = movies;
        }
        public Character()
        {
            
        }

        public override string ToString()
        {
            string tostring = $"Characters Name: {Name}\n\tAlias: {Alias}\n\tAbilities:";
            foreach (string s in Abilities)
            {
                tostring += "\n\t\t"+s;
            }
            return tostring;
        }
    }
}
