using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class AdminViewModel
    {
        public Adviser Adviser { get; set; }
        public CollegeSetting CollegeSetting { get; set; }
        public List<String> currentAdviserWalkinQueueFlashline { get; set; }
    }

}