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

namespace Alloy.Controllers
{
    public class CategoryListBlockController : BlockController<CategoryListBlock>
    {
        public int PreviewTextLength { get; set; }
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public CategoryListBlockController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public override ActionResult Index(CategoryListBlock currentBlock)
        {

            var category = Request.RequestContext.GetCustomRouteData<Category>("category");

            var blogs = FindPages(currentBlock, category);

            blogs = Sort(blogs, currentBlock.SortOrder);

            if(currentBlock.Count > 0)
            {
                blogs = blogs.Take(currentBlock.Count);
            }


            var model = new CategoryListModel(currentBlock)
                {
                    Categories = blogs, 
                    Heading = category != null ? "Category tags for post: "+category.Name : string.Empty
                };

            return PartialView(model);
        }

        public ActionResult Preview(PageData currentPage, CategoryListModel blogModel)
        {
            var pd = (CategoryPage)currentPage;
            PreviewTextLength = 200;

            var model = new CategoryPageModel(pd)
            {
                PreviewText = GetPreviewText(pd),
                ShowIntroduction = blogModel.ShowIntroduction,
                ShowPublishDate = blogModel.ShowPublishDate
            };

            return PartialView("Preview", model);
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


        private IEnumerable<PageData> FindPages(CategoryListBlock currentBlock, Category category)
        {
            var pageRouteHelper = ServiceLocator.Current.GetInstance<PageRouteHelper>();
            PageData currentPage = pageRouteHelper.Page ?? contentLoader.Get<PageData>(ContentReference.StartPage);

            var listRoot = currentBlock.Root ?? currentPage.ContentLink.ToPageReference();

            IEnumerable<PageData> pages;
            
            if (currentBlock.Recursive)
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    pages = contentLocator.FindPagesByPageType(listRoot, true, currentBlock.PageTypeFilter.ID);
                }
                else
                {
                    pages = contentLocator.GetAll<PageData>(listRoot);
                }
            }
            else
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    pages = contentLoader.GetChildren<PageData>(listRoot)
                        .Where(p => p.PageTypeID == currentBlock.PageTypeFilter.ID);
                }
                else
                {
                    pages = contentLoader.GetChildren<PageData>(listRoot);
                }
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                pages = pages.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
            }
            else if(category != null)
            {
                var catlist = new CategoryList();
                catlist.Add(category.ID);

                pages = pages.Where(x => x.Category.Intersect(catlist).Any());
            }
            pages = pages.Where(p => p.PageTypeName == typeof(CategoryPage).GetPageType().Name).ToList();
            return pages;
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
