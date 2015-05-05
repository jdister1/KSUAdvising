using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using RestSharp;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace KSUAdvising.Controllers
{
    public class LoginController : Controller
    {
        

        //
        // GET: /Login/
        AdvisingDBContext context = new AdvisingDBContext();


        public ActionResult Index(int ?colInp,bool ?isValidInp)
        {
            //seeds data of some colleges and some advisors in case data store changes
            //seedData();

            //resets session variables
            Session["LoggedInStudent"] = null;
            Session["LoggedInAdviser"] = null; 

            //view model
            LoginViewModel lvm = new LoginViewModel();

            //gets list of colleges
            lvm.colleges = context.CollegeSettings.ToList();
            //gets list of college names
            lvm.allCollegeNames = lvm.colleges.Select(c => c.CollegeName).ToList();

            //check if a college was specified in the URL, if not return view with negative college value meaning the user must specify
            if (colInp.HasValue)
            {
                lvm.col = (int)colInp;
                //creates session variable to maintain this value for as long as possible
                Session["collegeID"] = (int)colInp;
                lvm.collegeName = lvm.colleges.FirstOrDefault(c => c.CollegeID == lvm.col).CollegeName;
            }
            else if(Session["collegeID"] != null)
            {//if colInp wasn't specified try the Session variable to see if it retained its value
                lvm.col = (int)Session["collegeID"];
                lvm.collegeName = lvm.colleges.FirstOrDefault(c => c.CollegeID == lvm.col).CollegeName;
            }
            else
            {//last resorts ask someone to fill value back in
                lvm.col = -1;
            }
            
            //check if isValid has a value, if not then 
            if (!isValidInp.HasValue)
                lvm.isValid = true;
            else
                lvm.isValid = (bool)isValidInp;

            return View(lvm);
        }

        [HttpGet]
        public ActionResult Authenticate(string userName,string password)
        {
            userName = userName.ToLower();

            //send request to web service
            string requestText = "/Authenticate?flashlineID=" + userName + "&password=" + password;
            var client = new RestClient("http://ssdev-01.kent.edu/KSUAdvising_WebServices/api/AdvisingApi");
            var request = new RestRequest(requestText, Method.POST);
            IRestResponse response = client.Execute(request);

            //checks response to see if value login
            var isValidLogin = (response.Content == "false") ? false : true;

            //for testing purposes i am the only admin assisitant in the db
            if (userName == "jdister1")
                isValidLogin = true;

            if (isValidLogin)
            {
                
                //looks for username in advisor table
                var user = context.Advisers.FirstOrDefault(a => a.FlashlineID == userName);

                if (user != null)
                {
                    Session["LoggedInAdviser"] = userName;
                    return RedirectToAction("Index", "Adviser");
                }

                //looks for username in administrative assistant table
                var userAdAs = context.AdminstrativeAssistants.FirstOrDefault(ad => ad.FlashlineID == userName);
                if (userAdAs != null)
                {
                    Session["LoggedInAdminAssisit"] = userName;
                    return RedirectToAction("Index", "AdministrativeAssistant");
                }

                //if authenticated and no prior check matched, it is a student
                Session["LoggedInStudent"] = userName;
                return RedirectToAction("Index", "Student");
                
            }
            else //send error back to view by calling index method again
            {
                return RedirectToAction("Index", "Login", new { isValidInp = isValidLogin});
            }
        }

        public ActionResult SetCollege(int collegeID)
        {
            Session["collegeID"] = collegeID;
            return RedirectToAction("Index", "Login");
        }

        public ActionResult AddStudentToQueue()
        {
            var studentFlashlineID = (string)Session["LoggedInStudent"];
            var collegeID = (int)Session["collegeID"];
            WalkinQueue newWalkin = new WalkinQueue();
            newWalkin.FlashlineID = studentFlashlineID;
            newWalkin.CollegeSetting = context.CollegeSettings.SingleOrDefault(c=> c.CollegeID == collegeID);
            context.WalkinQueue.Add(newWalkin);
            context.SaveChanges();

            return RedirectToAction("Index", "Login");
        }

        public ActionResult EmailLink()
        {
            var studentFlashlineID = Session["LoggedInStudent"];
            string toEmail = studentFlashlineID + "@kent.edu";
            sendEmail(toEmail);
            return RedirectToAction("Index", "Login");
        }

        #region EmailSettings
        private void sendEmail(string toEmail)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("joedister@gmail.com");
            mailMessage.Subject = "Advising Appointment Link";
            mailMessage.Body = "Here is the link to schedule and advising appointment!";
            mailMessage.To.Add(new MailAddress(toEmail));

            //create smtp client
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";

            //network credentials
            smtp.EnableSsl = true;
            NetworkCredential netCred = new NetworkCredential();
            netCred.UserName = "team3softwareintegration@gmail.com";
            netCred.Password = "team3password";


            smtp.UseDefaultCredentials = true;
            smtp.Credentials = netCred;
            smtp.Port = 587;
            smtp.Send(mailMessage);
        }
        #endregion

        #region seedingDatabase
        private void seedData()
        {
            //seedColleges();
            //seedAdvisers();
            //seedAdminAssisitants();
        }
        private void seedColleges()
        {
            //college of business
            CollegeSetting cob = new CollegeSetting();
            cob.CollegeID = 1;
            cob.CollegeName = "College of Business";
            cob.CanAdvisorChange = true;
            cob.MinutesAllowedLate = 5;
            cob.MinutesAllowedEarly = 5;
            cob.NumberWalkinAllowed = 10;
            context.CollegeSettings.Add(cob);

            //arts and sciences
            CollegeSetting coa = new CollegeSetting();
            coa.CollegeID = 2;
            coa.CollegeName = "College of Arts and Sciences";
            coa.CanAdvisorChange = false;
            coa.MinutesAllowedLate = 5;
            coa.MinutesAllowedEarly = 5;
            coa.NumberWalkinAllowed = 15;
            context.CollegeSettings.Add(coa);

            //education
            CollegeSetting coe = new CollegeSetting();
            coe.CollegeID = 3;
            coe.CollegeName = "College of Education";
            coe.CanAdvisorChange = true;
            coe.MinutesAllowedLate = 5;
            coe.MinutesAllowedEarly = 5;
            coe.NumberWalkinAllowed = 20;
            context.CollegeSettings.Add(coe);

            context.SaveChanges();
        }

        private void seedAdvisers()
        {
            Adviser bbaker = new Adviser(){BannerID = 16,FirstName = "Beverley",LastName="Baker",FlashlineID="bbaker",ShowApptNotification = true,ShowWalkinNotification=true,NotificationVolume = 10, MinuteAllowedLate = 5,WalkinAdviser = false,CollegeID = 3};
            Adviser cbright = new Adviser() { BannerID = 17, FirstName = "Christine", LastName = "Bright", FlashlineID = "cbright", ShowApptNotification = true, ShowWalkinNotification = true, NotificationVolume = 40, MinuteAllowedLate = 5, WalkinAdviser = false, CollegeID = 3 };
            Adviser amckinne = new Adviser() { BannerID = 18, FirstName = "Angie", LastName = "Mckinney", FlashlineID = "amckinne", ShowApptNotification = true, ShowWalkinNotification = true, NotificationVolume = 60, MinuteAllowedLate = 5, WalkinAdviser = false, CollegeID = 3 };
            Adviser jdrake = new Adviser() { BannerID = 19, FirstName = "Johnny", LastName = "Drake", FlashlineID = "jdrake", ShowApptNotification = true, ShowWalkinNotification = true, NotificationVolume = 60, MinuteAllowedLate = 5, WalkinAdviser = false, CollegeID = 1 };
            context.Advisers.Add(bbaker);
            context.Advisers.Add(cbright);
            context.Advisers.Add(amckinne);
            context.Advisers.Add(jdrake);

            context.SaveChanges();
        }

        private void seedAdminAssisitants()
        {
            AdministrativeAssistant jdister1 = new AdministrativeAssistant() { BannerID = 26, FirstName = "Joe", LastName = "Dister", FlashlineID = "jdister1" };
            context.AdminstrativeAssistants.Add(jdister1);

            context.SaveChanges();
        }
        #endregion
    }
}
