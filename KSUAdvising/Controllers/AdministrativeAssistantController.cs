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
    public class AdministrativeAssistantController : Controller
    {
        //
        // GET: /AdministrativeAssistant/
        AdvisingDBContext context = new AdvisingDBContext();

        public ActionResult Index()
        {
            //gets anyone already in the walkin queue or advisers available for walkin
            AdministrativeAssistantViewModel adAssistantVm = new AdministrativeAssistantViewModel();
            adAssistantVm.currentAdviserWalkinQueueFlashline = context.Advisers.Where(a => a.WalkinAdviser == true).Select(a=>a.FlashlineID).ToList();
            adAssistantVm.currentWalkinQueueFlashline = context.WalkinQueue.Select(w=>w.FlashlineID).ToList();
            adAssistantVm.appointmentData = getTodaysAppointments();

            return View(adAssistantVm);
        }

        private List<Appointment> getTodaysAppointments()
        {
            //what will get returend
            List<Appointment> returnAppointments;

            //makes api call
            var collegeID = (int)Session["collegeID"];
            string requestText = "/AppointmentByGroup?groupID="+collegeID;
            var client = new RestClient("http://ssdev-01.kent.edu/KSUAdvising_WebServices/api/AdvisingApi");
            var request = new RestRequest(requestText, Method.POST);

            //deserializes JSON into Appointment objects
            IRestResponse response = client.Execute(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            returnAppointments = deserial.Deserialize<List<Appointment>>(response);

            
            //changes time format for recieved appointments
            foreach (var appt in returnAppointments)
            {
                var tempStartDate = Convert.ToDateTime(appt.AppointmentStartTime);
                var tempEndDate = Convert.ToDateTime(appt.AppointmentEndTime);
                appt.AppointmentStartTime = tempStartDate.ToShortTimeString();
                appt.AppointmentEndTime = tempEndDate.ToShortTimeString();
            }

            return returnAppointments;
        }

    }
}
