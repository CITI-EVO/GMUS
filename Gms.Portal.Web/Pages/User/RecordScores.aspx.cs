using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Security;
using DevExpress.XtraPrinting.Native;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Utils;
using MongoDB.Bson;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.User
{
    public partial class RecordScores : BasePage
    {


        private FormEntity _formEntity;
        public FormEntity FormEntity
        {
            get
            {
                if (_formEntity == null)
                    _formEntity = GetFormEntity();

                return _formEntity;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            InitStructure();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                var ratingData = recordScoresControl.GetFormData();
                recordScoresControl.BindData(ratingData);

                return;
            }

            BindData();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            String message;
            if (!SaveScoresData(null, out message))
            {
                lblResult.Text = message;

                BindData();
                return;
            }

            Response.Redirect("~/Pages/User/FullExpertDataGrid.aspx");
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/User/FullExpertDataGrid.aspx");
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            var ratesData = recordScoresControl.GetFormData();

            if (!ValidateData(ratesData))
                return;

            var scoresQuery = (from n in ratesData
                               where n.Key.StartsWith("score")
                               let v = DataConverter.ToNullableInt(n.Value)
                               select v.GetValueOrDefault());

            var scoresSum = scoresQuery.Sum();

            var text = $"Thank you for your evaluation.<br/>The total score is {scoresSum}.<br/>Please note, that you will be <b>UNABLE</b> to change your evaluation after submission";
            lblConfirmText.Text = text;

            mpeConfirmAccept.Show();
        }

        private bool ValidateData(IDictionary<String, Object> ratesData)
        {
            var rates = FormEntity.Rating.Rates;
            var list = new List<String>();

            foreach (var rate in rates)
            {
                if (rate.ParentID == null)
                    continue;

                var scoreKey = $"score_{rate.ID}";
                var commentKey = $"comment_{rate.ID}";

                var scoreVal = ratesData.GetValueOrDefault(scoreKey);
                var commentVal = ratesData.GetValueOrDefault(commentKey);

                var score = DataConverter.ToNullableInt32(scoreVal);
                if (score == null)
                    list.Add($"{rate.Number}: score is mandatory");
                else if (score < rate.MinScore || score > rate.MaxScore)
                    list.Add($"{rate.Number}: score not valid (your score: {score}; min score: {rate.MinScore},  max score: {rate.MaxScore}");

                var comment = DataConverter.ToString(commentVal);
                if (String.IsNullOrWhiteSpace(comment))
                    list.Add($"{rate.Number}: comment is mandatory");
            }

            if (list.Count > 0)
            {
                lblResult.Text = String.Join("<br/>", list);
                return false;
            }

            return true;
        }

        protected void InitStructure()
        {
            var rating = FormEntity.Rating;
            if (rating == null || rating.Rates == null)
                return;

            recordScoresControl.InitStructure(rating.Rates);
        }

        protected void BindData()
        {
            var formID = DataConverter.ToNullableGuid(RequestUrl["formID"]);
            if (formID == null)
                return;

            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);
            if (recordID == null)
                return;

            var userID = DataConverter.ToNullableGuid(RequestUrl["userID"]);
            if (userID == null)
                return;

            var docuemnt = MongoDbUtil.GetDocument(formID, recordID);
            var formData = BsonDocumentConverter.ConvertToFormDataUnit(docuemnt);

            var ratingData = new Dictionary<String, Object>();

            if (formData != null && formData.UserStatuses != null)
            {
                var userStatus = formData.UserStatuses.FirstOrDefault(n => n.UserID == userID);
                if (userStatus != null && userStatus.Params != null)
                {
                    if ((userStatus.StatusID == DataStatusCache.Submit.ID ||
                         userStatus.StatusID == DataStatusCache.Rejected.ID) && !UmUtil.Instance.CurrentUser.IsSuperAdmin)
                    {
                        btnSave.Visible = false;
                        btnSubmit.Visible = false;

                        pnlRecordScores.Enabled = false;
                    }

                    foreach (var pair in userStatus.Params)
                        ratingData.Add(pair.Key, pair.Value);
                }
            }

            recordScoresControl.BindData(ratingData);

            var mode = DataConverter.ToString(RequestUrl["mode"]);
            if (mode == "View")
            {
                btnSave.Visible = false;
                btnSubmit.Visible = false;

                pnlRecordScores.Enabled = false;
            }
        }

        protected FormEntity GetFormEntity()
        {
            var formID = DataConverter.ToNullableGuid(RequestUrl["formID"]);
            if (formID == null)
                return null;

            var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
            if (dbForm == null)
                return null;

            var converter = new FormEntityModelConverter(HbSession);
            var formModel = converter.Convert(dbForm);

            return formModel.Entity;
        }

        protected bool SaveScoresData(Guid? statusID, out String message)
        {
            message = String.Empty;

            var formID = DataConverter.ToNullableGuid(RequestUrl["formID"]);
            if (formID == null)
                return false;

            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);
            if (recordID == null)
                return false;

            var userID = DataConverter.ToNullableGuid(RequestUrl["userID"]);
            if (userID == null)
                return false;

            var ratesData = recordScoresControl.GetFormData();

            var document = MongoDbUtil.GetDocument(formID, recordID);

            BsonValue bsonValue;
            if (!document.TryGetValue(FormDataConstants.UserStatusesFields, out bsonValue))
                return false;

            var oldArray = BsonDocumentConverter.ConvertToBsonArray(bsonValue);
            if (oldArray == null)
                oldArray = new BsonArray();

            var formStatuses = BsonDocumentConverter.ConvertToFormStatuses(oldArray).ToList();

            var query = (from n in formStatuses
                         where n.UserID == UserUtil.GetCurrentUserID() &&
                               n.Params != null &&
                               n.Params.ContainsKey(FormDataConstants.ScoringParams)
                         select n);

            var formStatus = query.FirstOrDefault();
            if (formStatus == null)
                return false;

            formStatus.UserID = userID;
            formStatus.StatusID = (statusID ?? formStatus.StatusID);
            formStatus.DateOfStatus = DateTime.Now;

            foreach (var pair in ratesData)
                formStatus.Params[pair.Key] = pair.Value;

            var newArray = BsonDocumentConverter.ConvertToFormStatusesArray(formStatuses);
            document[FormDataConstants.UserStatusesFields] = newArray;

            MongoDbUtil.UpdateDocument(formID, document);
            return true;
        }

        protected void btnConfirmAcceptOK_OnClick(object sender, EventArgs e)
        {
            String message;
            if (!SaveScoresData(DataStatusCache.Submit.ID, out message))
            {
                lblResult.Text = message;
                BindData();
                return;
            }

            Response.Redirect("~/Pages/User/FullExpertDataGrid.aspx");
        }

        protected void btnConfirmAcceptCancel_OnClick(object sender, EventArgs e)
        {
            mpeConfirmAccept.Hide();
        }
    }
}