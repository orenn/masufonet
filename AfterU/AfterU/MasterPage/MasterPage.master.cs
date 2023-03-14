using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web.Security;

public partial class MasterPage_MasterPage : System.Web.UI.MasterPage
{

  
    public string UserName;
    public string UserId;
    public string RoleId;

    public string ConfigurationId;
    public string HebDate;
    public string Name;
    public string SchoolId;






    protected void Page_Load(object sender, EventArgs e)
    {
        //bool val1 = Request.IsAuthenticated;

        //hdnIsUser.Value = "sdsdds";

        HttpCookie cookie = null;

        if (HttpContext.Current.Request.Cookies["UserData"] != null)
        {
           cookie = HttpContext.Current.Request.Cookies["UserData"];

        }

     
        try
        {
            UserId = cookie["UserId"];
            RoleId = cookie["RoleId"];
            ConfigurationId = cookie["ConfigurationId"];
            SchoolId = cookie["SchoolId"];

            UserName = Server.UrlDecode(cookie["UserName"]);
            Name = Server.UrlDecode(cookie["Name"]);
            HebDate = Server.UrlDecode(cookie["HebDate"]);



        }
        catch (Exception ex)
        {
            UserId = "";
            RoleId = "";
            UserName = "";
            ConfigurationId = "";
            Name = "";
            HebDate = "";
            SchoolId = "";

        }

       // FormsAuthentication.SignOut();
    
    }




    



    public string GetSafeRequest(string requestField)
    {
        string tmpField = Request.QueryString[requestField];
        if (tmpField != null)
        {
            if (tmpField != "")
            {
                return tmpField;
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }

    }
}
