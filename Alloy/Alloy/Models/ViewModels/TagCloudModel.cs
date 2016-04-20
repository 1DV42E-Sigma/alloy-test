using System.Collections.Generic;
using EPiServer.Core;
using Alloy.Models.Blocks;

namespace Alloy.Models.ViewModels
{
    public class TagCloudModel
    {
        public TagCloudModel(TagCloudBlock block)
        {
            Heading = block.Heading;    
        }

        public string Heading { get; set; }

        public IEnumerable<BlogItemPageModel.TagItem> Tags { get; set; }

    }
}