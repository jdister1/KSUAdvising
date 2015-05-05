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
            //advisers to set this.
            var studentAppointments = getTodaysAppointments(StudentVM.flashlineID);
            StudentVM.hasAppointment = (studentAppointments.Count == 0) ?  false : true;

            if (StudentVM.hasAppointment)
            {
                //if student has an appointment fill the view model data with what is given by appointment rest service
                 StudentVM.studentName = studentAppointments[0].StudentFirstName +" " + studentAppointments[0].StudentLastName;
                 StudentVM.adviserFlashlineID = studentAppointments[0].AdvisorFlashlineID;
                 StudentVM.adviserName = studentAppointments[0].AdvisorFirstName + " " + studentAppointments[0].AdvisorLastName;
            }

            //uses collegeID to see if they are within time frame to showup for appointment if they have one
            var collegeID = (int)Session["collegeID"];
            var collegeInformation = context.CollegeSettings.FirstOrDefault(c => c.CollegeID == collegeID);
            bool adviserCanChange = collegeInformation.CanAdvisorChange;
            int minLate, minEarly;
            if(adviserCanChange && StudentVM.hasAppointment)
            {
                minLate = context.Advisers.FirstOrDefault(a => a.FlashlineID == StudentVM.adviserFlashlineID).MinuteAllowedLate;
            }
            else
            {
                minEarly = collegeInformation.MinutesAllowedEarly;
                minLate = collegeInformation.MinutesAllowedLate;
            }

            //when we can actually check if student is late
            //if(Convert.ToDateTime(studentAppointments[0].AppointmentStartTime).Subtract(DateTime.Now).TotalMinutes > minLate)
            StudentVM.isLate = false;

            StudentVM.walkinsAllowed = true;
            
            return View(StudentVM);
        }

        private List<Appointment> getTodaysAppointments(string flashlineID)
        {
            //what will get returend
            List<Appointment> returnAppointments;

            //makes api call
            var collegeID = (int)Session["collegeID"];
            string requestText = "/AppointmentByGroup?groupID=" + collegeID + "&date=02/24/2014";;
            var client = new RestClient("http://ssdev-01.kent.edu/KSUAdvising_WebServices/api/AdvisingApi");
            var request = new RestRequest(requestText, Method.POST);

            //deserializes JSON into Appointment objects
            IRestResponse response = client.Execute(request);
            RestSharp.Deserializers.JsonDeserializer deserial = new JsonDeserializer();
            returnAppointments = deserial.Deserialize<List<Appointment>>(response);

            //filters out for this student only
            returnAppointments = returnAppointments.Where(appt => appt.StudentFlashlineID == flashlineID).ToList();

            return returnAppointments;
        }
    }
}
