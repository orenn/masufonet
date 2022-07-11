using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Container
{
    public partial class tbl_users
    {
        public static int CurrentUserId
        {
            get
            {
                try
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                        if (ticket != null)
                        {
                            return Convert.ToInt32(ticket.UserData);
                        }
                    }
                }
                catch { }

                return 0;
            }
        }

        public static tbl_users Current
        {
            get
            {
                try
                {
                    tbl_users user = null;
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                        if (ticket != null)
                        {
                            int userId = Convert.ToInt32(ticket.UserData);
                            meichalimEntities db = new meichalimEntities();
                            user = db.tbl_users.SingleOrDefault(u=>u.UserNum==userId);
                        }
                    }

                    return user;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}