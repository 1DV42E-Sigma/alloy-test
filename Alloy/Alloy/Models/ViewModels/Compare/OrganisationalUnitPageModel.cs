using System.Collections.Generic;
using System.Web;
using Alloy.Models.Pages;
using EPiServer.Core;
using System;

namespace Alloy.Models.ViewModels
{
    public class OrganisationalUnitPageModel : PageViewModel<OrganisationalUnitPage>
    {
        public OrganisationalUnitPageModel(OrganisationalUnitPage currentPage)
            : base(currentPage)
        {}

        public IEnumerable<CategoryItemModel> Categories { get; set; }
     
        public string PreviewText { get; set; }
        public DateTime StartPublish { get; set; }
        public XhtmlString MainBody { get; set; }

        //public bool ShowPublishDate { get; set; }
        //public bool ShowIntroduction { get; set; }

        public CategoryList Category { get; set; }

    }
}