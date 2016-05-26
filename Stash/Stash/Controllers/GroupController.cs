using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using Services;

namespace Stash.Controllers
{
    public class GroupController : Controller
    {
        private GroupServices Service = new GroupServices();

        public ActionResult Index()
        {
            return View(Service.GroupIndex());
        }

        public ActionResult CreateGroup() {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult CreateGroup(Itemgroup Group) {
            throw new NotImplementedException();
        }

        public ActionResult DeleteGroup() {
            throw new NotImplementedException();
        }

        public ActionResult AddItemToGroup() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates both models in the database
        /// </summary>
        /// <param name="Group"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddItemToGroup(Itemgroup Group, Item Item)
        {
            throw new NotImplementedException();
        }
    }
}