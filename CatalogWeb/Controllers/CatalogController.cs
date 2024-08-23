using CatalogCore;
using CatalogDB;
using CatalogWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System;
using CatalogWeb.Models;
using static System.Net.Mime.MediaTypeNames;

namespace CatalogWeb.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly CatalogDBContext _context;

        public CatalogController(ILogger<CatalogController> logger, CatalogDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{id:Guid}")]
        public BookModel Get(Guid id)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == id).Include(b => b.Author).FirstOrDefault();
                if (existingBook != null)
                {
                    return BookToModel(existingBook);
                }
                else
                {
                    throw new Exception("Book not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving a book", ex);
            }
        }

        [HttpGet(Name = "GetList")]
        public IEnumerable<BookModel> GetList()
        {
            try
            {
                return _context.Books.Include(b => b.Author).Select(x => BookToModel(x)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retreiving a list of books", ex);
            }
        }

        [Route("[controller]/[action]")]
        [HttpPut(Name = "Put")]
        public bool Put(BookModel bookModel)
        {
            try
            {
                var book = new Book();
                SetModelAttributesToBook(bookModel, book, _context);
                _context.Books.Add(book);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving a book", ex);
            }
        }

        [Route("[controller]/[action]")]
        [HttpPost(Name = "Post")]
        public bool Post(BookModel bookModel)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == bookModel.Id).Include(b => b.Author).FirstOrDefault();
                if (existingBook != null)
                {
                    SetModelAttributesToBook(bookModel, existingBook, _context);
                    _context.Books.Update(existingBook);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Book not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating a book", ex);
            }
        }

        [HttpDelete(Name = "Delete")]
        [Route("{id}")]
        public void Delete(Guid id)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == id).FirstOrDefault();
                if (existingBook != null)
                {
                    _context.Books.Remove(existingBook);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing a book", ex);
            }
        }

        private static BookModel BookToModel(Book book)
        {
            return new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IssueDate = book.IssueDate.ToDateTime(new TimeOnly(0, 0, 0)),
                AuthorId = book.AuthorId,
                Author = book.Author,
                ImageId = book.ImageId,
                //Reviews = book.Reviews
            };
        }

        private static void SetModelAttributesToBook(BookModel model, Book book, CatalogDBContext context)
        {
            if (book.Id != model.Id)
                throw new Exception("Book doesn't exist");


            if(book.Title != model.Title)
                book.Title = model.Title;
            if(book.Description != model.Description)
                book.Description = model.Description;
            var dateOnly = DateOnly.FromDateTime(model.IssueDate);
            if (book.IssueDate != dateOnly)
                book.IssueDate = dateOnly;
            if (book.AuthorId != model.AuthorId)
                book.AuthorId = model.AuthorId;
            if (book.Author.FullName != model.Author.FullName)
                book.Author.FullName = model.Author.FullName;
            if (book.ImageId != model.ImageId)
            {
                book.ImageId = model.ImageId;
                book.Image = context.Images.Where(x => x.Id == book.ImageId).FirstOrDefault();
            }
            //book.Reviews = model.Reviews;
        }
    }
}
