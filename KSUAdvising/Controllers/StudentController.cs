using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSUAdvising.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/
        AdvisingDBContext context = new AdvisingDBContext();
        public ActionResult Index()
        {
            //instantiates studnet view model
            var StudentVM = new StudentViewModel();

            //gets logged in student
            var studentFlashlineID = Session["LoggedInStudent"].ToString();
            StudentVM.flashlineID = studentFlashlineID;
            
            //get appointment data to see if student has an appointment
            //**Will need to get adviser name as well to (1) send appointment and (2) see how early/late they allow people of college allows
            //advisers to set this. Also will get students full name from appointment data
            StudentVM.hasAppointment = false;

            //uses collegeID to see if they are within time frame to showup for appointment if they have one
            var collegeID = (int)Session["collegeID"];
            var collegeInformation = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == collegeID);
            bool adviserCanChange = collegeInformation.CanAdvisorChange;
            int minLate, minEarly;
            if(adviserCanChange)
            {
                //assign minLate & minEarly from adviser data
            }
            else
            {
                minEarly = collegeInformation.MinutesAllowedEarly;
                minLate = collegeInformation.MinutesAllowedLate;
            }

            StudentVM.isLate = false;

            StudentVM.walkinsAllowed = true;
            
            return View(StudentVM);
        }

    }
}
