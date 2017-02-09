using System;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.UserManagement.Web.Units
{
    [Serializable]
    public class TreeNodeUnit
    {
        static TreeNodeUnit()
        {
        }

        public Guid? Key
        {
            get
            {
                var key = String.Format("{0}/{1}/{2}", ID, ParentID, Type);
                return key.ComputeMd5Guid();
            }
        }

        public Guid? ID { get; set; }

        public Guid? ParentID { get; set; }

        public String Name { get; set; }

        public String Type { get; set; }
    }
}