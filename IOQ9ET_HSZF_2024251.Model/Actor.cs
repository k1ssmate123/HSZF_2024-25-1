using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOQ9ET_HSZF_2024251.Model
{

    public class Actor
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("characters")]
        public virtual ICollection<Character> Character { get; set; }
        public Actor(string name, int age, string nationality)
        {
            Name = name;
            Age = age;
            Nationality = nationality;
        }

        public Actor()
        {
            Character = new HashSet<Character>();
        
        }

        public override string ToString()
        {
            return $"Name: {Name}\n\tAge: {Age}\n\tNationality: {Nationality}";
        }
    }
}
