using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SanaAssessment.Data.Entities
{
    public partial class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5)]
        public string SKU { get; set; }
        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
