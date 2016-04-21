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
        GUID = "5f852296-61b7-4364-bb16-3a1a9567b1db", 
        DisplayName = "Category page", 
        Description = "Category page for comparison")]
    //[SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    [AvailableContentTypes(
       Availability.Specific,
       Include = new[] { typeof(OrganisationalUnitPage) })]

    public class CategoryPage : StandardPage
    {
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual OrganisationalUnitListBlock OrganisationalUnitList { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

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

            //OrganisationalUnitList.PageTypeFilter = typeof(OrganisationalUnitPage).GetPageType();
            //OrganisationalUnitList.Recursive = true;
        }

        #endregion
    }
}