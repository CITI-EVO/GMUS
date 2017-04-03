using System;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataListRef
    {
        private FormDataListRef()
        {
        }

        public FormDataListRef(FormDataListRef formDataListRef)
        {
            FormID = formDataListRef.FormID;
            OwnerID = formDataListRef.OwnerID;
            ParentID = formDataListRef.ParentID;
        }

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