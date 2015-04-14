using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string AdvisorFirstName { get; set; }
        public string AdvisorLastName { get; set; }
        public string AdvisorFlashlineID { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFlashlineID { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public string GroupName { get; set; }
        public int GroupID { get; set; }

    }
}