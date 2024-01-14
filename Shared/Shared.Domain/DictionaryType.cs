using System.Reflection;
using Shared.Domain.Exceptions;

namespace Shared.Domain
{
    public abstract class DictionaryType<TDictionary> where TDictionary : DictionaryType<TDictionary>
    {
        public int Id { get; }
        public string Name { get; }

        protected DictionaryType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        private static readonly IEnumerable<TDictionary> All
            = typeof(TDictionary).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(f => f.FieldType == typeof(TDictionary))
                .Select(f => f.GetValue(null))
                .Cast<TDictionary>();

        public static TDictionary Get(int id)
        {
            var result = All.SingleOrDefault(x => x.Id == id);

            return result ?? throw new ObjectNotFoundException();
        }

        public static TDictionary Get(string name)
        {
            var result = All.SingleOrDefault(x => x.Name == name);

            return result ?? throw new ObjectNotFoundException();
        }
    }
}
