using Microsoft.VisualBasic.FileIO;
using Microsoft.AspNetCore.Http;
using CatalogCore.FileUpload.Entities;
namespace CatalogWeb.Models
{
    public class FileUploadModel
    {
        public Guid Id { get; set; }
        public IFormFile? FileDetails { get; set; }
        public FileType FileType { get; set; }
    }
}