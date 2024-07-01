using CatalogCore.FileUpload.Entities;

namespace CatalogWeb.Services
{
    public interface IFileService
    {
        public Task PostFileAsync(Guid id, IFormFile fileData, FileType fileType);

        //public Task PostMultiFileAsync(List<FileUploadModel> fileData);

        public Task<FileDetails> GetFileDetails(Guid id);
    }
}
