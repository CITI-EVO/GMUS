using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response;

namespace CITI.EVO.CommonData.Web.Helpers.GovTalk
{
    public static class GovTalkApiClient
    {
        public static PersonInfo GetPersonInfo(string privateNumber, int birthYear)
        {
            var dict = new Dictionary<String, Object>
            {
                ["PrivateNumber"] = privateNumber,
                ["BirthYear"] = birthYear,
            };

            var xmlElem = GovTalkCallApi.LoadXml("SRNSFPersonInfo", true, dict);
            return Deserialize<PersonInfo>(xmlElem);
        }


        private static TItem Deserialize<TItem>(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return default(TItem);

            var reader = new StringReader(xmlElement.OuterXml);
            var serializer = new XmlSerializer(typeof(TItem), xmlElement.NamespaceURI);

            return (TItem)serializer.Deserialize(reader);
        }
    }
}