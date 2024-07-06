using CatalogCore;
using CatalogWeb.Models;
using CatalogWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using static System.Net.Mime.MediaTypeNames;

namespace CatalogWeb.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IFileService _uploadService;

        public ImageController(IFileService uploadService)
        {
            _uploadService = uploadService;
        }

        /// <summary>
        /// Single File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost(Name = "Upload")]
        public async Task<ActionResult> Upload([FromForm] FileUploadModel fileDetails)
        {
            if (fileDetails == null || fileDetails.FileDetails == null)
            {
                return BadRequest();
            }

            try
            {
                await _uploadService.PostFileAsync(fileDetails.Id, fileDetails.FileDetails);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///// <summary>
        ///// Multiple File Upload
        ///// </summary>
        ///// <param name="file"></param>
        ///// <returns></returns>
        //[HttpPost("PostMultipleFile")]
        //public async Task<ActionResult> PostMultipleFile([FromForm] List<FileUploadModel> fileDetails)
        //{
        //    if (fileDetails == null)
        //    {
        //        return BadRequest();
        //    }

        //    try
        //    {
        //        await _uploadService.PostMultiFileAsync(fileDetails);
        //        return Ok();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Download File
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Name = "Download")]
        [Route("{id:Guid}")]
        public async Task<ActionResult> Download(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                var file = _uploadService.GetFileDetails(id);
                var content = new MemoryStream(file.Result.FileData);
                return File(content, "image/*", file.Result.FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get file Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("{id:Guid}")]
        public async Task<Image> Get(Guid id)
        {
            try
            {
                var file = await _uploadService.GetFileDetails(id);
                return file;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}