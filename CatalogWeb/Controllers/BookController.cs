using CatalogCore;
using CatalogDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogWeb.Models;

namespace CatalogWeb.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly CatalogDBContext _context;

        public BookController(ILogger<BookController> logger, CatalogDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{id:Guid}")]
        public BookModel Get(Guid id)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == id).Include(b => b.Authors).FirstOrDefault();
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
                var datasource = _context.Books.Include(b => b.Authors).Select(x => BookToModel(x)).ToList();
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
                    case "authorsNames":
                        datasource = datasource.OrderBy(x => x.AuthorsNames).ToList();
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

        [HttpPut]
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

        [HttpPost]
        public bool Post(BookModel bookModel)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == bookModel.Id).Include(b => b.Authors).FirstOrDefault();
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
            var bookModel = new BookModel()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                IssueDate = book.IssueDate.ToDateTime(new TimeOnly(0, 0, 0)),
                Authors = new List<AuthorModel>() { },
                ImageId = book.ImageId,
                AuthorsNames = ""
                //Reviews = book.Reviews
            };
            foreach (var author in book.Authors) {
                bookModel.Authors.Add(new AuthorModel() { Id = author.Id, FullName = author.FullName });
                bookModel.AuthorsNames += author.FullName + ", "; 
            }
            bookModel.AuthorsNames = bookModel.AuthorsNames.Trim();
            bookModel.AuthorsNames = bookModel.AuthorsNames.TrimEnd(',');
            return bookModel;
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

            
            foreach (var authorModel in model.Authors) {
                if (!book.Authors.Any(b => b.FullName.Trim() == authorModel.FullName.Trim()))
                {
                    // add author to book if not exist
                    var existingAuthor = context.Authors.Where(a => a.FullName.Trim() == authorModel.FullName.Trim()).FirstOrDefault();
                    if (existingAuthor == null)
                    {
                        var authorToAdd = new Author() { Id = authorModel.Id, FullName = authorModel.FullName.Trim() };
                        context.Authors.Add(authorToAdd);
                        book.Authors.Add(authorToAdd);
                    }
                    else
                    {
                        book.Authors.Add(existingAuthor);
                    }
                }
            }
            var itemsToRemove = new List<Author>() {  };
            foreach (var existingBookAuthor in book.Authors)
            {
                var modelBookAuthor = model.Authors.Where(ba => ba.FullName.Trim() == existingBookAuthor.FullName.Trim()).FirstOrDefault();
                if (modelBookAuthor == null)
                {
                    itemsToRemove.Add(existingBookAuthor);
                }
            }
            foreach (var author in itemsToRemove)
            {
                book.Authors.Remove(author);
            }
            //book.Reviews = model.Reviews;
        }
    }
}
