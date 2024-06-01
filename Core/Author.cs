using System.ComponentModel.DataAnnotations;

namespace CatalogCore
{
    public class Author
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; } = new DateOnly();

        public DateOnly DeathDate { get; set; } = new DateOnly();

        public Author() { }
        public Author(Guid id,string firstName, string middleName, string lastName, DateOnly birthDate, DateOnly deathDate)
        {
            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            BirthDate = birthDate;
            DeathDate = deathDate;
        }
    }
}
