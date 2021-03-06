﻿using System;

namespace Gms.Portal.Web.Entities.DataContainer
{
    public static class FormDataConstants
    {
        public const String IDField = "ID";
        public const String DocIDField = "_id";
        public const String ParamsField = "Params";
        public const String FormIDField = "FormID";
        public const String UserIDField = "UserID";
        public const String FieldIDField = "FieldID";
        public const String OwnerIDField = "OwnerID";
        public const String VersionField = "Version";
        public const String IDNumberField = "IDNumber";
        public const String StatusIDField = "StatusID";
        public const String ParentIDField = "ParentID";
        public const String PreviousIDField = "PreviousID";
        public const String ContainerIDField = "ContainerID";
        public const String ParentVersionField = "ParentVersion";

        public const String ReviewFields = "ReviewFields";
        public const String PrivacyFields = "PrivaryFields";
        public const String DescriptionField = "Description";

        public const String DateCreatedField = "DateCreated";
        public const String DateChangedField = "DateChanged";
        public const String DateDeletedField = "DateDeleted";
        public const String DateOfAcceptField = "DateOfAccept";
        public const String DateOfSubmitField = "DateOfSubmit";
        public const String DateOfStatusField = "StatusChangeDate";
        public const String DateOfAssigneField = "DateOfAssigne";

        public const String ChangesRequiresAcceptField = "ChangesRequiresAccept";
        
        public const String HashCodeField = "HashCode";

        public const String FileNameField = "FileName";
        public const String FileBytesField = "FileBytes";

        public const String DocTypeField = "DocType";

        public const String BinaryDocType = "Binary";
        public const String ReferenceDocType = "Reference";

        public const String FieldParams = "Fields";
        public const String ScoringEmail = "ScoringEmail";
        public const String ScoringParams = "Scoring";
        public const String StatusComment = "StatusComment";
        public const String UserStatusesFields = "UserStatusesFields";

        public const String MonitoringBudget = "MonitoringBudget";
        public const String MonitoringProject = "MonitoringProject";
        public const String MonitoringPeriod = "MonitoringPeriod";
        public const String MonitoringStartDate = "MonitoringStartDate";
        public const String MonitoringEndDate = "MonitoringEndDate";
        
        public const String UserStatusStepField = "UserStatusesFields.Step";
        public const String UserStatusParamsField = "UserStatusesFields.Params";
        public const String UserStatusUserIDField = "UserStatusesFields.UserID";
        public const String UserStatusStatusIDField = "UserStatusesFields.StatusID";
        public const String UserStatusDateOfStatusField = "UserStatusesFields.DateOfStatus";
    }
}