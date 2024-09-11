using CatalogCore;
using CatalogDB;
using CatalogWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogWeb.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Headers;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        [Route("{sortBy}/{sortOrder}/{pageIndex:int}/{pageSize:int}/{filterValue?}")]
        public PaginatorResponseModel GetList(string sortBy, string sortOrder, int pageIndex, int pageSize, string filterValue = "")
        {
            var ret = new PaginatorResponseModel();

            try
            {
                var datasource = _context.Books.Include(b => b.Author).Select(x => BookToModel(x)).ToList();
                if (filterValue != null && filterValue != String.Empty) {
                    datasource = datasource.Where(x => x.Title.ToLower().Contains(filterValue.ToLower())).ToList();
                }
                ret.Length = datasource.Count;
                ret.PageIndex = pageIndex;
                ret.PageSize = pageSize;
                switch (sortBy) {
                    case "title":
                        datasource = datasource.OrderBy(x => x.Title).ToList();
                        break;
                    case "author.fullName":
                        datasource = datasource.OrderBy(x => x.Author.FullName).ToList();
                        break;
                } 
                if(sortOrder == "desc") {
                    datasource.Reverse();
                }
                datasource = datasource.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                ret.DataSource = datasource.ToArray();
                ret.Error = "";
            }
            catch (Exception ex)
            {
                ret.Error = "An error occurred while retreiving a list of books";
            }
            return ret;
        }

        [Route("[controller]/[action]")]
        [HttpPut(Name = "Put")]
        public bool Put(BookModel bookModel)
        {
            try
            {
                var book = new Book();
                SetModelAttributesToBook(bookModel, book, _context);
                // create new author or use existing
                var existingAuthor = _context.Authors.Where(a => a.FullName.Trim().ToLower() == bookModel.Author.FullName).FirstOrDefault();
                if (existingAuthor != null)
                {
                    book.AuthorId = existingAuthor.Id;
                    book.Author = null;
                }
                else
                {
                    var newAuthor = new Author();
                    newAuthor.FullName = bookModel.Author.FullName;
                    book.AuthorId = newAuthor.Id;
                    _context.Authors.Add(newAuthor);
                }
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
                    if (bookModel.Author.FullName != existingBook.Author.FullName)
                    {
                        // create new author or use existing
                        var existingAuthor = _context.Authors.Where(a => a.FullName.Trim().ToLower() == bookModel.Author.FullName).FirstOrDefault();
                        if (existingAuthor != null)
                        {
                            existingBook.AuthorId = existingAuthor.Id;
                        }
                        else {
                            var newAuthor = new Author();
                            newAuthor.FullName = bookModel.Author.FullName;
                            existingBook.AuthorId = newAuthor.Id;
                            _context.Authors.Add(newAuthor);
                        }
                    }

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
            // for new book
            if (book.Id != model.Id)
                book.Id = model.Id;

            if(book.Title != model.Title)
                book.Title = model.Title;
            if(book.Description != model.Description)
                book.Description = model.Description;
            var dateOnly = DateOnly.FromDateTime(model.IssueDate);
            if (book.IssueDate != dateOnly)
                book.IssueDate = dateOnly;
            if (book.ImageId != model.ImageId)
            {
                book.ImageId = model.ImageId;
                book.Image = context.Images.Where(x => x.Id == book.ImageId).FirstOrDefault();
            }
            //book.Reviews = model.Reviews;
        }
    }
}
