using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class FileEntity
    {
        private readonly IDictionary<String, Object> _data;

        public FileEntity()
        {
            _data = new Dictionary<String, Object>();
        }
        public FileEntity(IDictionary<String, Object> data)
        {
            _data = data;
        }

        public Guid? ID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("ID")); }
            set { _data["ID"] = value; }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("ParentID")); }
            set { _data["ParentID"] = value; }
        }

        public String FileName
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("FileName")); }
            set { _data["FileName"] = value; }
        }

        public String FileUrl
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("FileUrl")); }
            set { _data["FileUrl"] = value; }
        }

        public String Description
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Description")); }
            set { _data["Description"] = value; }
        }

        public byte[] FileData
        {
            get { return _data.GetValueOrDefault("FileData") as byte[]; }
            set { _data["FileData"] = value; }
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