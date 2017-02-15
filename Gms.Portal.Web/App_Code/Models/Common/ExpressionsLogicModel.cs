using System;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class ExpressionsLogicModel
    {
        public NamedExpressionsListModel Select { get; set; }
        public ExpressionsListModel FilterBy { get; set; }
        public ExpressionsListModel OrderBy { get; set; }
        public ExpressionsListModel GroupBy { get; set; }
    }
}