using CatalogCore.FileUpload.Entities;
using CatalogWeb.Services;
using CatalogCore;
using Microsoft.EntityFrameworkCore;
using CatalogDB;

namespace CatalogCore.FileUpload.Services
{
    public class FileService : IFileService
    {
        private readonly CatalogDBContext dbContextClass;

        public FileService(CatalogDBContext dbContextClass)
        {
            this.dbContextClass = dbContextClass;
        }

        public async Task PostFileAsync(Guid id, IFormFile fileData, FileType fileType)
        {
            try
            {
                var fileDetails = new FileDetails()
                {
                    Id = id,
                    FileName = fileData.FileName,
                    FileType = fileType,
                };

                using (var stream = new MemoryStream())
                {
                    fileData.CopyTo(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                var result = dbContextClass.FileDetails.Add(fileDetails);
                await dbContextClass.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public async Task PostMultiFileAsync(List<FileUploadModel> fileData)
        //{
        //    try
        //    {
        //        foreach (FileUploadModel file in fileData)
        //        {
        //            var fileDetails = new FileDetails()
        //            {
        //                Id = file.Id,
        //                FileName = file.FileDetails.FileName,
        //                FileType = file.FileType,
        //            };

        //            using (var stream = new MemoryStream())
        //            {
        //                file.FileDetails.CopyTo(stream);
        //                fileDetails.FileData = stream.ToArray();
        //            }

        //            var result = dbContextClass.FileDetails.Add(fileDetails);
        //        }
        //        await dbContextClass.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public Task<FileDetails> GetFileDetails(Guid id)
        {
            try
            {
                var file = dbContextClass.FileDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}