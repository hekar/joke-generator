using System;

namespace JokeGenerator.Service.Joke
{
    public class Joke
    {
        internal string DefaultName;

        public string Value { get; set; }
        public string Url { get; set; }
        public string[] Categories { get; set; }
        public string Id { get; set; }
        public string IconUrl { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public Joke()
        {
        }

        public Joke(string value) : this()
        {
            this.Value = value;
        }

        public Joke(string value, string defaultName) : this(value)
        {
            this.DefaultName = defaultName;
        }

        internal string ReplacedName(string replaceWith)
        {
            if (!string.IsNullOrWhiteSpace(this.DefaultName))
            {
                return Value.Replace(DefaultName, replaceWith);
            }
            else
            {
                return Value;
            }
        }
    }
}
