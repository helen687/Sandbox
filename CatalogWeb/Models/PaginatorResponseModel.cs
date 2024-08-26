using CatalogCore;

namespace CatalogWeb.Models
{
    public class PaginatorResponseModel
    {
        public int Length { get; set; } = 0;
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public BookModel[] DataSource { get; set; } = [];
        public string Error { get; set; } = "";
    }
}
