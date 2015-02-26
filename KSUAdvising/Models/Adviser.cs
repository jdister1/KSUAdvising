using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KSUAdvising.Models
{
    [Table("Advisers")]
    public class Adviser
    {
        [Key]
        public int AdvisorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}