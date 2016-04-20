using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Filters;
using Alloy.Business;
using Alloy.Models.Blocks;
using Alloy.Models.ViewModels;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using EPiServer.ServiceLocation;
using Alloy.Models.Pages;
using EPiServer.Web;
using EPiServer.DataAbstraction;
using EPiServer;

namespace Alloy.Controllers
{
    public class BlogStartPageController : PageControllerBase<BlogStartPage>
    {
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public BlogStartPageController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public ActionResult Index(BlogStartPage currentPage)
        {

            var model = new PageViewModel<BlogStartPage>(currentPage);

         

            return View(model);
        }


    }
}
