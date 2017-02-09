using System;

namespace CITI.EVO.UserManagement.Web.Models.Filters
{
    [Serializable]
    public class ResourcesFilterModel
    {
        public String Keyword { get; set; }
        public Guid? ProjectID { get; set; }
    }
}