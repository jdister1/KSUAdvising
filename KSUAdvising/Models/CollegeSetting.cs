using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KSUAdvising.Models
{
    [Table("CollegeSettings")]
    public class CollegeSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollegeID { get; set; }
        public string CollegeName { get; set; }
        public bool CanAdvisorChange { get; set; }
        public int MinutesAllowedLate { get; set; }
        public int NumberWalkinAllowed { get; set; }
        public int MinutesAllowedEarly { get; set; }

        public virtual ICollection<Adviser> Advisers{ get; set; }
        public virtual ICollection<Admin> Admins{ get; set; }
        public virtual ICollection<WalkinQueue> Walkins { get; set; }

    }
}