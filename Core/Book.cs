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
        public Book() {
            this.Author = new Author();
            this.AuthorId = this.Author.Id;
        }
        public Book(Guid id, 
                    Guid? authorId,
                    Guid? imageId,
                    string title,
                    string description,
                    DateOnly issueDate,
                    Author author,
                    Image? image,
                    List<Review> reviews)
        {
            this.Id = id; this.Title = title;
            this.Description = description; this.IssueDate = issueDate;
            this.AuthorId = authorId;  this.Author = author;
            this.ImageId = imageId; this.Image = Image;
            this.Reviews = reviews;
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
        public Guid? ImageId { get; set; } = null;

        public string Title { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        public DateOnly IssueDate { get; set; } = new DateOnly();

        public Author Author { get; set; }
        public Image? Image { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
