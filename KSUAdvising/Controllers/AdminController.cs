using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KSUAdvising.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        AdvisingDBContext context = new AdvisingDBContext();
        public ActionResult Index()
        {
            //instantiates adviser view model
            AdminViewModel adVm = new AdminViewModel();

            //gets current adviser
            //**WILL BE IMPLEMENTED ONCE LOGIN THING IS

            //gets college ID session variable
            var collegeID = (int)Session["collegeID"];
            adVm.CollegeSetting = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == collegeID);

            //gets current advisors available for walkin
            adVm.currentAdviserWalkinQueueFlashline = context.Advisers.Where(a => a.WalkinAdviser == true).Select(a => a.FlashlineID).ToList();


           return View(adVm);
        }

        [HttpPost]
        public void UpdateCollegeSettings(int minEarly, int minLate, int numWalkin)
        {
            var currentCollegeID = (int)Session["collegeID"];
            var currentCollege = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == currentCollegeID);
            currentCollege.MinutesAllowedEarly = minEarly;
            currentCollege.MinutesAllowedLate = minLate;
            currentCollege.NumberWalkinAllowed = numWalkin;
            context.SaveChanges();
        }

        [HttpPost]
        public JsonResult CheckAdviser(string adviserFlashlineID)
        {
            string doesExist = "False";
            string adviserFirst = "", adviserLast= "";

            var adviser = context.Advisers.FirstOrDefault(a => a.FlashlineID == adviserFlashlineID);

            if (adviser != null)
            {
                doesExist = "True";
                adviserFirst = adviser.FirstName;
                adviserLast = adviser.LastName;
            }

            return Json(new { doesExist, adviserFirst, adviserLast });
        }

        [HttpPost]
        public void UpdateAdviserStatus(string adviserFlashlineID, bool isAvailable)
        {
            var changeAdviser = context.Advisers.SingleOrDefault(a => a.FlashlineID == adviserFlashlineID);
            changeAdviser.WalkinAdviser = isAvailable;
            context.SaveChanges();

        }

        [HttpGet]
        public ActionResult AddAdviser(int adviserBannerID, string adviserFirstName, string adviserLastName, string adviserFlashlineID)
        {
            var collegeID = (int)Session["collegeID"];
            if(collegeID != null){
                //adds adviser using passed in data and defaults for notifications and minutes allowed late
                Adviser newAdviser = new Adviser() { BannerID = adviserBannerID, FirstName = adviserFirstName, LastName = adviserLastName, FlashlineID = adviserFlashlineID, ShowApptNotification = true, ShowWalkinNotification = true, NotificationVolume = 0, MinuteAllowedLate = 0, WalkinAdviser = false, CollegeID = collegeID };
                context.Advisers.Add(newAdviser);
                context.SaveChanges();
            }
                
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AddAdministrativeAssistant(int adminAssistBannerID, string adminAssistFirstName, string adminAssistLastName, string adminAssistFlashlineID)
        {
            var collegeID = (int)Session["collegeID"];
            if (collegeID != null)
            {
                //adds administrative assisitant using passed in data 
                AdministrativeAssistant newAdminAssistant = new AdministrativeAssistant() { BannerID = adminAssistBannerID, FirstName = adminAssistFirstName, LastName = adminAssistLastName, FlashlineID = adminAssistFlashlineID};
                context.AdminstrativeAssistants.Add(newAdminAssistant);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
