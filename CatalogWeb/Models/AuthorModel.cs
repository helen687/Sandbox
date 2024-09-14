using CatalogCore;
using System.ComponentModel.DataAnnotations;

namespace CatalogWeb.Models
{
    public class AuthorModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = String.Empty;

    }
}
