﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CITI.EVO.Tools.Utils;

namespace Gms.Portal.Web.Entities.DataContainer
{
    [Serializable]
    public class FormDataUnit : Dictionary<String, Object>
    {
        public const String IDField = "ID";
        public const String FormIDField = "FormID";
        public const String UserIDField = "UserID";
        public const String OwnerIDField = "OwnerID";
        public const String ParentIDField = "ParentID";
        public const String PreviousIDField = "PreviousID";

        public const String PrivacyField = "PrivaryFields";

        public const String DateCreatedField = "DateCreated";
        public const String DateChangedField = "DateChanged";
        public const String DateDeletedField = "DateDeleted";

        private static ISet<String> _defaultFields;
        public static ISet<String> DefaultFields
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (_defaultFields == null)
                {
                    _defaultFields = new HashSet<String>(new[]
                    {
                        IDField,
                        FormIDField,
                        UserIDField,
                        OwnerIDField,
                        ParentIDField,
                        PreviousIDField,
                        PrivacyField,
                        DateCreatedField,
                        DateChangedField,
                        DateDeletedField,
                    });
                }

                return _defaultFields;
            }
        }

        public FormDataUnit() : this(null)
        {
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
                var val = GetValue(IDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(IDField, value);
            }
        }

        public Guid? FormID
        {
            get
            {
                var val = GetValue(FormIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(FormIDField, value);
            }
        }

        public Guid? UserID
        {
            get
            {
                var val = GetValue(UserIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(UserIDField, value);
            }
        }

        public Guid? OwnerID
        {
            get
            {
                var val = GetValue(OwnerIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(OwnerIDField, value);
            }
        }

        public Guid? ParentID
        {
            get
            {
                var val = GetValue(ParentIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(ParentIDField, value);
            }
        }

        public Guid? PreviousID
        {
            get
            {
                var val = GetValue(PreviousIDField);
                return DataConverter.ToNullableGuid(val);
            }
            set
            {
                SetValue(PreviousIDField, value);
            }
        }

        public DateTime? DateCreated
        {
            get
            {
                var val = GetValue(DateCreatedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(DateCreatedField, value);
            }
        }

        public DateTime? DateChanged
        {
            get
            {
                var val = GetValue(DateChangedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(DateCreatedField, value);
            }
        }

        public DateTime? DateDeleted
        {
            get
            {
                var val = GetValue(DateDeletedField);
                return DataConverter.ToNullableDateTime(val);
            }
            set
            {
                SetValue(DateCreatedField, value);
            }
        }

        public ISet<String> PrivateFields
        {
            get
            {
                var val = GetValue(PrivacyField) as ISet<String>;
                return val;
            }
            set
            {
                SetValue(PrivacyField, value);
            }
        }

        public new Object this[String key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        private Object GetValue(String key)
        {
            Object val;
            if (TryGetValue(key, out val))
                return val;

            return null;
        }

        private void SetValue(String key, Object val)
        {
            base[key] = val;
        }
    }
}