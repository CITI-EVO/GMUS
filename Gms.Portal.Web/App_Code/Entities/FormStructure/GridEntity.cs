using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("Grid")]
    public class GridEntity : ContentEntity
    {
        [XmlElement("DetailsFormID")]
        public Guid? DetailsFormID { get; set; }

        [XmlElement("ValidationExp")]
        public String ValidationExp { get; set; }

        [XmlElement("ErrorMessage")]
        public String ErrorMessage { get; set; }

    }
}