﻿using System.Collections.Generic;
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
using Alloy.Business.Tags;
using System.Web;
using EPiServer;

namespace Alloy.Controllers
{
    public class TagCloudBlockController : BlockController<TagCloudBlock>
    {
        private ContentLocator contentLocator;
        private IContentLoader contentLoader;
        public TagCloudBlockController(ContentLocator contentLocator, IContentLoader contentLoader)
        {
            this.contentLocator = contentLocator;
            this.contentLoader = contentLoader;
        }

        public override ActionResult Index(TagCloudBlock currentBlock)
        {
            var model = new TagCloudModel(currentBlock)
            {
                Tags = GetTags(currentBlock.BlogTagLinkPage)
            };
         
            return PartialView(model);
        }

        public IEnumerable<TagItem> GetTags(PageReference startTagLink)
        {
            List<TagItem> tags = new List<TagItem>();

            foreach (var item in TagRepository.Instance.LoadTags())
            {
                Category cat = Category.Find(item.TagName);
                string url = string.Empty;

                if (startTagLink != null)
                {
                    url = TagFactory.Instance.GetTagUrl(contentLoader.Get<PageData>(startTagLink.ToPageReference()), cat);
                }

                tags.Add(new TagItem() { Count = item.Count, TagName = item.TagName, Weight = item.Weight, Url = url });
            }
            return tags;
        }

    }
}
