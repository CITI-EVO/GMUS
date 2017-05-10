using System;
using System.Collections.Generic;
using Gms.Portal.DAL.Domain;
using System.Web.UI.WebControls;

namespace Gms.Portal.Web.Models
{
    public class ElementPasteModel
    {
        public Guid? DestinationId { get; set; }

        public Guid? FormId { get; set; }
        public Guid? ElementId { get; set; }
        public List<GM_Form> Forms { get; set; }
        public List<ListItem> FormTree { get; set; }
    }
}