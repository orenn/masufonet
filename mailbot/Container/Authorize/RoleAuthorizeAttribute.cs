using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Container.Authorize
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        protected int level{ get; set; }
        private bool reqStatus { get; set; }

        public RoleAuthorizeAttribute(int level)
        {
            this.level = level;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            reqStatus= level == tbl_users.Current.User_Level;
            return level==tbl_users.Current.User_Level;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!this.reqStatus)
            {
                filterContext.Result = new RedirectResult(("/Home/NoPermission"));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
}