using System;

namespace JokeGenerator.Service.Joke
{
    public class CategoryQuery
    {
        public static readonly CategoryQuery None = new CategoryQuery(string.Empty);

        public string Category { get; }

        public CategoryQuery(string category)
        {
            this.Category = category;
        }

        public override bool Equals(object obj)
        {
            return obj is CategoryQuery query &&
                   Category == query.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Category);
        }
    }
}
