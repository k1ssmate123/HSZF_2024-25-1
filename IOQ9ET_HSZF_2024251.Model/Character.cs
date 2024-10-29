using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace IOQ9ET_HSZF_2024251.Model
{
    public class Character
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
 
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("abilities")]
        public List<string> Abilities { get; set; }

        [JsonProperty("movies")]
        public virtual ICollection<Movie> Movie { get; set; }

        public Character(string name, string alias, List<string> abilities, ICollection<Movie> movies)
        {
            Name = name;
            Alias = alias;
            Abilities = abilities;
            Movie = movies;
            Id = Guid.NewGuid().ToString();
        }
        public Character()
        {
            Movie = new HashSet<Movie>();
            Id = Guid.NewGuid().ToString();
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
