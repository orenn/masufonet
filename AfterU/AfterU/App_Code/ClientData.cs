using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
/// Summary description for ClientData
/// </summary>
public class ClientData
{
    private DataSet ds;

    private String _UserName = String.Empty;
    public String UserName
    {
        get
        {
            if (String.IsNullOrEmpty(_UserName))
            {
                _UserName = ds.Tables[0].Rows[0]["uName"].ToString();
            }
            return _UserName;
        }
        set
        {
            _UserName = value;
        }
    }

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
    }

    private int _CuserId = 0;
    public int CUserId
    {
        get
        {
            if (_CuserId == 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _CuserId = Convert.ToInt32(ds.Tables[0].Rows[0]["uId"]);
                }
            }
            return _CuserId;
        }
        set
        {
            _CuserId = value;
        }
    }

    private String _FullName = String.Empty;
    public String FullName
    {
        get
        {
            if (String.IsNullOrEmpty(_FullName))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _FullName = ds.Tables[0].Rows[0]["fName"].ToString();
                }
            }
            return _FullName;
        }
        set
        {
            _FullName = value;
        }
    }

    private String _Password = String.Empty;
    public String Password
    {
        get
        {
            if (String.IsNullOrEmpty(_Password))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _Password = ds.Tables[0].Rows[0]["uPass"].ToString();
                }
            }
            return _Password;
        }
        set
        {
            _Password = value;
        }
    }

    private String _Phone = String.Empty;
    public String Phone
    {
        get
        {
            if (String.IsNullOrEmpty(_Phone))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _Phone = ds.Tables[0].Rows[0]["uPhone"].ToString();
                }
            }
            return _Phone;
        }
        set
        {
            _Phone = value;
        }
    }

    private String _Email = String.Empty;
    public String Email
    {
        get
        {
            if (String.IsNullOrEmpty(_Email))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _Email = ds.Tables[0].Rows[0]["uEmail"].ToString();
                }
            }
            return _Email;
        }
        set
        {
            _Email = value;
        }
    }

    private String _CompanyName = String.Empty;
    public String CompanyName
    {
        get
        {
            if (String.IsNullOrEmpty(_CompanyName))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _CompanyName = ds.Tables[0].Rows[0]["companyName"].ToString();
                }
            }
            return _CompanyName;
        }
        set
        {
            _CompanyName = value;
        }
    }

    private String _CompanyId = String.Empty;
    public String CompanyId
    {
        get
        {
            if (String.IsNullOrEmpty(_CompanyId))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _CompanyId = ds.Tables[0].Rows[0]["companyId"].ToString();
                }
            }
            return _CompanyId;
        }
        set
        {
            _CompanyId = value;
        }
    }

    private String _logo = String.Empty;
    public String Logo
    {
        get
        {
            if (String.IsNullOrEmpty(_logo))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _logo = ds.Tables[0].Rows[0]["logo"].ToString();
                }
            }
            return _logo;
        }
        set
        {
            _logo = value;
        }
    }

    private String _description = String.Empty;
    public String Description
    {
        get
        {
            if (String.IsNullOrEmpty(_description))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _description = ds.Tables[0].Rows[0]["description"].ToString();
                }
            }
            return _description;
        }
        set
        {
            _description = value;
        }
    }

    private String _Address = String.Empty;
    public String Address
    {
        get
        {
            if (String.IsNullOrEmpty(_Address))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _Address = ds.Tables[0].Rows[0]["address"].ToString();
                }
            }
            return _Address;
        }
        set
        {
            _Address = value;
        }
    }

    private int _SendAlert = 0;
    public int SendAlert
    {
        get
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _SendAlert = Convert.ToInt32(ds.Tables[0].Rows[0]["sendAlert"]);
                }
            }
            return _SendAlert;
        }
        set
        {
            _SendAlert = value;
        }
    }

    private int _belongs2UserId = -1;
    public int Belongs2UserId
    {
        get
        {
            if (_belongs2UserId == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _belongs2UserId = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["Belongs2UserId"]);
                }
            }
            return _belongs2UserId;
        }
        set
        {
            _belongs2UserId = value;
        }
    }

    private int _ParentId = -1;
    public int ParentId
    {
        get
        {
            if (_ParentId == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ParentId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentId"]);
                }
            }
            return _ParentId;
        }
        set
        {
            _ParentId = value;
        }
    }

    private int _UserPermission = -1;
    public int UserPermission
    {
        get
        {
            if (_UserPermission == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _UserPermission = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["userPermission"]);
                }
            }
            return _UserPermission;
        }
        set
        {
            _UserPermission = value;
        }
    }

    private string _EndDate = "";
    public string EndDate
    {
        get
        {
            if (_EndDate == "")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _EndDate = ds.Tables[0].Rows[0]["endDate"].ToString();
                }
            }
            return _EndDate;
        }
        set
        {
            _EndDate = value;
        }
    }

    private int _FromH = 0;
    public int FromH
    {
        get
        {
            if (_FromH == 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _FromH = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["fromH"].ToString() );
                }
            }
            return _FromH;
        }
        set
        {
            _FromH = value;
        }
    }

    private int _ToH = 0;
    public int ToH
    {
        get
        {
            if (_ToH == 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ToH = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["toH"].ToString());
                }
            }
            return _ToH;
        }
        set
        {
            _ToH = value;
        }
    }

    private int _SendType = -1;
    public int SendType
    {
        get
        {
            if (_SendType == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _SendType = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["SendType"].ToString());
                }
            }
            return _SendType;
        }
        set
        {
            _SendType = value;
        }
    }

    private int _FilterType = -1;
    public int FilterType
    {
        get
        {
            if (_FilterType == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _FilterType = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["filterType"]);
                }
            }
            return _FilterType;
        }
        set
        {
            _FilterType = value;
        }
    }

    private int _AffiliateId = -1;
    public int AffiliateId
    {
        get
        {
            if (_AffiliateId == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _AffiliateId = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["affiId"]);
                }
            }
            return _AffiliateId;
        }
        set
        {
            _AffiliateId = value;
        }
    }

    private double _DelayLead = -1;
    public double DelayLead
    {
        get
        {
            if (_DelayLead == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _DelayLead = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["delay"]);
                }
            }
            return _DelayLead;
        }
        set
        {
            _DelayLead = value;
        }
    }

    private double _Commission = -1;
    public double Commission
    {
        get
        {
            if (_Commission == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _Commission = Convert.ToDouble("0" + ds.Tables[0].Rows[0]["commission"]);
                }
            }
            return _Commission;
        }
        set
        {
            _Commission = value;
        }
    }

    private Double _LeadCounter = -1;
    public Double LeadCounter
    {
        get
        {
            if (_LeadCounter == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _LeadCounter = Convert.ToDouble(ds.Tables[0].Rows[0]["leadCounter"]);
                }
            }
            return _LeadCounter;
        }
        set
        {
            _LeadCounter = value;
        }
    }

    private String _SuperUser = String.Empty;
    public String SuperUser
    {
        get
        {
            if (String.IsNullOrEmpty(_SuperUser))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _SuperUser = ds.Tables[0].Rows[0]["superUser"].ToString();
                }
            }
            return _SuperUser;
        }
        set
        {
            _SuperUser = value;
        }
    }

    private String _ProviderType = String.Empty;
    public String ProviderType
    {
        get
        {
            if (String.IsNullOrEmpty(_ProviderType))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _ProviderType = ds.Tables[0].Rows[0]["providerType"].ToString();
                }
            }
            return _ProviderType;
        }
        set
        {
            _ProviderType = value;
        }
    }

    private String _UserType = String.Empty;
    public String UserType
    {
        get
        {
            if (String.IsNullOrEmpty(_UserType))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _UserType = ds.Tables[0].Rows[0]["userType"].ToString();
                }
            }
            return _UserType;
        }
        set
        {
            _UserType = value;
        }
    }

    private String _SiteOwner = String.Empty;
    public String SiteOwner
    {
        get
        {
            if (String.IsNullOrEmpty(_SiteOwner))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _SiteOwner = ds.Tables[0].Rows[0]["siteOwner"].ToString();
                }
            }
            return _SiteOwner;
        }
        set
        {
            _SiteOwner = value;
        }
    }

    private String _discountPercent = String.Empty;
    public String DiscountPercent
    {
        get
        {
            if (String.IsNullOrEmpty(_discountPercent))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _discountPercent = ds.Tables[0].Rows[0]["discountPercent"].ToString();
                }
            }
            return _discountPercent;
        }
        set
        {
            _discountPercent = value;
        }
    }

    private String _SiteIds = String.Empty;
    public String SiteIds
    {
        get
        {
            if (String.IsNullOrEmpty(_SiteIds))
            {
                _SiteIds = ds.Tables[0].Rows[0]["siteIds"].ToString();
            }
            return _SiteIds;
        }
        set
        {
            _SiteIds = value;
        }
    }

    private int _SaleDelay = -1;
    public int SaleDelay
    {
        get
        {
            if (_SaleDelay == -1)
            {
                _SaleDelay = Convert.ToInt32(ds.Tables[0].Rows[0]["saleDelay"]);
            }
            return _SaleDelay;
        }
        set
        {
            _SaleDelay = value;
        }
    }

    private int _CanReport = -1;
    public int CanReport
    {
        get
        {
            if (_CanReport == -1)
            {
                _CanReport = Convert.ToInt32(ds.Tables[0].Rows[0]["canReport"]);
            }
            return _CanReport;
        }
        set
        {
            _CanReport = value;
        }
    }

    private String _NoteData = String.Empty;
    public String NoteData
    {
        get
        {
            if (String.IsNullOrEmpty(_NoteData))
            {
                _NoteData = ds.Tables[0].Rows[0]["note"].ToString();
            }
            return _NoteData;
        }
        set
        {
            _NoteData = value;
        }
    }

    private String _Note = String.Empty;
    public String Note
    {
        get
        {
            return _Note;
        }
        set
        {
            _Note = value;
        }
    }

    private int _isActive = -1;
    public int IsActive
    {
        get
        {
            if (_isActive == -1)
            {
                _isActive = Convert.ToInt32(ds.Tables[0].Rows[0]["isActive"]);
            }
            return _isActive;
        }
        set
        {
            _isActive = value;
        }
    }

    private string _Maskyoo = string.Empty;
    public string Maskyoo
    {
        get
        {
            if (_Maskyoo == string.Empty)
            {
                _Maskyoo = ds.Tables[0].Rows[0]["maskyoo"].ToString();
            }
            return _Maskyoo;
        }
        set
        {
            _Maskyoo = value;
        }
    }

    private int _pickOrPush = -1;
    public int PickOrPush
    {
        get
        {
            if (_pickOrPush == -1)
            {
                _pickOrPush = Convert.ToInt32(ds.Tables[0].Rows[0]["pickOrPush"].ToString());
            }
            return _pickOrPush;
        }
        set
        {
            _pickOrPush = value;
        }
    }

    private int _dayLimitPush = -1;
    public int DayLimitPush
    {
        get
        {
            if (_dayLimitPush == -1)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    _dayLimitPush = Convert.ToInt32("0" + ds.Tables[0].Rows[0]["dayLimitPush"].ToString());
                }
                else
                {
                    _dayLimitPush = 0;
                }
            }
            return _dayLimitPush;
        }
        set
        {
            _dayLimitPush = value;
        }
    }

    private int _exclusiveLead = -1;
    public int ExclusiveLead
    {
        get
        {
            if (_exclusiveLead == -1)
            {
                _exclusiveLead = Convert.ToInt32(ds.Tables[0].Rows[0]["ExclusiveLead"].ToString());
            }
            return _exclusiveLead;
        }
        set
        {
            _exclusiveLead = value;
        }
    }

    private int _exPercent = -1;
    public int ExPercent
    {
        get
        {
            if (_exPercent == -1)
            {
                _exPercent = Convert.ToInt32(ds.Tables[0].Rows[0]["ExPercent"].ToString());
            }
            return _exPercent;
        }
        set
        {
            _exPercent = value;
        }
    }


    private int _shabbathKeepping = -1;
    public int ShabbathKeepping
    {
        get
        {
            if (_shabbathKeepping == -1)
            {
                _shabbathKeepping = Convert.ToInt32(ds.Tables[0].Rows[0]["ShabbathKeepping"].ToString());
            }
            return _shabbathKeepping;
        }
        set
        {
            _shabbathKeepping = value;
        }
    }
    /*************************************************************************************************************/
    private int _perpostpay = -1;
    public int PerPostPay
    {
        get
        {
            if (_perpostpay == -1)
            {
                _perpostpay = Convert.ToInt32(ds.Tables[0].Rows[0]["perpostpay"].ToString());
            }
            return _perpostpay;
        }
        set
        {
            _perpostpay = value;
        }
    }

    private int _uniqpay = -1;
    public int UniqPay
    {
        get
        {
            if (_uniqpay == -1)
            {
                _uniqpay = Convert.ToInt32(ds.Tables[0].Rows[0]["uniqpay"].ToString());
            }
            return _uniqpay;
        }
        set
        {
            _uniqpay = value;
        }
    }

    private int _uniqperiod = -1;
    public int UniqPeriod
    {
        get
        {
            if (_uniqperiod == -1)
            {
                _uniqperiod = Convert.ToInt32(ds.Tables[0].Rows[0]["uniqperiod"].ToString());
            }
            return _uniqperiod;
        }
        set
        {
            _uniqperiod = value;
        }
    }

    private int _calldurationpay = -1;
    public int CallDurationPay
    {
        get
        {
            if (_calldurationpay == -1)
            {
                _calldurationpay = Convert.ToInt32(ds.Tables[0].Rows[0]["calldurationpay"].ToString());
            }
            return _calldurationpay;
        }
        set
        {
            _calldurationpay = value;
        }
    }

    private string _callArea = string.Empty;
    public string CallArea
    {
        get
        {
            if (_callArea == string.Empty)
            {
                _callArea = ds.Tables[0].Rows[0]["callArea"].ToString();
            }
            return _callArea;
        }
        set
        {
            _callArea = value;
        }
    }

    private string _callType = string.Empty;
    public string CallType
    {
        get
        {
            if (_callType == string.Empty)
            {
                _callType = ds.Tables[0].Rows[0]["callType"].ToString();
            }
            return _callType;
        }
        set
        {
            _callType = value;
        }
    }

    private Double _callCredit = -1;
    public Double CallCredit
    {
        get
        {
            if (_callCredit == -1)
            {
                _callCredit = Convert.ToDouble(ds.Tables[0].Rows[0]["callCredit"].ToString());
            }
            return _callCredit;
        }
        set
        {
            _callCredit = value;
        }
    }

    private int _callRank = -1;
    public int CallRank
    {
        get
        {
            if (_callRank == -1)
            {
                _callRank = Convert.ToInt32(ds.Tables[0].Rows[0]["callRank"].ToString());
            }
            return _callRank;
        }
        set
        {
            _callRank = value;
        }
    }

    private int _callDefaultRank = -1;
    public int CallDefaultRank
    {
        get
        {
            if (_callDefaultRank == -1)
            {
                _callDefaultRank = Convert.ToInt32(ds.Tables[0].Rows[0]["callDefaultRank"].ToString());
            }
            return _callDefaultRank;
        }
        set
        {
            _callDefaultRank = value;
        }
    }

    private Double _callPrice = -1;
    public Double CallPrice
    {
        get
        {
            if (_callPrice == -1)
            {
                _callPrice = Convert.ToDouble(ds.Tables[0].Rows[0]["callPrice"].ToString());
            }
            return _callPrice;
        }
        set
        {
            _callPrice = value;
        }
    }

    private int _account_type = -1;
    public int Account_type
    {
        get
        {
            if (_account_type == -1)
            {
                _account_type = Convert.ToInt32(ds.Tables[0].Rows[0]["account_type"].ToString());
            }
            return _account_type;
        }
        set
        {
            _account_type = value;
        }
    }

    

    private string _default_type = string.Empty;
    public string Default_type
    {
        get
        {
            if (_default_type == "")
            {
                _default_type = ds.Tables[0].Rows[0]["default_type"].ToString();
            }
            return _default_type;
        }
        set
        {
            _default_type = value;
        }
    }

    private string _default_area = string.Empty;
    public string Default_area
    {
        get
        {
            if (_default_area == "")
            {
                _default_area = ds.Tables[0].Rows[0]["default_area"].ToString();
            }
            return _default_area;
        }
        set
        {
            _default_area = value;
        }
    }

    private string _default_city = string.Empty;
    public string Default_city
    {
        get
        {
            if (_default_city == "")
            {
                _default_city = ds.Tables[0].Rows[0]["default_city"].ToString();
            }
            return _default_city;
        }
        set
        {
            _default_city = value;
        }
    }

    private int _show_on_homepage = -1;
    public int Show_on_homepage
    {
        get
        {
            if (_show_on_homepage == -1)
            {
                _show_on_homepage = Convert.ToInt32(ds.Tables[0].Rows[0]["show_on_homepage"].ToString());
            }
            return _show_on_homepage;
        }
        set
        {
            _show_on_homepage = value;
        }
    }

    private int _show_on_sidebar = -1;
    public int Show_on_sidebar
    {
        get
        {
            if (_show_on_sidebar == -1)
            {
                _show_on_sidebar = Convert.ToInt32(ds.Tables[0].Rows[0]["show_on_sidebar"].ToString());
            }
            return _show_on_sidebar;
        }
        set
        {
            _show_on_sidebar = value;
        }
    }

    private int _show_on_service_type = -1;
    public int Show_on_service_type
    {
        get
        {
            if (_show_on_service_type == -1)
            {
                _show_on_service_type = Convert.ToInt32(ds.Tables[0].Rows[0]["show_on_service_type"].ToString());
            }
            return _show_on_service_type;
        }
        set
        {
            _show_on_service_type = value;
        }
    }

    private int _show_on_area = -1;
    public int Show_on_area
    {
        get
        {
            if (_show_on_area == -1)
            {
                _show_on_area = Convert.ToInt32(ds.Tables[0].Rows[0]["show_on_area"].ToString());
            }
            return _show_on_area;
        }
        set
        {
            _show_on_area = value;
        }
    }

    private int _show_on_service_type_area = -1;
    public int Show_on_service_type_area
    {
        get
        {
            if (_show_on_service_type_area == -1)
            {
                _show_on_service_type_area = Convert.ToInt32(ds.Tables[0].Rows[0]["show_on_service_type_area"].ToString());
            }
            return _show_on_service_type_area;
        }
        set
        {
            _show_on_service_type_area = value;
        }
    }

    public ClientData(int uId)
    {
       // ds = BllNewLeads.Instance.New_get_userDetails(uId);
    }
    public ClientData()
	{
        if (UserId > 0)
        {
            //ds = BllNewLeads.Instance.New_get_userDetails(UserId);
        }
	}
    //public ActionResult AddNewClient()
    //{
    //    ActionResult result = BllNewLeads.Instance.New_addNewUser(FullName, UserName, Password, Phone, Email, CompanyName, ProviderType, 2, ParentId);

    //    return result;
    //}

    //public ActionResult UpdateClient()
    //{
    //    ActionResult actionResult = BllNewLeads.Instance.New_UpdateUser(UserId, FullName, UserName, Phone, Email, CompanyName, CompanyId, Address);
    //    return actionResult;
    //}

    //public ActionResult UpdateClientAll(int cUserId, string cFullName, string cUserName, string uPass, double leadCounter, string cPhone, string cEmail, string cCompanyName,
    //            string cCompanyId, string cAddress, string cSiteIds, string cProviderType, int cIsActive, 
    //            double delay, double commission, int parentId, int affiId, string note)
    //{
    //    ActionResult actionResult =  BllNewLeads.Instance.New_UpdateUser(cUserId, cFullName, cUserName, uPass, leadCounter, cPhone, cEmail, cCompanyName, cCompanyId, cAddress, cSiteIds, cProviderType, cIsActive, delay, commission, parentId, affiId, note, CanReport, SaleDelay, UserPermission);
    //    return actionResult;
    //}

    //public ActionResult UpdateClientAll(int cUserId, string cFullName, string cUserName, string uPass, double leadCounter, string cPhone, string cEmail, string cCompanyName,
    //            string cCompanyId, string cAddress, string cSiteIds, string cProviderType, int cIsActive,
    //            double delay, double commission, int parentId, int affiId, string note, string maskyoo, int pickOrPush, int dayLimitPush)
    //{
    //    ActionResult actionResult = BllNewLeads.Instance.New_UpdateUser(cUserId, cFullName, cUserName, uPass, leadCounter, cPhone, cEmail, cCompanyName, cCompanyId, cAddress, cSiteIds, cProviderType, cIsActive, delay, commission, parentId, affiId, note, CanReport, SaleDelay, UserPermission, maskyoo, pickOrPush, dayLimitPush);
    //    return actionResult;
    //}

    //public ActionResult UpdateClientAll(int cUserId, string cFullName, string cUserName, string uPass, double leadCounter, string cPhone, string cEmail, string cCompanyName,
    //            string cCompanyId, string cAddress, string cSiteIds, string cProviderType, int cIsActive,
    //            double delay, double commission, int parentId, int affiId, string note, string maskyoo, int pickOrPush, int dayLimitPush, int ExclusiveLead, int ExPercent)
    //{
    //    ActionResult actionResult = BllNewLeads.Instance.New_UpdateUser(cUserId, cFullName, cUserName, uPass, leadCounter, cPhone, cEmail, cCompanyName, cCompanyId, cAddress, cSiteIds, cProviderType, cIsActive, delay, commission, parentId, affiId, note, CanReport, SaleDelay, UserPermission, maskyoo, pickOrPush, dayLimitPush, ExclusiveLead, ExPercent);
    //    return actionResult;
    //}
    ///*
    //public ActionResult UpdateClientAll(int cUserId, string cFullName, string cUserName, string uPass, double leadCounter, string cPhone, string cEmail, string cCompanyName,
    //            string cCompanyId, string cAddress, string cSiteIds, string cProviderType, int cIsActive,
    //            double delay, double commission, int parentId, int affiId, string note, string maskyoo, int pickOrPush, int dayLimitPush, int ExclusiveLead, int ExPercent)
    //{
    //    ActionResult actionResult = BllNewLeads.Instance.New_UpdateUser(cUserId, cFullName, cUserName, uPass, leadCounter, cPhone, cEmail, cCompanyName, cCompanyId, cAddress, cSiteIds, cProviderType, cIsActive, delay, commission, parentId, affiId, note, CanReport, SaleDelay, UserPermission, maskyoo, pickOrPush, dayLimitPush, ExclusiveLead, ExPercent);
    //    return actionResult;
    //}*/

    //public ActionResult UpdateClientAll(int uId)
    //{
    //    ActionResult actionResult = BllNewLeads.Instance.New_UpdateUser(uId, FullName, UserName, Password, LeadCounter, Phone, Email, CompanyName, CompanyId, Address, SiteIds, ProviderType, IsActive, DelayLead, Commission, ParentId, AffiliateId, Note, CanReport, SaleDelay, UserPermission, Maskyoo, PickOrPush, DayLimitPush, ExclusiveLead, ExPercent, PerPostPay, UniqPay, UniqPeriod, CallDurationPay, CallArea, CallType, CallCredit, CallRank, CallDefaultRank, CallPrice, Belongs2UserId, Logo, Description, ShabbathKeepping, Account_type, Default_type, Default_area, Default_city, Show_on_homepage, Show_on_sidebar, Show_on_service_type, Show_on_area, Show_on_service_type_area, EndDate, ToH, FromH, SendType);
    //    return actionResult;
    //}

    //public void UpdateClientNote(int uId)
    //{
    //    BllNewLeads.Instance.New_UpdateUserNote(uId, Note);
    //}

    //public void UpdateSendAlert(int alertSatus)
    //{
    //    BllNewLeads.Instance.UpdateSendAlert(UserId,alertSatus);
    //}

    //public DataSet GetLastLogin()
    //{
    //    DataSet ds = DalNewLeads.Instance.getLastLogin(this.CUserId, this.UserName);
    //    return ds;
    //}

    //public DataSet getNumOfPayLeadsPerMonth()
    //{
    //    DataSet ds = DalNewLeads.Instance.getNumOfPayLeadsPerMonth(CUserId);
    //    return ds;
    //}

    //public DataSet getNumOfPayLeadsPerDay()
    //{
    //    DataSet ds = DalNewLeads.Instance.getNumOfPayLeadsPerDay(CUserId);
    //    return ds;
    //}

    //public DataSet getNumOfPayLeadsPerYear()
    //{
    //    DataSet ds = DalNewLeads.Instance.getNumOfPayLeadsPerYear(CUserId);
    //    return ds;
    //}   

    
}