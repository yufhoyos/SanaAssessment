using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanaAssessment.Data.Entities
{
    public partial class Category
    {
        public short Id { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
