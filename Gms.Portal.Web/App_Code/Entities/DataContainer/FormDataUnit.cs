using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataUnit : FormDataBase
    {
        public FormDataUnit() : this((Guid?)null)
        {
        }

        public FormDataUnit(FormDataUnit formData)
        {
            foreach (var pair in formData)
            {
                if (pair.Value is FormDataUnit)
                {
                    var clone = new FormDataUnit((FormDataUnit)pair.Value);
                    SetValue(pair.Key, clone);
                }
                else if (pair.Value is FormDataBinary)
                {
                    var clone = new FormDataBinary((FormDataBinary)pair.Value);
                    SetValue(pair.Key, clone);
                }
                else if (pair.Value is FormDataListRef)
                {
                    var clone = new FormDataListRef((FormDataListRef)pair.Value);
                    SetValue(pair.Key, clone);
                }
                else if (pair.Value is FormDataLazyList)
                {
                    var clone = new FormDataLazyList((FormDataLazyList)pair.Value);
                    SetValue(pair.Key, clone);
                }
                else if (pair.Value is FormDataListBase)
                {
                    var clone = new FormDataListBase((FormDataListBase)pair.Value);
                    SetValue(pair.Key, clone);
                }
                else
                {
                    SetValue(pair.Key, pair.Value);
                }
            }
        }

        public FormDataUnit(Guid? formID) : this(formID, null)
        {
        }
        public FormDataUnit(Guid? formID, Guid? ownerID) : this(formID, ownerID, null, null, null)
        {
        }

        public FormDataUnit(Guid? formID, Guid? ownerID, Guid? Id) : this(formID, ownerID, Id, null, null)
        {
        }

        public FormDataUnit(Guid? formID, Guid? ownerID, Guid? Id, Guid? parentID, DateTime? dateCreated)
        {
            ID = Id;
            FormID = formID;
            OwnerID = ownerID;
            ParentID = parentID;
            DateCreated = dateCreated;
        }

        public Guid? ID
        {
            get
            {
                var val = GetValue(FormDataConstants.IDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.IDField, value);
            }
        }

        public Guid? FormID
        {
            get
            {
                var val = GetValue(FormDataConstants.FormIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.FormIDField, value);
            }
        }

        public Guid? UserID
        {
            get
            {
                var val = GetValue(FormDataConstants.UserIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.UserIDField, value);
            }
        }

        public Guid? OwnerID
        {
            get
            {
                var val = GetValue(FormDataConstants.OwnerIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.OwnerIDField, value);
            }
        }

        public Guid? StatusID
        {
            get
            {
                var val = GetValue(FormDataConstants.StatusIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.StatusIDField, value);
            }
        }

        public Guid? ParentID
        {
            get
            {
                var val = GetValue(FormDataConstants.ParentIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.ParentIDField, value);
            }
        }

        public Guid? ContainerID
        {
            get
            {
                var val = GetValue(FormDataConstants.ContainerIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.ContainerIDField, value);
            }
        }

        public Guid? PreviousID
        {
            get
            {
                var val = GetValue(FormDataConstants.PreviousIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormDataConstants.PreviousIDField, value);
            }
        }

        public String HashCode
        {
            get
            {
                var val = GetValue(FormDataConstants.HashCodeField);
                return DataConverter.ToString(val);
            }
            set
            {
                SetValue(FormDataConstants.HashCodeField, value);
            }
        }

        public String Description
        {
            get
            {
                var val = GetValue(FormDataConstants.DescriptionField);
                return DataConverter.ToString(val);
            }
            set
            {
                SetValue(FormDataConstants.DescriptionField, value);
            }
        }

        public DateTime? DateCreated
        {
            get
            {
                var val = GetValue(FormDataConstants.DateCreatedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(FormDataConstants.DateCreatedField, value);
            }
        }

        public DateTime? DateChanged
        {
            get
            {
                var val = GetValue(FormDataConstants.DateChangedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(FormDataConstants.DateChangedField, value);
            }
        }

        public DateTime? DateDeleted
        {
            get
            {
                var val = GetValue(FormDataConstants.DateDeletedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(FormDataConstants.DateDeletedField, value);
            }
        }

        public ISet<String> ReviewFields
        {
            get
            {
                var val = GetValue(FormDataConstants.ReviewFields) as ISet<String>;
                return val;
            }
            set
            {
                SetValue(FormDataConstants.ReviewFields, value);
            }
        }

        public ISet<String> PrivateFields
        {
            get
            {
                var val = GetValue(FormDataConstants.PrivacyFields) as ISet<String>;
                return val;
            }
            set
            {
                SetValue(FormDataConstants.PrivacyFields, value);
            }
        }

        public IList<FormStatusUnit> UserStatuses
        {
            get
            {
                var val = GetValue(FormDataConstants.UserStatusesFields) as IList<FormStatusUnit>;
                return val;
            }
            set
            {
                SetValue(FormDataConstants.UserStatusesFields, value);
            }
        }
    }
}