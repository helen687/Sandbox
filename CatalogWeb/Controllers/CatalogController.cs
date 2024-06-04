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
    [Route("[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly DBContext _context;

        public CatalogController(ILogger<CatalogController> logger, DBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<Book> Get()
        {
            return _context.Books.ToArray();
        }

        [HttpPut(Name = "Put")]
        public void Put(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        [HttpPost(Name = "Post")]
        public void Post(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);
            if (existingBook != null) {
                _context.Books.Update(book);
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
