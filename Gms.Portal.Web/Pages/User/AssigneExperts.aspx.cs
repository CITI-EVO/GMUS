using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Driver;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.Others;
using System.Collections;
using System.Net;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Web.UI.Helpers;
using CITI.EVO.UserManagement.Svc.Contracts;
using Gms.Portal.Web.Models;
using MongoDB.Bson;
using NVelocityTemplateEngine;

namespace Gms.Portal.Web.Pages.User
{
    public partial class AssigneExperts : BasePage
    {
        private const String keywordGridAlias = "keywords";
        private const String keywordFieldAlias = "keyword";

        private const String employmentGridAlias = "current_employment";
        private const String workPlaceFieldAlias = "work_place";
        private const String workDepartmentFieldAlias = "work_department";

        private const String primaryDirectionFieldAlias = "primary_direction";
        private const String secondDirectionFieldAlias = "second_direction";
        private const String thirdDirectionFieldAlias = "third_direction";

        private const String primaryPersonnelGridAlias = "primary_personnels";
        private const String primaryPersonnelFieldAlias = "primary_personnel";

        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? RecordID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["RecordID"]); }
        }

        public Guid? UserID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["UserID"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            FillDataGrid();
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {



        }

        protected void btnRandom_OnClick(object sender, EventArgs e)
        {
            lstSelectedEsperts.Items.Clear();
            hdnSelectedExperts.Value = null;
            seExpertsCount.Text = null;

            mpeRandomExperts.Show();
        }

        protected void btnRefreshExpertsOK_OnClick(object sender, EventArgs e)
        {
            var count = DataConverter.ToNullableInt32(seExpertsCount.Text);
            if (count.GetValueOrDefault() < 1)
            {
                mpeRandomExperts.Show();
                return;
            }

            var filter = assigneExpertsFilterControl.Model;
            var gridData = GetGridData(false, false);

            if (filter.GroupID != null)
            {
                var recipients = GetRecipientGroupUsers(filter.GroupID.Value);
                var userIds = recipients.Select(n => n.UserID).ToList();

                gridData = from n in gridData
                           let u = DataConverter.ToNullableGuid(n[FormDataConstants.UserIDField])
                           where userIds.Contains(u)
                           select n;
            }

            gridData = from n in gridData
                       let userID = DataConverter.ToNullableGuid(n[FormDataConstants.UserIDField])
                       let statuses = n[FormDataConstants.UserStatusesFields] as IList<FormStatusUnit>
                       where statuses != null && statuses.All(m => m.UserID != userID)
                       select n;

            var list = gridData.ToList();


            var random = new Random();
            var selecteds = new HashSet<UserContract>();

            if (list.Count <= count)
            {
                var users = (from n in list
                             let userID = DataConverter.ToNullableGuid(n["UserID"])
                             where userID != null
                             let user = UmUsersCache.GetUser(userID)
                             where user != null
                             select user);

                selecteds.UnionWith(users);
            }
            else
            {
                while (selecteds.Count < count)
                {
                    var index = random.Next(0, list.Count);
                    var item = list[index];

                    var userID = DataConverter.ToNullableGuid(item["UserID"]);
                    if (userID == null)
                        continue;

                    var user = UmUsersCache.GetUser(userID);
                    if (user == null)
                        continue;

                    selecteds.Add(user);
                }
            }

            var query = (from n in selecteds
                         let name = $"{n.LoginName} - {n.FirstName} {n.LastName}"
                         orderby name
                         select new
                         {
                             ID = n.ID,
                             Name = name
                         });

            lstSelectedEsperts.DataSource = query;
            lstSelectedEsperts.DataBind();

            hdnSelectedExperts.Value = String.Join(",", selecteds.Select(n => n.ID));
        }

        protected void btnRecordStatusOK_OnClick(object sender, EventArgs e)
        {
            var ownerID = FormID;
            var recordID = RecordID;

            var model = recordStatusControl.Model;

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == model.RecordID);
            if (formStatus == null)
                return;

            formStatus.StatusID = model.StatusID;

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(ownerID, document);

            mpeRecordStatus.Hide();
        }

        protected void btnRecordStatusCancel_OnClick(object sender, EventArgs e)
        {
            mpeRecordStatus.Hide();
        }

        protected void btnRandomExpertsOK_OnClick(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(hdnSelectedExperts.Value))
                return;

            var parts = hdnSelectedExperts.Value.Split(',');
            var users = (from n in parts
                         let m = DataConverter.ToNullableGuid(n)
                         let u = UmUsersCache.GetUser(m)
                         where m != null && u != null
                         select u);


            var ownerID = FormID;
            var recordID = RecordID;

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            foreach (var user in users)
            {
                var formStatus = statusUnits.FirstOrDefault(n => n.UserID == user.ID);
                if (formStatus == null)
                {
                    formStatus = new FormStatusUnit
                    {
                        UserID = user.ID,
                        DateOfAssigne = DateTime.Now,
                    };

                    statusUnits.Add(formStatus);
                }

                var @params = new Dictionary<String, Object>
                {
                    {FormDataConstants.ScoringParams, "@"}
                };

                formStatus.Params = GmsCommonUtil.Merge(formStatus.Params, @params);
            }

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(ownerID, document);

            mpeRandomExperts.Hide();
        }

        protected void btnRandomExpertsCancel_OnClick(object sender, EventArgs e)
        {
            mpeRandomExperts.Hide();
        }

        protected void assigneExpertsControl_OnAttach(object sender, GenericEventArgs<Guid> e)
        {
            var ownerID = FormID;
            var recordID = RecordID;

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == e.Value);
            if (formStatus == null)
            {
                formStatus = new FormStatusUnit
                {
                    UserID = e.Value,
                    DateOfAssigne = DateTime.Now,
                };

                statusUnits.Add(formStatus);
            }

            var @params = new Dictionary<String, Object>
            {
                {FormDataConstants.ScoringParams, "@"}
            };

            formStatus.Params = GmsCommonUtil.Merge(formStatus.Params, @params);

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(ownerID, document);
        }

        protected void assigneExpertsControl_OnDetach(object sender, GenericEventArgs<Guid> e)
        {
            var ownerID = FormID;
            var recordID = RecordID;

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == e.Value);
            if (formStatus == null)
                return;

            if (formStatus.Params != null)
            {
                if (!formStatus.Params.ContainsKey(FormDataConstants.FieldParams))
                    statusUnits.Remove(formStatus);
                else
                    formStatus.Params.Remove(FormDataConstants.ScoringParams);
            }

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(ownerID, document);
        }

        protected void assigneExpertsControl_OnStatus(object sender, GenericEventArgs<Guid> e)
        {
            var model = new RecordStatusModel
            {
                FormID = FormID,
                RecordID = e.Value
            };

            recordStatusControl.Model = model;
            mpeRecordStatus.Show();
        }

        protected void assigneExpertsControl_OnEmail(object sender, GenericEventArgs<Guid> e)
        {
            var ownerID = FormID;
            var recordID = RecordID;

            var document = MongoDbUtil.GetDocument(ownerID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                bsonValue = new BsonArray();

            var bsonArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (bsonArray == null)
                bsonArray = new BsonArray();

            var statusUnits = BsonDocumentConverter.ConvertToFormStatuses(bsonArray).ToHashSet();

            var formStatus = statusUnits.FirstOrDefault(n => n.UserID == e.Value);
            if (formStatus == null)
                return;

            var @params = new Dictionary<String, Object>
            {
                {FormDataConstants.ScoringParams, "@"},
                {FormDataConstants.ScoringEmail, "Sent"},
            };

            formStatus.Params = GmsCommonUtil.Merge(formStatus.Params, @params);

            var formStatusesArray = BsonDocumentConverter.ConvertToFormStatusesArray(statusUnits);
            document[FormDataConstants.UserStatusesFields] = formStatusesArray;

            MongoDbUtil.UpdateDocument(ownerID, document);

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            TrySendMail(e.Value, formData);
        }

        protected void FillDataGrid()
        {
            var @set = new HashSet<String>
            {
                "RecordID",
                "FormID",
                "UserID",
                "FirstName",
                "LastName",
                "UserStatus",
                "Email",
                "EmailStatus",
                "StatusName",
                "Directions",
                "Keywords",
                "Projects",
                "UserStatusesFields"
            };

            var filter = assigneExpertsFilterControl.Model;
            var gridData = GetGridData(filter.AllUsers, filter.Assigneds);

            if (filter.GroupID != null)
            {
                var recipients = GetRecipientGroupUsers(filter.GroupID.Value);
                var userIds = recipients.Select(n => n.UserID).ToList();

                gridData = from n in gridData
                           let u = DataConverter.ToNullableGuid(n[FormDataConstants.UserIDField])
                           where userIds.Contains(u)
                           select n;
            }

            if (filter.Assigneds.GetValueOrDefault())
            {
                gridData = from n in gridData
                           let userID = DataConverter.ToNullableGuid(n[FormDataConstants.UserIDField])
                           let statuses = n[FormDataConstants.UserStatusesFields] as IList<FormStatusUnit>
                           where statuses != null &&
                                 statuses.Any(m => m.UserID == userID)
                           select n;
            }

            var dataView = new DictionaryDataView(gridData, @set);

            var model = new AssigneExpertsModel
            {
                DataView = dataView,
            };

            assigneExpertsControl.Model = model;
            assigneExpertsControl.DataBind();
        }

        protected IEnumerable<IDictionary<String, Object>> GetGridData(bool? allUsers, bool? assigneds)
        {
            var comparer = StringComparer.OrdinalIgnoreCase;

            var formsList = (from n in HbSession.Query<GM_Form>()
                             where n.DateDeleted == null &&
                                   n.FormType == "PersonalProfile"
                             select n).ToList();

            if (!formsList.Any())
            {
                lblError.Text = "Not specified profile forms in configuration";
                yield break;
            }

            var converter = new FormEntityModelConverter(HbSession);

            var formModels = (from n in formsList
                              let m = converter.Convert(n)
                              select m).ToList();

            var formNames = new Dictionary<Guid?, String>();
            var formControls = new Dictionary<Guid?, ILookup<String, ControlEntity>>();

            foreach (var formModel in formModels)
            {
                if (formModel == null || formModel.Entity == null)
                    continue;

                var controls = FormStructureUtil.PreOrderTraversal(formModel.Entity).ToList();
                var controlsLp = controls.ToLookup(n => n.Alias, comparer);

                formNames.Add(formModel.ID, formModel.Name);
                formControls.Add(formModel.ID, controlsLp);
            }

            if (!formControls.ContainsKey(FormID))
            {
                var baseForm = HbSession.Query<GM_Form>().FirstOrDefault(h => h.ID == FormID);
                var baseModel = converter.Convert(baseForm);

                var baseControls = FormStructureUtil.PreOrderTraversal(baseModel.Entity).ToList();
                var baseControlsLp = baseControls.ToLookup(n => n.Alias, comparer);

                formNames.Add(baseModel.ID, baseModel.Name);
                formControls.Add(baseModel.ID, baseControlsLp);
            }

            var profileForms = formModels.Select(n => n.ID).ToHashSet();

            var profileFormControls = (from n in formControls
                                       where profileForms.Contains(n.Key)
                                       select n).ToDictionary();

            var commonConfigErrors = CheckCommonFormsConfig(formControls, formNames);
            var projectConfigErrors = CheckProjectFormsConfig(formControls, formNames);
            var profileConfigErrors = CheckProfileFormsConfig(profileFormControls, formNames);

            var configErrors = new HashSet<String>();
            configErrors.UnionWith(commonConfigErrors);
            configErrors.UnionWith(profileConfigErrors);
            configErrors.UnionWith(projectConfigErrors);

            if (configErrors.Count > 0)
            {
                lblError.Text = String.Join("<br/>", configErrors);
                yield break;
            }

            var profileFormData = GetProfileFormData(formControls.Keys);
            if (profileFormData == null)
            {
                lblError.Text = "Unable to get profile data";
                yield break;
            }

            var allFormDatas = GetAllFormDatas(formControls.Keys).ToList();

            var projectFormData = allFormDatas.FirstOrDefault(n => n.ID == RecordID);
            if (projectFormData == null)
            {
                lblError.Text = "Unable to get project data";
                yield break;
            }

            var directions = GetDirections(allFormDatas, formControls);
            var keywords = GetAllKeywordFormDatas(formControls);
            var employments = GetAllEmploymentFormDatas(formControls);
            var primaryPersonnels = GetAllPrimaryPersonnelFormDatas(formControls);

            var directionsLp = directions.ToLookup(n => n.ID);
            var keywordsDatasLp = keywords.ToLookup(n => n.ParentID);
            var employmentsDatasLp = employments.ToLookup(n => n.ParentID);
            var primaryPersonnelDatasLp = primaryPersonnels.ToLookup(n => n.ParentID);

            var specialComparer = new SpecialFormDataComparer(profileFormData, projectFormData, directionsLp, keywordsDatasLp, employmentsDatasLp, primaryPersonnelDatasLp);
            var comparisonComparer = new ComparisonComparer<FormDataUnit>((x, y) => specialComparer.Compare(x, y));

            var finalQuery = allFormDatas.AsQueryable();

            if (!allUsers.GetValueOrDefault() && !assigneds.GetValueOrDefault())
            {
                var projectUsersQuery = (from n in allFormDatas
                                         where n.FormID == FormID
                                         select n.UserID);

                var projectUsers = projectUsersQuery.ToHashSet();

                finalQuery = (from n in finalQuery
                              where !projectUsers.Contains(n.UserID)
                              select n);

                finalQuery = (from n in finalQuery
                              where specialComparer.IsAcceptable(n)
                              select n);

                finalQuery = finalQuery.OrderByDescending(n => n, comparisonComparer);
            }

            var @set = new HashSet<Guid?>();

            foreach (var item in finalQuery)
            {
                if (!@set.Add(item.UserID))
                    continue;

                var umUser = UmUsersCache.GetUser(item.UserID);
                if (umUser == null)
                    continue;

                var directionsQuery = directionsLp[item.ID].Select(n => n.Name);

                var itemDirections = String.Join("; ", directionsQuery);
                var itemKeywords = specialComparer.GetSameKeywords(projectFormData, item);

                var emailStatus = (Object)null;
                var statusName = String.Empty;


                var userStatuses = projectFormData.UserStatuses;
                if (userStatuses != null)
                {
                    var userStatus = userStatuses.FirstOrDefault(n => n.UserID == item.UserID);
                    if (userStatus != null && userStatus.Params != null)
                    {
                        emailStatus = userStatus.Params.GetValueOrDefault(FormDataConstants.ScoringEmail);

                        var dataStatus = DataStatusCache.GetStatus(userStatus.StatusID);
                        if (dataStatus != null)
                            statusName = dataStatus.Name;
                    }
                }

                var dict = new Dictionary<String, Object>
                {
                    ["RecordID"] = item.ID,
                    ["FormID"] = item.FormID,
                    ["UserID"] = item.UserID,
                    ["FirstName"] = umUser.FirstName,
                    ["LastName"] = umUser.LastName,
                    ["UserStatus"] = UserUtil.GetUserStatus(umUser),
                    ["Email"] = umUser.Email,
                    ["EmailStatus"] = emailStatus,
                    ["StatusName"] = statusName,
                    ["Directions"] = itemDirections,
                    ["Keywords"] = itemKeywords.Count,
                    ["Projects"] = null,
                    ["UserStatusesFields"] = projectFormData.UserStatuses,
                };

                yield return dict;
            }
        }

        protected FormDataUnit GetProfileFormData(IEnumerable<Guid?> formsID)
        {
            foreach (var formID in formsID)
            {
                var collection = MongoDbUtil.GetCollection(formID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.UserIDField] == UserID &&
                                   n[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select n);

                var document = query.FirstOrDefault();
                if (document == null)
                    continue;

                var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                return formData;
            }

            return null;
        }

        protected void TrySendMail(Guid userID, FormDataUnit formData)
        {
            var user = UmUsersCache.GetUser(userID);
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
                return;

            var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);
            var modelConverter = new FormEntityModelConverter(HbSession);

            var model = modelConverter.Convert(dbForm);
            if (model == null)
                return;

            var entity = model.Entity;
            if (entity.Rating == null)
                return;

            var mailTemplate = entity.Rating.MailTemplate;
            if (String.IsNullOrWhiteSpace(mailTemplate))
                return;

            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = FormID,
                ["RecordID"] = RecordID,
                ["TemplateID"] = entity.Rating.ID,
                ["TemplateType"] = "Mail",
                ["ResponseType"] = "Html",
                ["LoginToken"] = UmUtil.Instance.CurrentToken
            };

            using (var client = new WebClient())
            {
                var url = HttpServerUtil.ResolveAbsoluteUrl(urlHelper.ToEncodedUrl());
                var text = client.DownloadString(url);

                EmailUtil.SendEmail(user.Email, "Rnsf", text);
            }
        }

        protected List<RecipientModel> GetRecipientGroupUsers(Guid groupId)
        {
            var entities = (from n in HbSession.Query<GM_Recipient>()
                            where n.DateDeleted == null && n.GroupID == groupId
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new RecipientEntityModelConverter(HbSession);

            var recipients = (from n in entities
                              let m = converter.Convert(n)
                              select m).ToList();

            return recipients;
        }

        protected IEnumerable<FormDataUnit> GetAllFormDatas(params Guid?[] formsID)
        {
            return GetAllFormDatas((IEnumerable<Guid?>)formsID);
        }
        protected IEnumerable<FormDataUnit> GetAllFormDatas(IEnumerable<Guid?> formsID)
        {
            foreach (var formID in formsID)
            {
                var collection = MongoDbUtil.GetCollection(formID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select n);

                foreach (var document in query)
                {
                    var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
                    yield return formData;
                }
            }
        }
        protected IEnumerable<FormDataUnit> GetAllKeywordFormDatas(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict)
        {
            foreach (var pair in controlsDict)
            {
                var controlsLp = pair.Value;

                var formGrid = controlsLp[keywordGridAlias].FirstOrDefault();
                if (formGrid == null)
                    yield break;

                var allControls = FormStructureUtil.PreOrderTraversal(formGrid);
                var gridFields = allControls.ToLookup(n => n.Alias);

                var keywordField = gridFields[keywordFieldAlias].FirstOrDefault();

                if (keywordField == null)
                    yield break;

                var collection = MongoDbUtil.GetCollection(formGrid.ID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select n);

                var keywordFieldKey = Convert.ToString(keywordField.ID);

                foreach (var bsonDoc in query)
                {
                    var dict = BsonDocumentConverter.ConvertToDictionary(bsonDoc);

                    var formData = new FormDataUnit
                    {
                        [FormDataConstants.IDField] = dict.GetValueOrDefault(FormDataConstants.IDField),
                        [FormDataConstants.ParentIDField] = dict.GetValueOrDefault(FormDataConstants.ParentIDField),

                        [keywordField.Alias] = dict.GetValueOrDefault(keywordFieldKey)
                    };

                    yield return formData;
                }
            }
        }
        protected IEnumerable<FormDataUnit> GetAllEmploymentFormDatas(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict)
        {
            foreach (var pair in controlsDict)
            {
                var controlsLp = pair.Value;

                var formGrid = controlsLp[employmentGridAlias].FirstOrDefault();
                if (formGrid == null)
                    yield break;

                var allControls = FormStructureUtil.PreOrderTraversal(formGrid);
                var gridFields = allControls.ToLookup(n => n.Alias);

                var workPlaceField = gridFields[workPlaceFieldAlias].FirstOrDefault();
                var workDepartmentField = gridFields[workDepartmentFieldAlias].FirstOrDefault();

                if (workPlaceField == null || workDepartmentField == null)
                    yield break;

                var collection = MongoDbUtil.GetCollection(formGrid.ID);

                var query = (from n in collection.AsQueryable()
                             where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                             select n);

                var workPlaceFieldKey = Convert.ToString(workPlaceField.ID);
                var workDepartmentFieldKey = Convert.ToString(workDepartmentField.ID);

                foreach (var bsonDoc in query)
                {
                    var dict = BsonDocumentConverter.ConvertToDictionary(bsonDoc);

                    var formData = new FormDataUnit
                    {
                        [FormDataConstants.IDField] = dict.GetValueOrDefault(FormDataConstants.IDField),
                        [FormDataConstants.ParentIDField] = dict.GetValueOrDefault(FormDataConstants.ParentIDField),

                        [workPlaceField.Alias] = dict.GetValueOrDefault(workPlaceFieldKey),
                        [workDepartmentField.Alias] = dict.GetValueOrDefault(workDepartmentFieldKey)
                    };


                    yield return formData;
                }
            }
        }
        protected IEnumerable<FormDataUnit> GetAllPrimaryPersonnelFormDatas(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict)
        {
            var controlsLp = controlsDict[FormID];

            var formGrid = controlsLp[primaryPersonnelGridAlias].FirstOrDefault();
            if (formGrid == null)
                yield break;

            var allControls = FormStructureUtil.PreOrderTraversal(formGrid);
            var gridFields = allControls.ToLookup(n => n.Alias);

            var personnelField = gridFields[primaryPersonnelFieldAlias].FirstOrDefault();
            if (personnelField == null)
                yield break;

            var collection = MongoDbUtil.GetCollection(formGrid.ID);

            var query = (from n in collection.AsQueryable()
                         where n[FormDataConstants.DateDeletedField] == (DateTime?)null
                         select n);

            var personnelFieldKey = Convert.ToString(personnelField.ID);

            foreach (var bsonDoc in query)
            {
                var dict = BsonDocumentConverter.ConvertToDictionary(bsonDoc);

                var formData = new FormDataUnit
                {
                    [FormDataConstants.IDField] = dict.GetValueOrDefault(FormDataConstants.IDField),
                    [FormDataConstants.ParentIDField] = dict.GetValueOrDefault(FormDataConstants.ParentIDField),

                    [personnelField.Alias] = dict.GetValueOrDefault(personnelFieldKey),
                };

                yield return formData;
            }
        }

        protected IEnumerable<IDNameExEntity> GetDirections(FormDataUnit formData, ILookup<String, ControlEntity> controlsLp)
        {
            var primaryDirectionField = controlsLp[primaryDirectionFieldAlias].FirstOrDefault();
            if (primaryDirectionField != null)
            {
                var directionFieldKey = Convert.ToString(primaryDirectionField.ID);
                var direction = Convert.ToString(formData[directionFieldKey]);

                if (!String.IsNullOrWhiteSpace(direction))
                {
                    var entity = new IDNameExEntity
                    {
                        ID = formData.ID,
                        Value = direction,
                        Name = GetDirectionText(formData, primaryDirectionField, controlsLp)
                    };

                    yield return entity;
                }
            }

            var secondDirectionField = controlsLp[secondDirectionFieldAlias].FirstOrDefault();
            if (secondDirectionField != null)
            {
                var directionFieldKey = Convert.ToString(secondDirectionField.ID);
                var direction = Convert.ToString(formData[directionFieldKey]);

                if (!String.IsNullOrWhiteSpace(direction))
                {
                    var entity = new IDNameExEntity
                    {
                        ID = formData.ID,
                        Value = direction,
                        Name = GetDirectionText(formData, secondDirectionField, controlsLp)
                    };

                    yield return entity;
                }
            }

            var thirdDirectionField = controlsLp[thirdDirectionFieldAlias].FirstOrDefault();
            if (thirdDirectionField != null)
            {
                var directionFieldKey = Convert.ToString(thirdDirectionField.ID);
                var direction = Convert.ToString(formData[directionFieldKey]);

                if (!String.IsNullOrWhiteSpace(direction))
                {
                    var entity = new IDNameExEntity
                    {
                        ID = formData.ID,
                        Value = direction,
                        Name = GetDirectionText(formData, thirdDirectionField, controlsLp)
                    };

                    yield return entity;
                }
            }
        }
        protected IEnumerable<IDNameExEntity> GetDirections(IEnumerable<FormDataUnit> formDatas, IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict)
        {
            var query = (from n in formDatas
                         let c = controlsDict.GetValueOrDefault(n.FormID)
                         from m in GetDirections(n, c)
                         select m);

            return query;
        }

        protected String GetDirectionText(FormDataUnit formData, ControlEntity entity, ILookup<String, ControlEntity> controlsLp)
        {
            return Convert.ToString(formData[Convert.ToString(entity.ID)]);
            //var value = formData[Convert.ToString(entity.ID)];
            //if (value == null)
            //    return Convert.ToString(value);

            //var fieldEntity = entity as FieldEntity;
            //if (fieldEntity == null)
            //    return null;

            //if (fieldEntity.Type != "ComboBox" && fieldEntity.Type != "CheckBoxList")
            //    return Convert.ToString(value);

            //var textExp = fieldEntity.TextExpression;
            //var valueExp = fieldEntity.ValueExpression;
            //var dataSource = fieldEntity.DataSourceID;

            //if (String.IsNullOrWhiteSpace(dataSource) ||
            //    String.IsNullOrWhiteSpace(textExp) ||
            //    String.IsNullOrWhiteSpace(valueExp))
            //    return Convert.ToString(value);

            //var userID = DataConverter.ToNullableGuid(formData[FormDataConstants.UserIDField]);

            //var dataSourceHelper = new DataSourceHelper(userID, fieldEntity);

            //var values = new[] { value };
            //if (value is IEnumerable && !(value is String))
            //{
            //    var collection = (IEnumerable)value;
            //    values = collection.Cast<Object>().ToArray();
            //}

            //var dataRecords = dataSourceHelper.FindDataRecords(values);
            //if (dataRecords == null)
            //    return Convert.ToString(value);

            //var controlsQuery = controlsLp.SelectMany(n => n);

            //var expGlobal = new ExpressionGlobalsUtil(formData.UserID, controlsQuery, formData);

            //var texts = GetLabelTexts(dataRecords, expGlobal, textExp);
            //var result = String.Join("; ", texts);

            //return result;
        }

        protected IEnumerable<String> GetLabelTexts(IEnumerable<FormDataBase> dataRecords, ExpressionGlobalsUtil expGlobal, String textExpression)
        {
            var expNode = ExpressionParser.GetOrParse(textExpression);

            foreach (var dataRecord in dataRecords)
            {
                expGlobal.AddSource(dataRecord);

                var result = ExpressionEvaluator.TryEval(expNode, expGlobal.Eval);
                if (result.Error != null)
                    yield return $"[TextExpression error] - {result.Error.Message}";
                else
                    yield return Convert.ToString(result.Value);

                expGlobal.RemoveSource(dataRecord);
            }
        }

        protected IList<String> CheckCommonFormsConfig(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict, IDictionary<Guid?, String> formNames)
        {
            var errors = new List<String>();

            foreach (var pair in controlsDict)
            {
                var formName = formNames.GetValueOrDefault(pair.Key);
                var controlsLp = pair.Value;

                var primaryDirectionField = controlsLp[primaryDirectionFieldAlias].FirstOrDefault();
                if (primaryDirectionField == null)
                {
                    errors.Add($"Unable to find field '{primaryDirectionFieldAlias}' in form '{formName}'");
                }

                var secondDirectionField = controlsLp[secondDirectionFieldAlias].FirstOrDefault();
                if (secondDirectionField == null)
                {
                    errors.Add($"Unable to find field '{secondDirectionFieldAlias}' in form '{formName}'");
                }

                var thirdDirectionField = controlsLp[thirdDirectionFieldAlias].FirstOrDefault();
                if (thirdDirectionField == null)
                {
                    errors.Add($"Unable to find field '{thirdDirectionFieldAlias}' in form '{formName}'");
                }

                var keywordsGrid = controlsLp[keywordGridAlias].FirstOrDefault();
                if (keywordsGrid == null)
                {
                    errors.Add($"Unable to find grid '{keywordGridAlias}' in form '{formName}'");
                }
                else
                {
                    var keywordsGridControls = FormStructureUtil.PreOrderTraversal(keywordsGrid);
                    var keywordsGridFields = keywordsGridControls.ToLookup(n => n.Alias);

                    var keywordField = keywordsGridFields[keywordFieldAlias].FirstOrDefault();
                    if (keywordField == null)
                    {
                        errors.Add($"Unable to find column '{keywordFieldAlias}' in form '{formName}'");
                    }
                }
            }

            return errors;
        }

        protected IList<String> CheckProfileFormsConfig(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict, IDictionary<Guid?, String> formNames)
        {
            var errors = new List<String>();

            foreach (var pair in controlsDict)
            {
                var formName = formNames.GetValueOrDefault(pair.Key);
                var controlsLp = pair.Value;

                var employmentGrid = controlsLp[employmentGridAlias].FirstOrDefault();
                if (employmentGrid == null)
                {
                    errors.Add($"Unable to find grid '{employmentGridAlias}' in form '{formName}'");
                }
                else
                {
                    var employmentGridControls = FormStructureUtil.PreOrderTraversal(employmentGrid);
                    var employmentGridFields = employmentGridControls.ToLookup(n => n.Alias);

                    var workPlaceField = employmentGridFields[workPlaceFieldAlias].FirstOrDefault();
                    var workDepartmentField = employmentGridFields[workDepartmentFieldAlias].FirstOrDefault();

                    if (workPlaceField == null)
                    {
                        errors.Add($"Unable to find column '{workPlaceFieldAlias}' in form '{formName}'");
                    }

                    if (workDepartmentField == null)
                    {
                        errors.Add($"Unable to find column '{workDepartmentFieldAlias}' in form '{formName}'");
                    }
                }
            }

            return errors;
        }

        protected IList<String> CheckProjectFormsConfig(IDictionary<Guid?, ILookup<String, ControlEntity>> controlsDict, IDictionary<Guid?, String> formNames)
        {
            var errors = new List<String>();
            var controlsLp = controlsDict[FormID];

            var formName = formNames.GetValueOrDefault(FormID);

            var primaryPersonnelGrid = controlsLp[primaryPersonnelGridAlias].FirstOrDefault();
            if (primaryPersonnelGrid == null)
            {
                errors.Add($"Unable to find grid '{primaryPersonnelGridAlias}' in form '{formName}'");
            }
            else
            {
                var primaryPersonnelGridControls = FormStructureUtil.PreOrderTraversal(primaryPersonnelGrid);
                var primaryPersonnelGridFields = primaryPersonnelGridControls.ToLookup(n => n.Alias);

                var personnelField = primaryPersonnelGridFields[primaryPersonnelFieldAlias].FirstOrDefault();
                if (personnelField == null)
                {
                    errors.Add($"Unable to find column '{primaryPersonnelFieldAlias}' in form '{formName}'");
                }
            }

            return errors;
        }

    }
}