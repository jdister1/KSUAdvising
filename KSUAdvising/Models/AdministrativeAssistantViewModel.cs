using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KSUAdvising.Models
{
    public class AdministrativeAssistantViewModel
    {
       public List<String> currentWalkinQueueFlashline { get; set; }
       public List<String> currentAdviserWalkinQueueFlashline { get; set; }
    }
}