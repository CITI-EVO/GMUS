using System;
using System.Collections.Generic;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.Others
{
    [Serializable]
    public class MoneyTransferEntity
    {
        private readonly IDictionary<String, Object> _data;

        public MoneyTransferEntity()
        {
            _data = new Dictionary<String, Object>();
        }
        public MoneyTransferEntity(IDictionary<String, Object> data)
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

        public Guid? GoalID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("GoalID")); }
            set { _data["GoalID"] = value; }
        }

        public DateTime? DateOfTransfer
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("DateOfTransfer")); }
            set { _data["DateOfTransfer"] = value; }
        }

        public double? Remain
        {
            get { return DataConverter.ToNullableDouble(_data.GetValueOrDefault("Remain")); }
            set { _data["Remain"] = value; }
        }

        public double? Incoming
        {
            get { return DataConverter.ToNullableDouble(_data.GetValueOrDefault("Incoming")); }
            set { _data["Incoming"] = value; }
        }

        public double? Outgoing
        {
            get { return DataConverter.ToNullableDouble(_data.GetValueOrDefault("Outgoing")); }
            set { _data["Outgoing"] = value; }
        }

        public bool? Returned
        {
            get { return DataConverter.ToNullableBool(_data.GetValueOrDefault("Returned")); }
            set { _data["Returned"] = value; }
        }

        public bool? Accepted
        {
            get { return DataConverter.ToNullableBool(_data.GetValueOrDefault("Accepted")); }
            set { _data["Accepted"] = value; }
        }

        public Guid? CreateUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("CreateUserID")); }
            set { _data["CreateUserID"] = value; }
        }

        public Guid? AcceptUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("AcceptUserID")); }
            set { _data["AcceptUserID"] = value; }
        }

        public DateTime? DateOfAccept
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("DateOfAccept")); }
            set { _data["DateOfAccept"] = value; }
        }


        public String Comment
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Comment")); }
            set { _data["Comment"] = value; }
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