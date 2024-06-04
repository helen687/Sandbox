using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogCore
{
    public class Book
    {
        public Book() { }
        public Book(Guid id, Guid? authorId,
                    string title,
                    string description,
                    DateOnly issueDate,
                    Author author,
                    List<Review> reviews)
        {
            this.Id = id; this.Title = title;
            this.Description = description; this.IssueDate = issueDate;
            this.AuthorId = authorId;  this.Author = author; 
            this.Reviews = reviews;
        }

        public void SetPropertiesFromAnotherBook(Book anotherBook) {
            this.Id = anotherBook.Id;
            this.Title = anotherBook.Title;
            this.Description = anotherBook.Description; 
            this.IssueDate = anotherBook.IssueDate;
            this.AuthorId = anotherBook.AuthorId; 
            this.Author.Id = anotherBook.Author.Id;
            this.Author.FirstName = anotherBook.Author.FirstName;
            this.Author.MiddleName = anotherBook.Author.MiddleName;
            this.Author.LastName = anotherBook.Author.LastName;
            this.Author.BirthDate = anotherBook.Author.BirthDate;
            this.Author.DeathDate = anotherBook.Author.DeathDate;
            //this.Reviews = anotherBook.reviews;
        }
        public void AddReview(Review review)
        {
            this.Reviews.Add(review);
        }

        public void SetReview(Review review)
        {
            var existingReview = this.Reviews.Find(r => r.Id == review.Id);
            if (existingReview != null)
            {
                existingReview = review;
            }
        }

        public void DeleteReview(Review review)
        {
            var existingReview = this.Reviews.Find(r => r.Id == review.Id);
            if (existingReview != null)
            {
                this.Reviews.Remove(existingReview);
            }
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid? AuthorId { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public DateOnly IssueDate { get; set; } = new DateOnly();

        public Author Author { get; set; }

        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
