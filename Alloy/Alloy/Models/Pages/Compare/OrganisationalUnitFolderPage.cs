using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Alloy.Models.Pages
{
    [SiteContentType(
        GroupName = "Compare",
        GUID = "2a9d3fd5-627a-4503-b026-04ab66f76439",
        DisplayName = "OrganisationalUnitFolderPage",
        Description = "A folder page containing all organisational unit pages for the compare")]
    //[SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(OrganisationalUnitPage) })]  // Pages we can create under the start page...

    public class OrganisationalUnitFolderPage : StandardPage
    {
        /*
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual CategoryListBlock CategoryList { get; set; }

        
        //[Display(GroupName = SystemTabNames.Content)]
        //public virtual TagCloudBlock TagCloud { get; set; }

        //[Display(GroupName = SystemTabNames.Content)]
        //public virtual BlogArchiveBlock Archive { get; set; }
        

        [Display(GroupName = SystemTabNames.Content)]
        public virtual ContentArea RightContentArea { get; set; }
        */

        #region IInitializableContent

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            //CategoryList.PageTypeFilter = typeof(CategoryPage).GetPageType();
            //CategoryList.Recursive = true;
        }

        #endregion
    }
}