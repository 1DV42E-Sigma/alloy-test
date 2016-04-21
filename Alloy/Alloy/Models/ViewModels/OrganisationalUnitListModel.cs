using System.Collections.Generic;
using EPiServer.Core;
using Alloy.Models.Blocks;

namespace Alloy.Models.ViewModels
{
    public class OrganisationalUnitListModel
    {
        public OrganisationalUnitListModel()
        {
            //Empty
        }
        public OrganisationalUnitListModel(OrganisationalUnitListBlock block)
        {
            Heading = block.Heading;
            ShowIntroduction = block.IncludeIntroduction;
            ShowPublishDate = block.IncludePublishDate;
        }
        public string Heading { get; set; }
        public IEnumerable<PageData> OrganisationalUnits { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
    }
}