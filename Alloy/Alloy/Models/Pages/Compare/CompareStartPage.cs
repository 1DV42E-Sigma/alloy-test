using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Alloy.Models.Blocks;
using Alloy.Business;

namespace Alloy.Models.Pages
{
    [SiteContentType(
        GroupName = "Compare",
        GUID = "8f421f96-a21b-4946-bbc1-046c914a9ad7", 
        DisplayName = "Compare Start", 
        Description = "Compare Start Page with categories")]
    //[SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    [AvailableContentTypes(
        Availability.Specific,
        Include = new[] { typeof(CategoryPage), typeof(OrganisationalUnitFolderPage) })]  // Pages we can create under the start page...
  
    public class CompareStartPage : StandardPage
    {
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual CategoryListBlock CategoryList { get; set; }

        /*
        [Display(GroupName = SystemTabNames.Content)]
        public virtual TagCloudBlock TagCloud { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual BlogArchiveBlock Archive { get; set; }
        */

        [Display(GroupName = SystemTabNames.Content)]
        public virtual ContentArea RightContentArea { get; set; }


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