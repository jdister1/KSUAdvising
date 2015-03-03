using KSUAdvising.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Authenticate(string userName,string password)
        {
            //send request to web service
            var isValidLogin = true;

            if (isValidLogin)
            {
                
                //gets back some user information (we were given username at login so using that right now) 
                //var flashlineID = "jdrake";

                //looks for username in advisor table
                var user = context.Advisers.FirstOrDefault(a => a.FlashlineID == userName);

                if (user != null)
                {
                    Session["LoggedInAdviser"] = userName;
                    return RedirectToAction("Index", "Adviser");
                }
                else
                {
                    Session["LoggedInStudent"] = userName;
                    return RedirectToAction("Index", "Student");
                }
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

        #region seedingDatabase
        private void seedData()
        {
            //seedColleges();
            //seedAdvisers();
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

        #endregion
    }
}
