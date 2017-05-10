using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Gms.Portal.Web.Models
{
    public class ElementMoveModel
    {
        public Guid? ElementId { get; set; }
        public Guid? DestinationId { get; set; }

        public List<ListItem> FormTree { get; set; }
    }
}