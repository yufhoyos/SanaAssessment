using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SanaAssessment.Data.Entities
{
    public partial class Customer
    {
        public int Id { get; set; }
        [Required]
        public int IdentificationNumber { get; set; }
        [Required]
        public string FirtsName { get; set; }

        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirtsName} {MiddleName} {LastName}";
            }
        }
    }
}
