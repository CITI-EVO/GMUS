using System;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataListRef
    {
        public FormDataListRef(Guid? formID, Guid? ownerID, Guid? parentID)
        {
            if (formID == null || ownerID == null)
                throw new Exception();

            FormID = formID;
            OwnerID = ownerID;
            ParentID = parentID;
        }

        public Guid? FormID { get; set; }

        public Guid? OwnerID { get; set; }

        public Guid? ParentID { get; set; }
    }
}