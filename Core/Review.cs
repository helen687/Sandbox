using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogCore
{
    public class Review
    {
        public Review()
        {
            this.Id = new Guid();
            this.Description = "";
            this.Reviewer = "";
            this.Stars = 0;
        }
        public Review(Guid id, int starts, string description, string reviewer)
        {
            this.Id = id;
            this.Stars = starts;
            this.Description = description;
            this.Reviewer = reviewer;
        }

        [Key]
        public Guid Id { get; set; }

        public int Stars { get; set; }

        public string Description { get; set; }


        public string Reviewer { get; set; }

    }
}
