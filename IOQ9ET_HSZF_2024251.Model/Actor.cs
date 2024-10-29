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
        public NationalityEnum Nationality { get; set; }

        [JsonProperty("characters")]
        public virtual ICollection<Character> Character { get; set; }
        public Actor(string name, int age, NationalityEnum nationality, List<Character> characters)
        {
            Name = name;
            Age = age;
            Nationality = nationality;
            Character = characters;
            Id = Guid.NewGuid().ToString();
        }

        public Actor()
        {
            Character = new HashSet<Character>();
            Id = Guid.NewGuid().ToString();
        }

        public override string ToString()
        {
            return $"Name: {Name}\n\tAge: {Age}\n\tNationality: {Nationality}";
        }
    }
}
