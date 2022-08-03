using Container.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Container.Controllers
{
    public class HomeController : Controller
    {
        //public IFormsAuthenticationService FormsService { get; set; }
        //public IMembershipService MembershipService { get; set; }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
        //    if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

        //    base.Initialize(requestContext);
        //}

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            meichalimEntities db = new meichalimEntities();
            // string encPassword = GenericHandler.EncodePasswordToBase64(password);
            tbl_users user = db.tbl_users.Where(x => x.UserEmail.Equals(email)).FirstOrDefault(); //&& x.password.Equals(encPassword)).FirstOrDefault();
            if (user != null )
            {
                HttpCookie cookie = new HttpCookie(".ASPXAUTH");
                cookie.Value = FormsAuthentication.Encrypt(new FormsAuthenticationTicket(1, user.UserEmail, DateTime.Now, DateTime.MaxValue, false, user.userNum.ToString()));
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult NoPermission()
        {
            return View();
        }
    }
}