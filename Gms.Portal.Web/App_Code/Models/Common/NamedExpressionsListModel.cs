using System;
using System.Collections.Generic;

namespace Gms.Portal.Web.Models.Common
{
    [Serializable]
    public class NamedExpressionsListModel
    {
        public List<NamedExpressionModel> Expressions { get; set; }
    }
}