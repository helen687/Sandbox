﻿using CatalogCore;
using System.ComponentModel.DataAnnotations;

namespace CatalogWeb.Models
{
    public class BookModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? ImageId { get; set; } = null;

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public DateTime IssueDate { get; set; } = new DateTime();

        public List<AuthorModel> Authors { get; set; }
        public string AuthorsNames { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();

    }
}
