using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Container.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetItems(string q)
        {
            meichalimEntities db = new meichalimEntities();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                var accounts = db.tbl_items.Where(x => x.ItemName.ToLower().StartsWith(q.ToLower())||x.ItemID.StartsWith(q)).Select(x=> new {id= x.ItemID,text= x.ItemName});
              //  string[] arr = accounts.ToArray();
                return Json(accounts, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GetClients(string q)
        {
            meichalimEntities db = new meichalimEntities();
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                var clients = db.tbl_clients.Where(x => x.CliientName.ToLower().StartsWith(q.ToLower())|| x.ClientId.ToString().StartsWith(q)).Select(x => new { id = x.ClientId, text = x.CliientName });
                //  string[] arr = accounts.ToArray();
                return Json(clients, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GetItemDetails(string id)
        {
            meichalimEntities db = new meichalimEntities();
            if (id!="0" && !string.IsNullOrEmpty(id))
            {
                var item = db.tbl_items.Find(id);
                //  string[] arr = accounts.ToArray();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult GetClientDetails(int id)
        {
            meichalimEntities db = new meichalimEntities();
            if (id != 0)
            {
                var client = db.tbl_clients.Find(id);
                //  string[] arr = accounts.ToArray();
                return Json(client, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        public ActionResult SearchForItem()
        {
            meichalimEntities db = new meichalimEntities();
            //  Dictionary<string, string> items = new Dictionary<string, string>();
            List<tbl_items> items = db.tbl_items.OrderBy(x => x.ItemID).ToList(); //.ToDictionary(x=>x.ItemID,x=>x.ItemName);
            return View(items);
        }

        public ActionResult SearchForClient()
        {
            meichalimEntities db = new meichalimEntities();
            List<tbl_clients> items = db.tbl_clients.OrderBy(x => x.CliientName).ToList();
            return View(items);
        }

        public ActionResult SearchBySizes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SearchBySizes(FormCollection frmColl)
        {
            meichalimEntities db = new meichalimEntities();
            int height = Convert.ToInt32(frmColl["height"]);
            int width = Convert.ToInt32(frmColl["width"]);
            int length = Convert.ToInt32(frmColl["length"]);
            decimal per = Convert.ToDecimal(frmColl["per"]);
            bool existInStock = frmColl["inStock"] == "on" ? true : false;
            bool includeClose = frmColl["includeClose"] == "on" ? true:false;
            bool withoutPrint = frmColl["withoutPrint"] == "on" ? true:false;
            //קיים במלאי צריך לכלול רק את הפריטים שהשדה הזה גדול מ-0
            //כולל סגורים - לכלול את אלה שיש להם 9 בשדה מאפיינים אחרת לא לכלול אותם.
            //ללא הדפסה -רק את אלו שיש להם * בשם שלהם, אחרת ללא הבחנה
            decimal heightPlus = height * (1 + per / 100);
            decimal heightMinus = height * (1 - per / 100);
            decimal widthPlus = width * (1 + per / 100);
            decimal widthMinus = width * (1 - per / 100);
            decimal lengthPlus = length * (1 + per / 100);
            decimal lengthMinus = length * (1 - per / 100);
            List<tbl_items> items = db.tbl_items.Where(x => x.Hight >= heightMinus && x.Hight <= heightPlus && x.Length >= lengthMinus && x.Length <= lengthPlus  && x.width >= widthMinus && x.width <= widthPlus).ToList();
            if (existInStock)
                items = items.Where(x => x.StockAmount!=null && x.StockAmount > 0).ToList();
            if (!includeClose)
                items = items.Where(x => x.attribute != "9").ToList();
            if (withoutPrint)
                items = items.Where(x => !x.ItemName.Contains("*")).ToList();
            List<ItemsBySizes> listBySizes = new List<ItemsBySizes>();
            foreach(tbl_items item in items)
            {
                ItemsBySizes ibs = new ItemsBySizes(item);
                ibs.differential = Math.Abs(height - (item.Hight!=null? item.Hight.Value:0))+ Math.Abs(width - (item.width != null ? item.width.Value : 0)) + Math.Abs(length - (item.Length != null ? item.Length.Value : 0));
                listBySizes.Add(ibs);
            }
            listBySizes = listBySizes.OrderBy(x => x.differential).ToList();
            ViewBag.items = listBySizes;
            ViewBag.height = height;
            ViewBag.width = width;
            ViewBag.length = length;
            ViewBag.per = per;
            ViewBag.existInStock = existInStock ? 1 : 0;
            ViewBag.includeClose = includeClose ? 1 : 0;
            ViewBag.withoutPrint = withoutPrint ? 1 : 0;
            return View();
        }

        public ActionResult SearchBySimilarItem()
        {
            meichalimEntities db = new meichalimEntities();
            List<tbl_items> items = db.tbl_items.OrderBy(x => x.ItemID).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult SearchBySimilarItem(FormCollection frmColl)
        {
            meichalimEntities db = new meichalimEntities();
            string itemId = frmColl["item"];
            tbl_items item = db.tbl_items.Find(itemId);
            decimal per = Convert.ToDecimal(frmColl["per"]);
            bool existInStock = frmColl["inStock"] == "on" ? true : false;
            bool includeClose = frmColl["includeClose"] == "on" ? true : false;
            bool withoutPrint = frmColl["withoutPrint"] == "on" ? true : false;
            //קיים במלאי צריך לכלול רק את הפריטים שהשדה הזה גדול מ-0
            //כולל סגורים - לכלול את אלה שיש להם 9 בשדה מאפיינים אחרת לא לכלול אותם.
            //ללא הדפסה -רק את אלו שאין להם * בשם שלהם, אחרת ללא הבחנה
            decimal heightPlus = item.Hight!=null? item.Hight.Value * (1 + per / 100) : 0;
            decimal heightMinus = item.Hight != null ? item.Hight.Value * (1 - per / 100) : 0;
            decimal widthPlus = item.width!=null? item.width.Value * (1 + per / 100):0;
            decimal widthMinus = item.width != null ? item.width.Value * (1 - per / 100) : 0;
            decimal lengthPlus = item.Length!=null? item.Length.Value * (1 + per / 100):0;
            decimal lengthMinus = item.Length!=null? item.Length.Value * (1 - per / 100):0;
            List<tbl_items> items = db.tbl_items.Where(x => x.Hight >= heightMinus && x.Hight <= heightPlus && x.Length >= lengthMinus && x.Length <= lengthPlus && x.width >= widthMinus && x.width <= widthPlus).ToList();
            if (existInStock)
                items = items.Where(x => x.StockAmount != null && x.StockAmount > 0).ToList();
            if (!includeClose)
                items = items.Where(x => x.attribute != "9").ToList();
            if (withoutPrint)
                items = items.Where(x => !x.ItemName.Contains("*")).ToList();
            List<ItemsBySizes> listBySizes = new List<ItemsBySizes>();
            foreach (tbl_items i in items)
            {
                ItemsBySizes ibs = new ItemsBySizes(i);
                ibs.differential = Math.Abs((item.Hight!=null?item.Hight.Value:0) - (i.Hight != null ? i.Hight.Value : 0)) + Math.Abs((item.width!=null?item.width.Value:0) - (i.width != null ? i.width.Value : 0)) + Math.Abs((item.Length!=null?item.Length.Value:0) - (i.Length != null ? i.Length.Value : 0));
                listBySizes.Add(ibs);
            }
            listBySizes = listBySizes.OrderBy(x => x.differential).ToList();
            ViewBag.items = listBySizes;
            ViewBag.itemID = itemId;
            ViewBag.per = per;
            ViewBag.existInStock = existInStock ? 1 : 0;
            ViewBag.includeClose = includeClose ? 1 : 0;
            ViewBag.withoutPrint = withoutPrint ? 1 : 0;
            return View(db.tbl_items);
        }

        public class ItemsBySizes
        {
            public tbl_items item { get; set; }
            public decimal differential { get; set; }

            public ItemsBySizes(tbl_items item)
            {
                this.item = item;
            }
        }
    }
}