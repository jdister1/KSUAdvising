using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class AdviserViewModel
    {
        public Adviser Adviser { get; set; }
        public bool isAdmin { get; set; }
        public CollegeSetting CollegeSetting { get; set; }
        public List<String> currentWalkinQueueFlashline { get; set; }
    }
}