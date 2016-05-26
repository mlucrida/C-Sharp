using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlTypes;
using Entities;
using Services;
using System.Net;

namespace Stash.Controllers
{
    public class ItemController : Controller
    {
        //private DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
        //private StashEntities db = new StashEntities();
        private ItemServices Service = new ItemServices();

        /// <summary>
        /// View of all items. 
        /// Create/edit/delete from this page.
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns></returns>
        public ActionResult Index(string searchString)
        {
            return View(Service.GetDetails(searchString));
        }

        /// <summary>
        /// Create Item Page
        /// </summary>
        /// <author>Matt Lucrida and Daniel Sacdalan</author>
        /// <returns></returns>
        public ActionResult CreateItem()
        {
            var newItem = Service.InitializeItem();
            return View(newItem);
        }

        /// <summary>
        /// Saves item add to db.
        /// </summary>
        /// <author>Matt Lucrida and Daniel Sacdalan</author>
        /// <param name="newItem"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateItem([Bind(Include = "RFIDkey,name,object_description,itemgroupkey,photokey,create_date,delete_date")]Item newItem)
        {
            if(ModelState.IsValid)
            {
                // service path seems redundant :/
                Service.AddNewItem(newItem);
                return RedirectToAction("Index");
            }
            return RedirectToAction("CreateItem");
        }

        /// <summary>
        /// This also includes adding photos
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateItem(int id)
        {
            //if (id == null) {
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Item Item = Service.GetItem(id);
            return View(Item);
        }

        [HttpPost]
        public ActionResult UpdateItem([Bind(Include = "itemkey,RFIDkey,name,object_description,itemgroupkey,photokey,create_date,delete_date")]Item Item)
        {
            if (ModelState.IsValid)
            {
                Service.UpdateItem(Item);
                return RedirectToAction("Index");
            }
            return View(Item);
        }

        public ActionResult DeleteItem(int id) {
            Item Item = Service.GetItem(id);
            return View(Item);
        }

        [HttpPost]
        public ActionResult DeleteItem([Bind(Include = "itemkey,RFIDkey,name,object_description,itemgroupkey,photokey,create_date,delete_date")]Item Item)
        {
            if (ModelState.IsValid)
            {
                Service.DeleteItem(Item);
                return RedirectToAction("Index");
            }
            return View(Item);
        }

    }
}