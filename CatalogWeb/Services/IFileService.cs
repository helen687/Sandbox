using CatalogCore;

namespace CatalogWeb.Services
{
    public interface IFileService
    {
        public Task PostFileAsync(Guid id, IFormFile fileData);

        //public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task<Image> GetFileDetails(Guid id);
    }
}
