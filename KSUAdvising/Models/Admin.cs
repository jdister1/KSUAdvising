using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KSUAdvising.Models
{
    [Table("Admins")]
    public class Admin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BannerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CollegeID { get; set; }

        public virtual CollegeSetting CollegeSetting { get; set; }
    }
}