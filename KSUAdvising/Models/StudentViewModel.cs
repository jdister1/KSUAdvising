using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class StudentViewModel
    {
        public bool hasAppointment { get; set; }
        public bool isLate { get; set; }
        public bool walkinsAllowed { get; set; }
        public string flashlineID { get; set; }
        public string studentName { get; set; }
        public string adviserFlashlineID { get; set; }
        public string adviserName { get; set; }
    }
}