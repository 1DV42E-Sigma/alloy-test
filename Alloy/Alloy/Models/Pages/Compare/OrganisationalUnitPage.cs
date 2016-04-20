using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;

namespace Alloy.Models.Pages
{
    [SiteContentType(
        GroupName = "Compare",
        GUID = "56f6be6a-25b6-4459-a154-8a68b3af08f3", 
        DisplayName = "Organisational unit", 
        Description = "Organisational unit page")]
    //[SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-article.png")]
    [AvailableContentTypes(
     Availability.Specific,
     Include = new System.Type[] { })]

    public class OrganisationalUnitPage : StandardPage
    {
        [Display(GroupName = SystemTabNames.Content)]
        public virtual string Author { get; set; }

        [Display(GroupName = SystemTabNames.Content)]
        public virtual ContentArea RightContentArea { get; set; }

    }
    
}