namespace JokeGenerator.Service.Name
{
    public class CharacterName
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname}";
        }
    }
}
