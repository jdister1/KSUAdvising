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

            //sets avaibility to none on login
            LoggedInUser.WalkinAdviser = false;
            context.SaveChanges();
            
            AdviserViewModel AdviserVM = new AdviserViewModel();
            //populates view model with adviser information
            AdviserVM.Adviser = LoggedInUser;
            AdviserVM.CollegeSetting = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == AdviserVM.Adviser.CollegeID);

            //checks if adviser is also admin 
            //AdviserVM.isAdmin      if(context.Admins.FirstOrDefault(ad => ad.BannerID == LoggedInUser.BannerID))


            //populates view model with existing walkins in queue before login
            AdviserVM.currentWalkinQueueFlashline = context.WalkinQueue.Select(w => w.FlashlineID).ToList();

            return View(AdviserVM);
        }

        [HttpPost]
        public void UpdateStatus(string adviserFlashlineID, bool isAvailable)
        {
            var changeAdviser = context.Advisers.SingleOrDefault(a => a.FlashlineID == adviserFlashlineID);
            changeAdviser.WalkinAdviser = isAvailable;
            context.SaveChanges();

        }

        [HttpPost]
        public void UpdateSettings(string adviserFlashlineID, bool schedAlert, bool walkinAlert, int volumeAlert)
        {
            var changeAdviser = context.Advisers.SingleOrDefault(a => a.FlashlineID == adviserFlashlineID);
            changeAdviser.ShowApptNotification = schedAlert;
            changeAdviser.ShowWalkinNotification = walkinAlert;
            changeAdviser.NotificationVolume = volumeAlert;
            context.SaveChanges();
        }

        [HttpPost]
        public void RemoveStudentFromWalkin(string studentFlashlineID)
        {
            var removeWalkin = context.WalkinQueue.SingleOrDefault(w => w.FlashlineID == studentFlashlineID);
            context.WalkinQueue.Remove(removeWalkin);
            context.SaveChanges();
        }

        [HttpPost]
        public void ReturnStudentToQueue(string studentFlashlineID)
        {
            var collegeID = (int)Session["collegeID"];
            WalkinQueue newWalkin = new WalkinQueue();
            newWalkin.FlashlineID = studentFlashlineID;
            newWalkin.CollegeSetting = context.CollegeSettings.SingleOrDefault(c => c.CollegeID == collegeID);
            context.WalkinQueue.Add(newWalkin);
            context.SaveChanges();
        }

    }
}
