using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.Monitoring
{
    [Serializable]
    public class ProjectTaskEntity
    {
        private readonly IDictionary<String, Object> _data;

        public ProjectTaskEntity()
        {
            _data = new Dictionary<String, Object>();
        }
        public ProjectTaskEntity(IDictionary<String, Object> data)
        {
            _data = data;
        }

        public Guid? ID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("ID")); }
            set { _data["ID"] = value; }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("OwnerID")); }
            set { _data["OwnerID"] = value; }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("RecordID")); }
            set { _data["RecordID"] = value; }
        }

        public Guid? TaskID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("TaskID")); }
            set { _data["TaskID"] = value; }
        }

        public String Name
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Name")); }
            set { _data["Name"] = value; }
        }

        public DateTime? StartDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("StartDate")); }
            set { _data["StartDate"] = value; }
        }

        public DateTime? EndDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("EndDate")); }
            set { _data["EndDate"] = value; }
        }

        public String DoneStatus
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("DoneStatus")); }
            set { _data["DoneStatus"] = value; }
        }
        public String DoneDescription
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("DoneDescription")); }
            set { _data["DoneDescription"] = value; }
        }

        public String Status
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Status")); }
            set { _data["Status"] = value; }
        }

        public Guid? CreateUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("CreateUserID")); }
            set { _data["CreateUserID"] = value; }
        }

        public DateTime? SubmitDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("SubmitDate")); }
            set { _data["SubmitDate"] = value; }
        }

        public Guid? SubmitUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("SubmitUserID")); }
            set { _data["SubmitUserID"] = value; }
        }
        
        public ISet<Guid?> FlawsID
        {
            get
            {
                var array = _data.GetValueOrDefault("FlawsID") as IEnumerable<Object>;
                array = (array ?? Enumerable.Empty<Object>());

                var query = (from n in array
                             let v = DataConverter.ToNullableGuid(n)
                             where v != null
                             select v);

                return query.ToHashSet();
            }
            set
            {
                if (value == null)
                    _data["FlawsID"] = value;
                else
                    _data["FlawsID"] = value.ToArray();
            }
        }

        public String Comment
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Comment")); }
            set { _data["Comment"] = value; }
        }
        public DateTime? ExpireDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("ExpireDate")); }
            set { _data["ExpireDate"] = value; }
        }
        public DateTime? DateCreated
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("DateCreated")); }
            set { _data["DateCreated"] = value; }
        }
        public DateTime? DateChanged
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("DateChanged")); }
            set { _data["DateChanged"] = value; }
        }
        public DateTime? DateDeleted
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("DateDeleted")); }
            set { _data["DateDeleted"] = value; }
        }

        public IDictionary<String, Object> GetContainer()
        {
            return _data;
        }
    }
}