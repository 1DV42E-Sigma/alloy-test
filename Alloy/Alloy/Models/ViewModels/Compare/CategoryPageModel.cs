using System.Collections.Generic;
using System.Web;
using Alloy.Models.Pages;
using EPiServer.Core;
using System;

namespace Alloy.Models.ViewModels
{
    public class CategoryPageModel : PageViewModel<CategoryPage>
    {
        public CategoryPageModel(CategoryPage currentPage)
            : base(currentPage)
        {}

        //public IEnumerable<TagItem> Tags { get; set; }
     
        public string PreviewText { get; set; }
        public DateTime StartPublish { get; set; }
        public XhtmlString MainBody { get; set; }

        /*
        public bool ShowPublishDate { get; set; }

        public bool ShowIntroduction { get; set; }
        
        public CategoryList Category { get; set; }
        */

        /*
        public class TagItem
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }
        */
    }
}