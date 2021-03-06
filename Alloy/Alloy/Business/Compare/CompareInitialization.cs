﻿using System;
using System.Linq;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.BaseLibrary;
using Alloy.Models.Pages;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.DataAccess;
using EPiServer.Security;
using Alloy.Business.Initialization;
using EPiServer.DataAbstraction;
using System.Web.Routing;
using EPiServer.Web.Routing;
using EPiServer;
using Alloy.Helpers.Compare;

namespace Alloy.Business.Compare
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CompareInitialization : IInitializableModule
    {
        private const string ORGANISATIONAL_UNIT_FOLDER_NAME = "OrganisationalUnits";

        public void Initialize(InitializationEngine context)
        {
            DataFactory.Instance.CreatingPage +=Instance_CreatingPage;
            //DataFactory.Instance.CreatingContent += Instance_CreatingContent;

            DataFactory.Instance.SavingContent += Instance_SavingContent;

            /*
            var partialRouter = new BlogPartialRouter();

            RouteTable.Routes.RegisterPartialRouter<BlogStartPage, Category>(partialRouter);
            */
        }

        void Instance_PublishingPage(object sender, PageEventArgs e)
        {
            
        }

        void Instance_CreatedPage(object sender, PageEventArgs e)
        {
           
        }

        //Returns if we are doing an import or mirroring
        public bool IsImport()
        {
            return ContextCache.Current["CurrentITransferContext"] != null;
        }

        /*
         * When a page gets created lets see if it is a blog post and if so lets create the date page information for it
         */
        void Instance_SavingContent(object sender, ContentEventArgs e)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            PageData page = contentRepository.Get<PageData>(e.ContentLink);
            
            if (page != null &&
                string.Equals(page.PageTypeName, typeof(CategoryPage).GetPageType().Name, StringComparison.OrdinalIgnoreCase) &&
                page.Name != e.Content.Name)
            {
                //renaming name of the page!
                string oldName = page.Name;
                string newName = e.Content.Name;

                var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();
                var category = CategoryHelper.FindCompareCategory(categoryRepository, oldName);
                if (category != null)
                {
                    category = category.CreateWritableClone();
                    category.Name = newName;
                    categoryRepository.Save(category);
                }
                else
                {
                    CategoryHelper.SaveCompareCategory(categoryRepository, newName, newName);
                }
            }
        }

        void Instance_CreatingPage(object sender, PageEventArgs e)
        {
            if (this.IsImport() || e.Content == null)
            {
                return;
            }
            if (string.Equals(e.Page.PageTypeName, typeof(CompareStartPage).GetPageType().Name, StringComparison.OrdinalIgnoreCase))
            {
                //var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
                //CreateOrganisationalUnitsPage(contentRepository, e.Page.ContentLink);

                //var contentAssetHelper = ServiceLocator.Current.GetInstance<ContentAssetHelper>();
                //var contentAssetFolder = contentAssetHelper.GetOrCreateAssetFolder(e.Page.ContentLink);
            }
            else if (string.Equals(e.Page.PageTypeName, typeof(CategoryPage).GetPageType().Name, StringComparison.OrdinalIgnoreCase))
            {
                var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();
                var category = CategoryHelper.FindCompareCategory(categoryRepository, e.Page.Name);
                if (category == null)
                {
                    CategoryHelper.SaveCompareCategory(categoryRepository, e.Page.Name, e.Page.Name);
                }
            }
            else if (string.Equals(e.Page.PageTypeName, typeof(OrganisationalUnitPage).GetPageType().Name, StringComparison.OrdinalIgnoreCase))
            {
                var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();

                PageData page = contentRepository.Get<PageData>(e.Page.ParentLink);
                string categoryName = null;
                if (page is CategoryPage)
                {
                    categoryName = page.Name;
                    page = contentRepository.Get<PageData>(page.ParentLink);
                }

                PageData startPage = (page is CompareStartPage ? page : null);
                if (startPage != null)
                {
                    var ouFolderPage = GetOrganisationalUnitsPageRef(startPage, contentRepository);
                    var existingOuPage = (ouFolderPage != null ? contentRepository.GetChildren<OrganisationalUnitPage>(ouFolderPage).FirstOrDefault() : null);
                    if (existingOuPage == null)
                    {
                        e.Page.ParentLink = GetOrganisationalUnitsPageRef(startPage, contentRepository);

                        if (!String.IsNullOrWhiteSpace(categoryName))
                        {
                            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();
                            Category category = CategoryHelper.FindCompareCategory(categoryRepository, categoryName);
                            if (category == null)
                            {
                                category = CategoryHelper.SaveCompareCategory(categoryRepository, categoryName, categoryName);
                            }

                            //add category to the ou page
                            e.Page.Category.Add(category.ID);
                        }
                    }
                    else
                    {
                        //cancel, ou page already exists in this comparison content
                        e.CancelReason = "An organisational unit with the name " + e.Page.Name + " already exists. Please choose another name or edit the existing one.";
                        e.CancelAction = true;

                    }
                }
                else
                {
                    //cancel, no compare start page
                    e.CancelReason = "Could not find the compare start page";
                    e.CancelAction = true;
                }
            }
        }
        
        

        // in here we know that the page is a compare start page and now we must create the organisational unit page unless already created
        private static PageReference GetOrganisationalUnitsPageRef(PageData compareStart, IContentRepository contentRepository)
        {
            foreach (var current in contentRepository.GetChildren<PageData>(compareStart.ContentLink))
            {
                if (current.Name == ORGANISATIONAL_UNIT_FOLDER_NAME)
                {
                    return current.PageLink;
                }
            }
            return CreateOrganisationalUnitFolderPage(contentRepository, compareStart.ContentLink);
        }

        private static PageReference CreateOrganisationalUnitFolderPage(IContentRepository contentRepository, ContentReference parent)
        {
            OrganisationalUnitFolderPage defaultPageData = contentRepository.GetDefault<OrganisationalUnitFolderPage>(parent, typeof(OrganisationalUnitFolderPage).GetPageType().ID, LanguageSelector.AutoDetect().Language);
            defaultPageData.PageName = ORGANISATIONAL_UNIT_FOLDER_NAME;
            defaultPageData.URLSegment = UrlSegment.CreateUrlSegment(defaultPageData);
            return contentRepository.Save(defaultPageData, SaveAction.Publish, AccessLevel.Publish).ToPageReference();
        }

        /*
        // in here we know that the page is a blog start page and now we must create the date pages unless they are already created
        public PageReference GetDatePageRef(PageData compareStart, DateTime published, IContentRepository contentRepository)
        {

            foreach (var current in contentRepository.GetChildren<PageData>(compareStart.ContentLink))
            {
                if (current.Name == published.Year.ToString())
                {
                    PageReference result;
                    foreach (PageData current2 in contentRepository.GetChildren<PageData>(current.ContentLink))
                    {
                        if (current2.Name == published.Month.ToString())
                        {
                            result = current2.PageLink;
                            return result;
                        }
                    }
                    result = CreateDatePage(contentRepository, current.PageLink, published.Month.ToString(), new DateTime(published.Year, published.Month, 1));
                    return result;
            
                }
            }
            PageReference parent = CreateDatePage(contentRepository, compareStart.ContentLink, published.Year.ToString(), new DateTime(published.Year, 1, 1));
            return CreateDatePage(contentRepository, parent, published.Month.ToString(), new DateTime(published.Year, published.Month, 1));     
        }

        private PageReference CreateDatePage(IContentRepository contentRepository, ContentReference parent, string name, DateTime startPublish)
        {
            BlogListPage defaultPageData = contentRepository.GetDefault<BlogListPage>(parent, typeof(BlogListPage).GetPageType().ID, LanguageSelector.AutoDetect().Language);
            defaultPageData.PageName = name;
            defaultPageData.Heading = name;
            defaultPageData.StartPublish = startPublish;
            defaultPageData.URLSegment = UrlSegment.CreateUrlSegment(defaultPageData);
            return contentRepository.Save(defaultPageData, SaveAction.Publish, AccessLevel.Publish).ToPageReference();
        }
        */

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            DataFactory.Instance.CreatingPage -= Instance_CreatingPage;
            DataFactory.Instance.SavingContent -= Instance_SavingContent;

        }
    }
}