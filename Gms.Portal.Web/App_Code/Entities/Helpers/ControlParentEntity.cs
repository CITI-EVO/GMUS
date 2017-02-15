using System;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Entities.Helpers
{
    public class ControlTreeEntity
    {
        public ControlTreeEntity(ControlEntity control)
        {
            Control = control;
        }
        public ControlTreeEntity(ControlEntity control, Guid? parentID)
        {
            ParentID = parentID;
            Control = control;
        }

        public Guid? ParentID { get; set; }

        public ControlEntity Control { get; set; }
    }
}