namespace IOQ9ET_HSZF_2024251.Model
{

    public class Actor
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public NationalityEnum Nationality { get; private set; }
        public List<Character> Characters { get; private set; }

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
