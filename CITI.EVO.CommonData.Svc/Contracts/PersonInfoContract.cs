using System;
using System.Runtime.Serialization;

namespace CITI.EVO.CommonData.Svc.Contracts
{
    [DataContract]
    [Serializable]
    public class PersonInfoContract
    {
        [DataMember]
        public String ID { get; set; }

        [DataMember]
        public String PersonalID { get; set; }

        [DataMember]
        public String FirstName { get; set; }

        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public DateTime? BirthDate { get; set; }

        [DataMember]
        public String CitizenshipCountry { get; set; }

        [DataMember]
        public String CitizenshipCountryID { get; set; }

        [DataMember]
        public String PersonStatus { get; set; }

        [DataMember]
        public String PersonStatusId { get; set; }
    }
}