using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Alloy.Models.Blocks
{
    [ContentType(DisplayName = "DefaultBlock1", GUID = "4cd1187c-6933-4178-9018-1a49b1e4a71a", Description = "")]
    public class DefaultBlock1 : BlockData
    {
/*
        [CultureSpecific]
        [Display(
            Name = "Name",
            Description = "Name field's description",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Name { get; set; }
 */
    }
}