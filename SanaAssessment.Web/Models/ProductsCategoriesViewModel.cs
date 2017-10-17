using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SanaAssessment.Web.Models
{
    public class ProductsCategoriesViewModel
    {
        public short CategoryId { get; set; }
        public string CategoryDescription { get; set; }
        public bool Assigned { get; set; }
    }
}
