using System;
using System.Xml.Serialization;

namespace Gms.Portal.Web.Entities.FormStructure
{
    [Serializable]
    [XmlRoot("NameValue")]
    public class NameValueEntity
    {
        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Value")]
        public String Value { get; set; }
    }
}