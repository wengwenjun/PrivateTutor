using PrivateTutorCenter.Helper;
using PrivateTutorCenter.Models;
using PrivateTutorCenter.Models.View_Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrivateTutorCenter.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Courses()
        {
            EditableCourse listOfCategories = initiateList();
            return View(listOfCategories);
        }

        public ActionResult StudentInstruction()
        {
            return View();
        }

        public ActionResult TeacherInstruction()
        {
            return View();
        }



        private EditableCourse initiateList()
        {
            EditableCourse model = new EditableCourse();
            List<SelectListItem> list = new List<SelectListItem>
            {
                new SelectListItem {Text="Computer Science", Value="CS" },
                new SelectListItem {Text="Mathematics", Value="Math" },
                new SelectListItem { Text="English", Value="Eng" },
            };

            model.subjects = new SelectList(list, "Value", "Text");
            return model;

        }

        [Authorize]
        public ActionResult InitiateCreditTransaction(String courseId)
        {
     /*       cId = int.Parse(courseId);
            string studentName = this.User.Identity.Name;
            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                Course c = context.courses.Find(cId);

                var studentIds = from r in context.registrations
                                 where r.courseId == cId
                                 select r.studentId;
                if (studentIds.Contains(sId))
                {
                    ViewBag.Message = "You have enrolled in " + c.title;
                    return View("Courses");
                }
            }*/

            String AppId = ConfigurationManager.AppSettings["CreditAppId"];
            String SharedKey = ConfigurationManager.AppSettings["CreditAppSharedKey"];
            String AppTransId = "19";
            String AppTransAmount = "15.00";

            String hash = HttpUtility.UrlEncode(CreditCardHasher.GenerateClientRequestHash(SharedKey, AppId, AppTransId, AppTransAmount));

            String url = "http://ectweb2.cs.depaul.edu/ECTCreditGateway/Authorize.aspx";
            url = url + "?AppId=" + AppId;
            url = url + "&TransId=" + AppTransId;
            url = url + "&AppTransAmount=" + AppTransAmount;
            url = url + "&AppHash=" + hash;
         

            return Redirect(url);
        }

        public ActionResult ProcessCreditResponse (String TransId, String TransAmount, String StatusCode, String AppHash)
        {
            String AppId = ConfigurationManager.AppSettings["CreditAppId"];
            String SharedKey = ConfigurationManager.AppSettings["CreditAppSharedKey"];

            if (CreditCardHasher.VerifyServerResponseHash(AppHash, SharedKey, AppId, TransId, TransAmount, StatusCode))
            {
                switch (StatusCode)
                {
                    case ("A"):
                        {
                            ViewBag.TransactionStatus = "Transaction Approved!";
                           // enrollStudent();
                            break;
                        }
                    case ("D"): ViewBag.TransactionStatus = "Transaction Denied!"; break;
                    case ("C"): ViewBag.TransactionStatus = "Transaction Cancelled!"; break;
                }
            }
            else
            {
                ViewBag.TransactionStatus = "Hash Verification failed.";
            }

            return View();
        }

      /*  private void enrollStudent()
        {
            using (PrivateTutorContext context = new PrivateTutorContext())
            {
                Registration r = new Registration { courseId = cId, studentId = sId };
                context.registrations.Add(r);
                context.SaveChanges();
            }
        }*/


    }
}