using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel;
using EPiServer.Filters;

namespace Alloy.Models.Blocks
{
    [SiteContentType(
        GUID = "fa50f9a5-6b51-4422-95cd-384a4c435025", 
        DisplayName = "Organisational Unit List Block")]
    [SiteImageUrl]

    public class OrganisationalUnitListBlock : SiteBlockData
    {
        /*
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 2, Name = "Include Publish Date")]
        [DefaultValue(false)]
        public virtual bool IncludePublishDate { get; set; }

        /// <summary>
        /// Gets or sets whether a page introduction/description should be included in the list
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 3, Name = "Include Introduction")]
        [DefaultValue(true)]
        public virtual bool IncludeIntroduction { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 4)]
        [DefaultValue(3)]
        public virtual int Count { get; set; }

        */

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 4, Name = "Sort Order")]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        public virtual FilterSortOrder SortOrder { get; set; }


        /*
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 5)]
        public virtual PageReference Root { get; set; }
        
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 7, Name = "Category Filter")]
        public virtual CategoryList CategoryFilter { get; set; }
        
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 6, Name = "Page Type Filter")]
        public virtual PageType PageTypeFilter { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 8)]
        public virtual bool Recursive { get; set; }
        */


        #region IInitializableContent

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            /*
            Count = 3;
            IncludeIntroduction = true;
            IncludePublishDate = true;
            */
            SortOrder = FilterSortOrder.PublishedDescending;
            //Recursive = true;
        }

        #endregion
    }
}