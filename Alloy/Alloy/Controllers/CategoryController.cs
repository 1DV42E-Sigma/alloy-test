using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Search;
using Alloy.Business;
using Alloy.Models.Pages;
using Alloy.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Hosting;
using EPiServer.Web.Mvc.Html;
using EPiServer.DataAbstraction;
using System;
using System.Text;
using System.Text.RegularExpressions;
using EPiServer.Core.Html;
using EPiServer.DynamicContent;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Alloy.Business.Tags;
using EPiServer.Web.Mvc;

namespace Alloy.Controllers
{
    public class CategoryController : Controller
    {
        public int PreviewTextLength { get; set; }

        public ActionResult Preview(PageData currentPage, CategoryListModel categoryModel)
        {
            var pd = (CategoryPage)currentPage;
            PreviewTextLength = 200;

            var model = new CategoryPageModel(pd)
            {
                PreviewText = GetPreviewText(pd),
                //ShowIntroduction = categoryModel.ShowIntroduction,
                //ShowPublishDate = categoryModel.ShowPublishDate
            };

            return PartialView("Preview", model);
        }

        public ActionResult Full(CategoryPage currentPage)
        {
            PreviewTextLength = 200;

            var model = new CategoryPageModel(currentPage)
            {
                //Category = currentPage.Category,
                PreviewText = GetPreviewText(currentPage),
                MainBody = currentPage.MainBody,
                StartPublish = currentPage.StartPublish
            };

            var editHints = ViewData.GetEditHints<CategoryPageModel, CategoryPage>();
            //editHints.AddConnection(m => m.Category, p => p.Category);
            //editHints.AddFullRefreshFor(p => p.Category);
            editHints.AddFullRefreshFor(p => p.StartPublish);
            
           

            return PartialView("Full", model);
        }

        public ActionResult Index(CategoryPage currentPage)
        {
             var model = PageViewModel.Create(currentPage);

          
                //Connect the view models logotype property to the start page's to make it editable
                var editHints = ViewData.GetEditHints<PageViewModel<CategoryPage>, CategoryPage>();
                editHints.AddConnection(m => m.CurrentPage.Category, p => p.Category);
                editHints.AddConnection(m => m.CurrentPage.StartPublish, p => p.StartPublish);


            return View(model);
        }
        
        protected string GetPreviewText(CategoryPage page)
        {
            if (PreviewTextLength <= 0)
            {
                return string.Empty;
            }

            string previewText = String.Empty;

            if (page.MainBody != null)
            {
                previewText = page.MainBody.ToHtmlString();
            }

            if (String.IsNullOrEmpty(previewText))
            {
                return string.Empty;
            }

            //If the MainBody contains DynamicContents, replace those with an empty string
            StringBuilder regexPattern = new StringBuilder(@"<span[\s\W\w]*?classid=""");
            regexPattern.Append(DynamicContentFactory.Instance.DynamicContentId.ToString());
            regexPattern.Append(@"""[\s\W\w]*?</span>");
            previewText = Regex.Replace(previewText, regexPattern.ToString(), string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return TextIndexer.StripHtml(previewText, PreviewTextLength);
        }

    
    }
}
