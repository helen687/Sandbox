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
        public AuthorModel Get(Guid id)
        {
            try
            {
                var existingAuthor = _context.Authors.Where(a => a.Id == id).Select(a => AuthorToModel(a)).FirstOrDefault();
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

        [Route("{sortBy}/{sortOrder}/{pageIndex:int}/{pageSize:int}/{filterValue?}")]
        public PaginatorResponseModel<AuthorModel> GetList(string sortBy, string sortOrder, int pageIndex, int pageSize, string filterValue = "")
        {
            var ret = new PaginatorResponseModel<AuthorModel>();
            try
            {
                var datasource = _context.Authors.ToList();
                if (filterValue != null && filterValue != String.Empty) {
                    datasource = datasource.Where(x => x.FullName.ToLower().Contains(filterValue.ToLower())).ToList();
                }
                ret.Length = datasource.Count;
                ret.PageIndex = pageIndex;
                ret.PageSize = pageSize;
                switch (sortBy)
                {
                    case "fullName":
                        datasource = datasource.OrderBy(x => x.FullName).ToList();
                        break;
                }
                if (sortOrder == "desc")
                {
                    datasource.Reverse();
                }
                datasource = datasource.Skip(pageIndex * pageSize).Take(pageSize).ToList();
                ret.DataSource = datasource.Select(a => AuthorToModel(a)).ToArray();
                ret.Error = "";
            }
            catch (Exception ex)
            {
                ret.Error = "An error occurred while retreiving a list of authors";
            }
            return ret;
        }

        [HttpPut]
        public bool Put(AuthorModel authorModel)
        {
            try
            {
                var author = new Author();
                SetModelAttributesToAuthor(authorModel, author);

                _context.Authors.Add(author);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving an author", ex);
            }
        }

        [HttpPost]
        public bool Post(AuthorModel authorModel)
        {
            try
            {
                var existingAuthor = _context.Authors.Where(a => a.Id == authorModel.Id).FirstOrDefault();
                if (existingAuthor != null)
                {
                    SetModelAttributesToAuthor(authorModel, existingAuthor);

                    _context.Authors.Update(existingAuthor);
                    _context.SaveChanges();
                    return true;
                }
                else
                {
                    throw new Exception("Author was not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating an author", ex);
            }
        }

        private static void SetModelAttributesToAuthor(AuthorModel model, Author book)
        {
            // for new book
            if (book.Id != model.Id)
                book.Id = model.Id;

            if (book.FullName != model.FullName)
                book.FullName = model.FullName;
        }

        private static AuthorModel AuthorToModel(Author author)
        {
            var authorModel = new AuthorModel()
            {
                Id = author.Id,
                FullName = author.FullName
            };
            return authorModel;
        }

    }
}
