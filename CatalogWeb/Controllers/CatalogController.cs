using CatalogCore;
using CatalogDBContext;
using CatalogWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;
using System;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly DBContext _context;

        public CatalogController(ILogger<CatalogController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetList")]
        public IEnumerable<Book> GetList()
        {
            try
            {
                return _context.Books.Include(b => b.Author).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retreiving a list of books", ex);
            }
        }

        [HttpPut(Name = "Put")]
        public void Put(Book book)
        {
            try
            {
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                throw new Exception("An error occurred while saving a book", ex);
            }
        }

        [HttpPost(Name = "Post")]
        public void Post(Book book)
        {
            try
            {
                var existingBook = _context.Books.Where(b => b.Id == book.Id).Include(b => b.Author).FirstOrDefault();
                if (existingBook != null)
                {
                    existingBook.SetPropertiesFromAnotherBook(book);
                    _context.Books.Update(existingBook);
                    _context.SaveChanges();
                }
                else {
                    throw new Exception("Book not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating a book", ex);
            }
        }

        [HttpDelete(Name = "Delete")]
        public void Delete(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);
            if (existingBook != null)
            {
                _context.Books.Remove(book);
            }
        }
    }
}
