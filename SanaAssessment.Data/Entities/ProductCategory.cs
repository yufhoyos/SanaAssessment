using System;
using System.Collections.Generic;
using System.Text;

namespace SanaAssessment.Data.Entities
{
   public class ProductCategory
    {
        public short CategoryId { get; set; }
        public Category Category { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
