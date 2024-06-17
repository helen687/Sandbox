using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace CatalogCore
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FullName { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; } = null;

        public DateOnly? DeathDate { get; set; } = null;

        //public Book Book { get; set; } = null!;
        public Author() { }
        public Author(Guid id,string fullName, string middleName, string lastName, DateOnly birthDate, DateOnly deathDate)
        {
            Id = id;
            FullName = fullName;
            BirthDate = birthDate;
            DeathDate = deathDate;
        }
    }
}
