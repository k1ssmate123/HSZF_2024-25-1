using Newtonsoft.Json;

namespace IOQ9ET_HSZF_2024251.Model
{

    public class Actor
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("nationality")]
        public NationalityEnum Nationality { get; set; }

        [JsonProperty("characters")]
        public List<Character> Characters { get; set; }
        public Actor(string name, int age, NationalityEnum nationality, List<Character> characters)
        {
            Name = name;
            Age = age;
            Nationality = nationality;
            Characters = characters;
        }

        public Actor()
        {}

        public override string ToString()
        {
            return $"Name: {Name}\n\tAge: {Age}\n\tNationality: {Nationality}";
        }
    }
}
