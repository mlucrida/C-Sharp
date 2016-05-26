using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Entities;
using Services;
using System.Net.Sockets;

namespace Stash.Controllers
{
    public class HomeController : Controller
    {
        private BoxServices Service = new BoxServices();

        /// <summary>
        /// View of all box's contents
        /// </summary>
        /// <author>Daniel Sacdalan and Matt Lucrida</author>
        /// <returns></returns>
        public async Task<ActionResult> Index(string searchString)
        {
            DataAccess.DataAccess dataAccess = new DataAccess.DataAccess();
            JsonBoxData CurrentItems = new JsonBoxData();

            CurrentItems = await dataAccess.ScanBoxes(); // How are the box keys being assigned?
            
            if (CurrentItems != null)
                dataAccess.ScanStash(CurrentItems);

            return View(Service.BoxIndex(searchString));
        }

        /// <summary>
        /// Contact page.
        /// </summary>
        /// <author>Daniel Sacdalan</author>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult SearchBox()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// (Req 2.7: search box by name)
        /// I think this can be somehow integrated with the index view.
        /// still figuring that bit out, but for now we'll work with this.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchBox(string BoxName)
        {
            throw new NotImplementedException();
        }
    }
}