using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.ServiseStructure
{
    [Serializable]
    [XmlRoot("Method")]
    public class ReturnEntity
    {
        [XmlElement("Type")]
        public String Type { get; set; }

        [XmlElement("IsPrimitive")]
        public bool IsPrimitive { get; set; }
       
    }
}