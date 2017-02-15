using System;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class NamedExpressionModel
    {
        public Guid? Key { get; set; }

        public String Name { get; set; }
        public String Expression { get; set; }
        public String OutputType { get; set; }
    }
}