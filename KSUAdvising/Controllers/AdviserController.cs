using KSUAdvising.Models;
using RestSharp;
using RestSharp.Deserializers;
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

            var collegeID = (int)Session["collegeID"];
            AdviserVM.CollegeSetting = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == collegeID);

            //checks if adviser is also admin 
            //AdviserVM.isAdmin      if(context.Admins.FirstOrDefault(ad => ad.BannerID == LoggedInUser.BannerID))

            //populates with available for walkin in case set by administrator
            AdviserVM.isWalkinAtLogin = AdviserVM.Adviser.WalkinAdviser;

            //populates with appointment data
            AdviserVM.appointmentData = getTodaysAppointments(AdviserVM.Adviser.FlashlineID);

            //populates view model with existing walkins in queue before login
            AdviserVM.currentWalkinQueueFlashline = context.WalkinQueue.Select(w => w.FlashlineID).ToList();

            return View(AdviserVM);
        }

        private List<Appointment> getTodaysAppointments(string flashlineID)
        {
            //what will get returend
            List<Appointment> returnAppointments;

            //makes api call
            var collegeID = (int)Session["collegeID"];
            string requestText = "/AppointmentByGroup?groupID=" + collegeID + "&date=02/24/2014";
            var client = new RestClient("http://ssdev-01.kent.edu/KSUAdvising_WebServices/api/AdvisingApi");
            var request = new RestRequest(requestText, Method.POST);

            //deserializes JSON into Appointment objects
            IRestResponse response = client.Execute(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            returnAppointments  = deserial.Deserialize<List<Appointment>>(response);

            //filters out for this adviser only
            returnAppointments = returnAppointments.Where(appt => appt.AdvisorFlashlineID == flashlineID).ToList();

            //changes time format for recieved appointments
            foreach(var appt in returnAppointments)
            {
                var tempDate = Convert.ToDateTime(appt.AppointmentStartTime);
                appt.AppointmentStartTime = tempDate.ToShortTimeString();
            }
            return returnAppointments;
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
