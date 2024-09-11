using CatalogCore;
using CatalogDB;
using CatalogWeb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CatalogWeb.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Headers;
using System.Linq;

namespace CatalogWeb.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly CatalogDBContext _context;

        public AuthorController(ILogger<AuthorController> logger, CatalogDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("{id:Guid}")]
        public Author Get(Guid id)
        {
            try
            {
                var existingAuthor = _context.Authors.Where(b => b.Id == id).FirstOrDefault();
                if (existingAuthor != null)
                {
                    return existingAuthor;
                }
                else
                {
                    throw new Exception("Author not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving an autor", ex);
            }
        }

        [Route("{filterValue?}")]
        public List<Author> GetList(string filterValue = "")
        {
            try
            {
                var datasource = _context.Authors.ToList();
                if (filterValue != null && filterValue != String.Empty) {
                    datasource = datasource.Where(x => x.FullName.ToLower().Contains(filterValue.ToLower())).ToList();
                }
                return datasource.OrderBy(a => a.FullName).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving an author", ex);
            }
        }
    }
}
