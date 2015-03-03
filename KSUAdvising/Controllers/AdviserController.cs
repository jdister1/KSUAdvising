using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSUAdvising.Controllers
{
    public class AdviserController : Controller
    {
        //
        // GET: /Advisor/
        AdvisingDBContext context = new AdvisingDBContext();
        Adviser LoggedInUser = new Adviser();
        public ActionResult Index()
        {
            
            //if session data is empty redirect to logins creen
            if(Session["LoggedInAdviser"] == null)
                RedirectToAction("Index","Login");

            //gets flashlineID from temp data from login controller
            LoggedInUser.FlashlineID = Session["LoggedInAdviser"].ToString();
            LoggedInUser = context.Advisers.FirstOrDefault(a => a.FlashlineID == LoggedInUser.FlashlineID);
            
            AdviserViewModel AdviserVM = new AdviserViewModel();
            //populates view model with adviser information
            AdviserVM.Adviser = LoggedInUser;
            AdviserVM.CollegeSetting = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == AdviserVM.Adviser.CollegeID);

            return View(AdviserVM);
        }

    }
}
