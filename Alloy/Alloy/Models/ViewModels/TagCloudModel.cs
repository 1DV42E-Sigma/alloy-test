using System.Collections.Generic;
using EPiServer.Core;
using Alloy.Models.Blocks;
using Alloy.Business.Tags;

namespace Alloy.Models.ViewModels
{
    public class TagCloudModel
    {
        public TagCloudModel(TagCloudBlock block)
        {
            Heading = block.Heading;    
        }

        public string Heading { get; set; }

        public IEnumerable<TagItem> Tags { get; set; }

    }
}