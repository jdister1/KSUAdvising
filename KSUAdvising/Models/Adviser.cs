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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BannerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FlashlineID { get; set; }
        public bool ShowApptNotification { get; set; }
        public bool ShowWalkinNotification { get; set; }
        public int NotificationVolume { get; set; }
        public int MinuteAllowedLate { get; set; }
        public bool WalkinAdviser { get; set; }
        public int CollegeID { get; set; }

        public virtual CollegeSetting CollegeSetting{ get; set; }


    }
}