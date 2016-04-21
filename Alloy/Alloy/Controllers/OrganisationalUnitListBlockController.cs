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
using EPiServer.DataAbstraction;
using Alloy.Models.Pages;
using Alloy.Business.Tags;
using System.Text;
using System;
using System.Text.RegularExpressions;
using EPiServer.Core.Html;
using EPiServer.DynamicContent;
using EPiServer;
using Alloy.Business.Compare;
using Alloy.Helpers.Compare;

namespace Alloy.Controllers
{
    public class OrganisationalUnitListBlockController : BlockController<OrganisationalUnitListBlock>
    {
        public int PreviewTextLength { get; set; }
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public OrganisationalUnitListBlockController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public override ActionResult Index(OrganisationalUnitListBlock currentBlock)
        {
            //var category = Request.RequestContext.GetCustomRouteData<Category>("category");

            var organisationalUnits = FindPages(currentBlock); //, category);
            organisationalUnits = Sort(organisationalUnits, currentBlock.SortOrder);

            /*
            if(currentBlock.Count > 0)
            {
                organisationalUnits = organisationalUnits.Take(currentBlock.Count);
            }
            */

            var model = new OrganisationalUnitListModel(currentBlock)
                {
                    OrganisationalUnits = organisationalUnits 
                    //Heading = string.Empty //category != null ? "Category tags for post: "+category.Name : string.Empty
                };

            return PartialView(model);
        }

        public ActionResult Preview(PageData currentPage, OrganisationalUnitListModel organisationalUnitListModel)
        {
            var pd = (OrganisationalUnitPage)currentPage;
            PreviewTextLength = 200;

            var model = new OrganisationalUnitPageModel(pd)
            {
                Categories = CategoryHelper.GetCategoryViewModels(pd),
                PreviewText = GetPreviewText(pd),
                //ShowIntroduction = organisationalUnitListModel.ShowIntroduction,
                //ShowPublishDate = organisationalUnitListModel.ShowPublishDate
            };

            return PartialView("Preview", model);
        }
        


        protected string GetPreviewText(OrganisationalUnitPage page)
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

        private PageData GetCompareStartPage(PageData currentPage)
        {
            if (currentPage.PageTypeName == typeof(CompareStartPage).GetPageType().Name)
            {
                return currentPage;
            }
            if (currentPage.ParentLink != null)
            {
                return GetCompareStartPage(contentLoader.Get<PageData>(currentPage.ContentLink)); //DataFactory.Instance.GetPage(currentPage.ParentLink));
            }
            return null;
        }
        private IEnumerable<PageData> FindPages(OrganisationalUnitListBlock currentBlock) //, Category categoryParameter)
        {
            IEnumerable<PageData> pages = null;

            var pageRouteHelper = ServiceLocator.Current.GetInstance<PageRouteHelper>();
            PageData currentPage = pageRouteHelper.Page ?? contentLoader.Get<PageData>(ContentReference.StartPage);

            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();
            var category = CategoryHelper.FindCompareCategory(categoryRepository, currentPage.Name);

            if (category != null)
            {
                //var listRoot = currentBlock.Root ?? currentPage.ContentLink.ToPageReference();
                var compareStartPage = contentLoader.Get<CompareStartPage>(currentPage.ParentLink); //GetCompareStartPage(currentPage);


                if (compareStartPage != null)
                {
                    var ouFolderPage = contentLoader.GetChildren<OrganisationalUnitFolderPage>(compareStartPage.ContentLink).FirstOrDefault();
                    if (ouFolderPage != null)
                    {
                        var ouPages = contentLoader.GetChildren<OrganisationalUnitPage>(ouFolderPage.ContentLink).Where(o => o.Category.Contains(category.ID)).ToList();
                        if (ouPages != null && ouPages.Count > 0)
                        {
                            pages = ouPages;
                        }
                    }
                    //contentLoader.GetChildren<PageData>(compareStartPage.ContentLink.ToPageReference());
                    //PageReference ouPage = CompareInitialization.GetOrganisationalUnitsPageRef(compareStartPage, contentRepository);

                }
            }

            return pages ?? new List<PageData>();
        }

        private IEnumerable<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection;
        }

      
    }
}
