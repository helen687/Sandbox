using CatalogCore;
using CatalogDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet(Name = "GetList")]
        public IEnumerable<Book> GetList()
        {
            return _context.Books.ToArray();
        }

    }
}
