using Alloy.Business;
using Alloy.Models.Pages;
using Alloy.Models.ViewModels;
using EPiServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Alloy.Controllers
{
    public class CompareStartPageController : Controller
    {
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public CompareStartPageController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public ActionResult Index(CompareStartPage currentPage)
        {
            var model = new PageViewModel<CompareStartPage>(currentPage);
            return View(model);
        }
        
    }
}