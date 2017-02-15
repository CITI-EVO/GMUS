using System;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class ExpressionModel
    {
        public Guid? Key { get; set; }

        public String Expression { get; set; }
        public String OutputType { get; set; }
    }
}