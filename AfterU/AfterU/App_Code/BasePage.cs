using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;

public partial class BasePage : System.Web.UI.Page 
{
    private int _userId;
    public int UserId
    {
        get
        {
            String tmpStr = String.Empty;
            HttpCookie loginCookie = HttpContext.Current.Request.Cookies["userId"];  //"Test" is the cookie name specified in the config file.   
            if (loginCookie != null)
            {
                FormsAuthenticationTicket t = FormsAuthentication.Decrypt(loginCookie.Value);
                _userId = Convert.ToInt32(t.Name);
            }
            
            return _userId;
        }
        set
        {
            _userId = value;
            HttpCookie cookie = FormsAuthentication.GetAuthCookie(_userId.ToString(), true); //true is used to create a persistent cookie
            cookie.Expires = DateTime.Now.AddDays(2); 
            Response.Cookies.Add(cookie);
        }
    }

    private String _DC = String.Empty;
    public String DC
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["dc"]))
            {
                _DC = HttpContext.Current.Request["dc"].ToString();
            }
            return _DC;
        }
    }

    public void createCookie(String cName, String cValue, int dayExpire)
    {
        HttpCookie cookie = new HttpCookie(cName, cValue);
        cookie.Expires = DateTime.Now.AddDays(dayExpire);

        string environment = ConfigurationManager.AppSettings.Get("debug");
        if (environment == "true")
            cookie.Domain = "localhost"; 
        else
            cookie.Domain = "afteru-ppc.co.il";
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    public String getCookie(String cName, String defaultValue)
    {
        if (HttpContext.Current.Request.Cookies[cName] != null)
        {
            return HttpContext.Current.Request.Cookies[cName].Value;
        }
        return defaultValue;
    }

    private String _unickDevice = String.Empty;
    public String UnickDevice
    {
        get
        {
            _unickDevice = getCookie("unickDev", "");
            if (_unickDevice == "")
            {
                Guid id = Guid.NewGuid();
                _unickDevice = id.ToString();
                createCookie("unickDev", _unickDevice, 365*10);    
            }
            return _unickDevice;
        }
    }

    private String _unick24Key = String.Empty;
    public String Unick24Key
    {
        get
        {
            _unick24Key = getCookie("unickKey1", "");
            if (_unick24Key == "")
            {
                Guid id = Guid.NewGuid();
                _unick24Key = id.ToString();
                createCookie("unickKey1", _unick24Key, 1);
            }
            return _unick24Key;
        }
    }

    private int _leadId;
    public int LeadId
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["lId"]))
            {
                _leadId = Convert.ToInt32(HttpContext.Current.Request["lId"]);
            }
            return _leadId;
        }
    }

    private string _msgLead;
    public string MsgLead
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["Msg"]))
            {
                _msgLead = HttpContext.Current.Request["Msg"].ToString();
            }
            return _msgLead;
        }
    }

    private int _statusId;
    public int StatusId
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["sId"]))
            {
                _statusId = Convert.ToInt32(HttpContext.Current.Request["sId"]);
            }
            return _statusId;
        }
    }

    private String _eventDate;
    public String UEventDate
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["UEventDate"]))
            {
                _eventDate = HttpContext.Current.Request["UEventDate"].ToString();
            }
            return _eventDate;
        }
    }

    private String _desc = " ";
    public String Desc
    {
        get
        {
            if (!String.IsNullOrEmpty(HttpContext.Current.Request["Desc"]))
            {
                _desc = HttpContext.Current.Request["Desc"].ToString();
            }
            return _desc;
        }
    }

    public String _pageName = String.Empty;
    public String PageName
    {
        get
        {
            if (String.IsNullOrEmpty(_pageName))
            {
                _pageName = HttpContext.Current.Request.Url.ToString();
                _pageName = _pageName.Substring(_pageName.LastIndexOf("/")+1);
                if (_pageName.IndexOf('?') > 0)
                {
                    _pageName = _pageName.Substring(0,_pageName.IndexOf('?'));
                }
            }
            return _pageName;
        }
    }

    private ClientData _Client = null;
    public ClientData Client
    {
        get
        {
            if (_Client == null)
            {
                _Client = new ClientData(UserId);
            }

            return _Client;
        }
    }



    public string LOGO
    {
        get
        {
            return "http://afteru-ppc.co.il/images/logoafteru.png";
        }
    }

    

    protected string AffiliateName
    {
        get
        {
            return "אפטר יו";
        }
    }

    protected string DomainName
    {
        get
        {
           
            return "http://afteru-ppc.co.il/";
        }
    }

    protected string FolderName
    {
        get
        {
            return "afteru";
        }
    }

    protected string AffiliateSenderMail
    {
        get
        {
            return "service@ppcmedia.co.il";
        }
    }

    protected string AffiliateEMail
    {
        get
        {
            return "pini.levi@gmail.com";
        }
    }

    public BasePage()
    {
        string ip;
        string environment = ConfigurationManager.AppSettings.Get("debug");
        if (environment=="true")
            ip = "212.179.199.85"; 
        else 
            ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        string userAgent = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        string url = HttpContext.Current.Request.Url.ToString();
        string queryString = HttpContext.Current.Request.QueryString.ToString();
        string form = HttpContext.Current.Request.Form.ToString();
        int groupId = 0;
        string dataSec = "";
        string userName = "";
        string password = "";

        try
        {
            if (HttpContext.Current.Request["groupId"] == null)
                groupId = 0;
            else groupId = Convert.ToInt32("0" + HttpContext.Current.Request["groupId"].ToString());
        }
        catch (Exception ex)
        {
            groupId = 0;
        }

        try
        {
            if (HttpContext.Current.Request.Form["uName"] == null)
                userName = "";
            else
                userName = HttpContext.Current.Request.Form["uName"].ToString();
            if (HttpContext.Current.Request.Form["uPass"] == null)
                password = "";
            else password = HttpContext.Current.Request.Form["uPass"].ToString();

        }
        catch (Exception ex)
        {
            userName = "";
            password = "";
        }

       

        if (Client.UserPermission == 3)
        {

        }

        if ((UserId >= 0) && (PageName == "login.aspx"))
        {
            //Response.Redirect("default.aspx");
            //FormsAuthentication.RedirectFromLoginPage(UserId.ToString(), true);
        }        
    }
}