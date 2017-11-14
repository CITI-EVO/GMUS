using System;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class IDNameTreeEntity : IDNameEntity
    {
        public Guid? ParentID { get; set; }
    }
}