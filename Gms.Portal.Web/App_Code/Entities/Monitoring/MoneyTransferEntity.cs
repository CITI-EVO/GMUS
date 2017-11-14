using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.Monitoring
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

        public Guid? ParagraphID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("ParagraphID")); }
            set { _data["ParagraphID"] = value; }
        }

        public Guid? OrganizationID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("OrganizationID")); }
            set { _data["OrganizationID"] = value; }
        }

        public String Goal
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Goal")); }
            set { _data["Goal"] = value; }
        }

        public String PaymentType
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("PaymentType")); }
            set { _data["PaymentType"] = value; }
        }

        public String OrganizationType
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("OrganizationType")); }
            set { _data["OrganizationType"] = value; }
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

        public Guid? CreateUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("CreateUserID")); }
            set { _data["CreateUserID"] = value; }
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

        public String Status
        {
            get { return DataConverter.ToString(_data.GetValueOrDefault("Status")); }
            set { _data["Status"] = value; }
        }

        public Guid? StatusUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("StatusUserID")); }
            set { _data["StatusUserID"] = value; }
        }

        public DateTime? StatusDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("StatusDate")); }
            set { _data["StatusDate"] = value; }
        }

        public Guid? SubmitUserID
        {
            get { return DataConverter.ToNullableGuid(_data.GetValueOrDefault("SubmitUserID")); }
            set { _data["SubmitUserID"] = value; }
        }

        public DateTime? SubmitDate
        {
            get { return DataConverter.ToNullableDateTime(_data.GetValueOrDefault("SubmitDate")); }
            set { _data["SubmitDate"] = value; }
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