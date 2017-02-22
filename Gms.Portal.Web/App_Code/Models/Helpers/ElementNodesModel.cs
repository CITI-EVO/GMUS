using System;
using System.Collections.Generic;
using Gms.Portal.Web.Entities.Others;

namespace Gms.Portal.Web.Models.Helpers
{
    [Serializable]
    public class ElementNodesModel
    {
        public List<ElementTreeNodeEntity> List { get; set; }
    }
}