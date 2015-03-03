using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class LoginViewModel
    {
        public bool isValid { get; set; }
        public int col { get; set; }
        public string collegeName { get; set; }
        public List<CollegeSetting> colleges {get; set;}
        public List<string> allCollegeNames { get; set; }
    }

}