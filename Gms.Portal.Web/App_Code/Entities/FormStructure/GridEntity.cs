using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Grid")]
    public class GridEntity : ContainerControlEntity
    {
        [XmlAttribute("DetailsFormID")]
        public Guid DetailsFormID { get; set; }
    }
}