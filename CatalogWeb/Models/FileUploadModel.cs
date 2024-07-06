using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Http;
namespace CatalogWeb.Models
{
    public class FileUploadModel
    {
        public Guid Id { get; set; }
        public IFormFile? FileDetails { get; set; }
    }
}