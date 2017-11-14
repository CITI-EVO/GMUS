using System;
using System.Xml.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts.GovTalk.Response
{
    [Serializable]
    [XmlRoot("Person")]
    public class PersonInfo : ResultData
    {
        [XmlElement]
        public string ID { get; set; }

        [XmlElement]
        public string PrivateNumber { get; set; }

        [XmlElement]
        public string LastName { get; set; }

        [XmlElement]
        public string FirstName { get; set; }

        [XmlElement]
        public string BirthDate { get; set; }

        [XmlElement]
        public string GenderName { get; set; }

        [XmlElement]
        public string GenderID { get; set; }

        [XmlElement]
        public string PersonStatus { get; set; }

        [XmlElement]
        public string PersonStatusId { get; set; }

        [XmlElement]
        public string BirthPlace { get; set; }

        [XmlElement]
        public string BirthPlaceCountryId { get; set; }

        [XmlElement]
        public string BirthPlaceCountry { get; set; }

        [XmlElement]
        public string BirthPlaceRaionId { get; set; }

        [XmlElement]
        public string BirthPlaceRaion { get; set; }

        [XmlElement]
        public string CitizenshipCountry { get; set; }

        [XmlElement]
        public string CitizenshipCountryID { get; set; }

        [XmlElement]
        public string MainDataId { get; set; }

        [XmlElement]
        public string AppDataId { get; set; }

        [XmlElement]
        public Registration Registration { get; set; }

        [XmlElement]
        public PersonAdditionalStatuses PersonAdditionalStatuses { get; set; }
    }
}
