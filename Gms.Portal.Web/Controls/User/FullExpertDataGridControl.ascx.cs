using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Cache;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.ExpressionEngine;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Enums;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Comparers;
using Gms.Portal.Web.Entities.FormStructure;

namespace Gms.Portal.Web.Controls.User
{
    public partial class FullExpertDataGridControl : BaseUserControlExtend<FullExpertDataGridModel>
    {
        public event EventHandler<GenericEventArgs<String>> Message;
        protected virtual void OnMessage(GenericEventArgs<String> e)
        {
            if (Message != null)
                Message(this, e);
        }

        public event EventHandler<GenericEventArgs<String>> Accept;
        protected virtual void OnAccept(GenericEventArgs<String> e)
        {
            if (Accept != null)
                Accept(this, e);
        }

        public event EventHandler<GenericEventArgs<String>> Reject;
        protected virtual void OnReject(GenericEventArgs<String> e)
        {
            if (Reject != null)
                Reject(this, e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnMessage_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);
            OnMessage(new GenericEventArgs<String>(commandArg));
        }

        protected void btnAccept_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);
            OnAccept(new GenericEventArgs<String>(commandArg));
        }

        protected void btnReject_OnCommand(object sender, CommandEventArgs e)
        {
            var commandArg = Convert.ToString(e.CommandArgument);
            OnReject(new GenericEventArgs<String>(commandArg));
        }

        protected void gridView_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            var descriptor = e.Row.DataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return;

            var myStatuses = (from n in userStatuses
                              where n.Params != null &&
                                    n.Params.ContainsKey(FormDataConstants.ScoringParams)
                              select n);

            var myStatus = myStatuses.FirstOrDefault();
            if (myStatus == null)
                return;

            if (myStatus.StatusID == DataStatusCache.Submit.ID)
                e.Row.BackColor = Color.FromArgb(0, 237, 234, 230);

            if (myStatus.StatusID == DataStatusCache.Rejected.ID)
                e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);

            if (myStatus.StatusID == DataStatusCache.Accepted.ID)
                e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);
        }

        protected bool GetViewVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                var statuses = (from n in userStatuses
                                where n.UserID == UserUtil.GetCurrentUserID() &&
                                      n.Params != null &&
                                      n.Params.ContainsKey(FormDataConstants.ScoringParams)
                                select n);

                var status = statuses.FirstOrDefault();
                if (status != null && status.StatusID == DataStatusCache.Accepted.ID)
                    return true;
            }

            return false;
        }

        protected bool GetScoresVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                var statuses = (from n in userStatuses
                                where n.UserID == UserUtil.GetCurrentUserID() &&
                                      n.Params != null &&
                                      n.Params.ContainsKey(FormDataConstants.ScoringParams)
                                select n);

                var status = statuses.FirstOrDefault();
                if (status != null && status.StatusID == DataStatusCache.Accepted.ID)
                    return true;
            }

            return false;
        }

        protected bool GetPrintVisible(object dataItem)
        {
            return true;
        }

        protected bool GetDetailsVisible(object dataItem)
        {
            return UmUtil.Instance.HasAccess("Admin");
        }

        protected bool GetMessageVisible(object dataItem)
        {
            return true;
        }

        protected bool GetAcceptVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                var statuses = (from n in userStatuses
                                where n.UserID == UserUtil.GetCurrentUserID() &&
                                      n.Params != null &&
                                      n.Params.ContainsKey(FormDataConstants.ScoringParams)
                                select n);

                var status = statuses.FirstOrDefault();
                if (status != null &&
                    status.StatusID != DataStatusCache.Submit.ID &&
                    status.StatusID != DataStatusCache.Accepted.ID)
                {
                    return true;
                }
            }

            return false;
        }

        protected bool GetRejectVisible(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return false;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return false;

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                var statuses = (from n in userStatuses
                                where n.UserID == UserUtil.GetCurrentUserID() &&
                                      n.Params != null &&
                                      n.Params.ContainsKey(FormDataConstants.ScoringParams)
                                select n);

                var status = statuses.FirstOrDefault();
                if (status != null &&
                    status.StatusID != DataStatusCache.Submit.ID &&
                    status.StatusID != DataStatusCache.Rejected.ID)
                {
                    return true;
                }
            }

            return false;
        }

        protected String GetUserName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var userID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.UserIDField));
            if (userID == null)
                return null;

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetFormExperts(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return String.Empty;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return String.Empty;

            var experts = (from n in userStatuses
                           where n.Params != null &&
                                 n.Params.ContainsKey(FormDataConstants.ScoringParams)
                           let m = UmUsersCache.GetUser(n.UserID)
                           where m != null
                           let name = $"{m.LoginName} - {m.FirstName} {m.LastName}"
                           orderby name
                           select new
                           {
                               UserID = n.UserID,
                               Params = n.Params,
                               StatusID = n.StatusID,
                               LoginName = m.LoginName,
                               UserName = $"{m.FirstName} {m.LastName}",
                           });

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                experts = (from n in experts
                           where n.UserID == UserUtil.GetCurrentUserID()
                           select n);
            }
            else
            {
                experts = (from n in experts
                           where n.UserID != null
                           select n);
            }

            var emptyDict = new Dictionary<String, Object>();
            var container = new HtmlGenericControl("div");

            foreach (var item in experts)
            {
                var status = DataStatusCache.GetStatus(item.StatusID);
                status = (status ?? DataStatusCache.None);

                var @params = (item.Params ?? emptyDict);

                var scoresQuery = (from n in @params
                                   let v = DataConverter.ToNullableInt32(n.Value)
                                   where v != null && n.Key.StartsWith("score")
                                   select v);

                var row = new HtmlGenericControl("div");

                var icon = new HtmlGenericControl("i");
                icon.Attributes["title"] = status.Name;
                icon.Attributes["class"] = GetIconName(item.StatusID);
                icon.Attributes["style"] = "padding-right: 5px;";

                var urlHelper = new UrlHelper("~/Pages/User/RecordScores.aspx")
                {
                    ["Mode"] = "View",
                    ["UserID"] = item.UserID,
                    ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                    ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                };

                var name = new HyperLink
                {
                    ToolTip = item.LoginName,
                    Text = $"{item.UserName} - {scoresQuery.Sum()}",
                    NavigateUrl = urlHelper.ToEncodedUrl()
                };

                row.Controls.Add(icon);
                row.Controls.Add(name);

                container.Controls.Add(row);
            }

            var result = container.RenderString();
            return result;
        }

        protected String GetIconName(Guid? statusID)
        {
            if (statusID == DataStatusCache.Submit.ID)
                return "fa fa-circle text-navy";

            if (statusID == DataStatusCache.Accepted.ID)
                return "fa fa-circle text-warning";

            if (statusID == DataStatusCache.Rejected.ID)
                return "fa fa-circle text-danger";

            return "fa fa-circle text-default";
        }

        protected String GetFormName(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return String.Empty;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return String.Empty;

            var formsCache = CommonObjectCache.InitObject("Forms", CommonCacheStore.Request, GetFormModels);

            var formModel = formsCache.GetValueOrDefault(formID.Value);
            if (formModel == null)
                return String.Empty;

            return formModel.Name;
        }

        protected String GetCommandArg(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var recordID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.IDField));
            if (recordID == null)
                return null;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return null;

            return $"{recordID}/{formID}";
        }

        protected String GetViewUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var returnUrl = RequestUrl.ToEncodedUrl();

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx")
            {
                ["Mode"] = Convert.ToString(FormMode.View),
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["OwnerID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["ReturnUrl"] = GmsCommonUtil.ConvertToBase64(returnUrl)
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetPrintUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var urlHelper = new UrlHelper("~/Handlers/PrintFormData.ashx")
            {
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
                ["TemplateID"] = GetTemplateID(descriptor),
                ["LoginToken"] = UmUtil.Instance.CurrentToken,
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetDetailsUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var urlHelper = new UrlHelper("~/Pages/User/RecordUsersGird.aspx")
            {
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetChangeScoresUrl(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return "#";

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return "#";

            var query = (from n in userStatuses
                         where n.UserID == UserUtil.GetCurrentUserID() &&
                               n.Params != null &&
                               n.Params.ContainsKey(FormDataConstants.ScoringParams)
                         select n);

            var status = query.FirstOrDefault();
            if (status == null)
                return "#";

            var urlHelper = new UrlHelper("~/Pages/User/RecordScores.aspx")
            {
                ["UserID"] = status.UserID,
                ["FormID"] = descriptor.GetValue(FormDataConstants.FormIDField),
                ["RecordID"] = descriptor.GetValue(FormDataConstants.IDField),
            };

            return urlHelper.ToEncodedUrl();
        }

        protected String GetSummaryScores(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return String.Empty;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return String.Empty;

            var userStatuses = descriptor.GetValue(FormDataConstants.UserStatusesFields) as IEnumerable<FormStatusUnit>;
            if (userStatuses == null)
                return String.Empty;

            if (!UmUtil.Instance.HasAccess("Admin"))
                return String.Empty;

            var model = Model;
            var entity = model.Entities.GetValueOrDefault(formID);

            if (entity == null || entity.Rates == null || entity.Rates.Count == 0)
                return String.Empty;

            if (String.IsNullOrWhiteSpace(entity.SelectorExpression) || String.IsNullOrWhiteSpace(entity.SummaryExpression))
                return String.Empty;

            var scores = GetRecordScores(userStatuses, entity).ToList();

            var selectorExpNode = ExpressionParser.GetOrParse(entity.SelectorExpression);
            var summaryExpNode = ExpressionParser.GetOrParse(entity.SummaryExpression);

            var advResolver = new AdvancedDataResolver();
            advResolver.SetValue("scores", scores);

            Object selectorResult;
            if (!ExpressionEvaluator.TryEval(selectorExpNode, advResolver, out selectorResult))
                return null;

            advResolver.SetValue("scores", selectorResult);

            Object summaryResult;
            if (!ExpressionEvaluator.TryEval(summaryExpNode, advResolver, out summaryResult))
                return null;

            return Convert.ToString(summaryResult);
        }

        protected String GetTemplateID(DictionaryItemDescriptor descriptor)
        {
            var formsCache = CommonObjectCache.InitObject("Forms", CommonCacheStore.Request, GetFormModels);

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            var formModel = formsCache.GetValueOrDefault(formID.Value);

            var formEntity = formModel.Entity;
            var ratingEntity = formEntity.Rating;

            if (ratingEntity == null)
                return null;

            return Convert.ToString(ratingEntity.ID);
        }

        protected int? GetDaysLeft(object dataItem)
        {
            var descriptor = dataItem as DictionaryItemDescriptor;
            if (descriptor == null)
                return null;

            var formID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.FormIDField));
            if (formID == null)
                return null;

            var statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.DateOfSubmitField));
            if (statusDate == null)
            {
                statusDate = DataConverter.ToNullableDateTime(descriptor.GetValue(FormDataConstants.DateOfStatusField));
                if (statusDate == null)
                    return null;
            }

            var currentStatusID = DataConverter.ToNullableGuid(descriptor.GetValue(FormDataConstants.StatusIDField));
            if (currentStatusID != DataStatusCache.Submit.ID)
                return null;

            var formDeadlines = GmsCommonUtil.GetFormDeadlines();
            if (formDeadlines == null)
                return null;

            var deadline = formDeadlines.GetValueOrDefault(formID.Value);
            if (deadline == null)
                return null;

            var diff = DateTime.Now - statusDate.Value;
            return (int)(deadline - diff.TotalDays);
        }

        protected IDictionary<Guid, FormModel> GetFormModels()
        {
            var query = (from n in HbSession.Query<GM_Form>()
                         where n.DateDeleted == null
                         select n);

            var converter = new FormEntityModelConverter(HbSession);

            var dict = new Dictionary<Guid, FormModel>();

            foreach (var item in query)
            {
                var model = converter.Convert(item);
                dict.Add(item.ID, model);
            }

            return dict;
        }

        protected IEnumerable<FormDataBase> GetRecordScores(IEnumerable<FormStatusUnit> userStatuses, RatingEntity entity)
        {
            var statusQuery = (from n in userStatuses
                               where n.Params != null &&
                                     n.Params.ContainsKey(FormDataConstants.ScoringParams)
                               select n);

            if (!UmUtil.Instance.HasAccess("Admin"))
            {
                statusQuery = (from n in statusQuery
                               where n.UserID == UserUtil.GetCurrentUserID()
                               select n);
            }

            var statusComparer = new ComparisonComparer<FormStatusUnit>(FormDataUtil.CompareFormStatusDates);
            var orderedStatuses = statusQuery.OrderBy(n => n, statusComparer);

            foreach (var item in orderedStatuses)
            {
                var statusName = String.Empty;

                var dataStatus = DataStatusCache.GetStatus(item.StatusID);
                if (dataStatus != null)
                    statusName = dataStatus.Name;

                var scoresQuery = (from n in entity.Rates
                                   where n.ParentID != null
                                   let k = $"score_{n.ID}"
                                   let v = item.Params.GetValueOrDefault(k)
                                   let p = DataConverter.ToNullableInt(v)
                                   select p.GetValueOrDefault());

                var scoresSum = scoresQuery.Sum();

                var dict = new FormDataBase(true)
                {
                    ["UserID"] = item.UserID,
                    ["UserName"] = GetUserName(item.UserID),
                    ["ScoresSum"] = scoresSum,
                    ["StatusName"] = statusName,
                    ["DateOfStatus"] = item.DateOfStatus,
                    ["DateOfAssigne"] = item.DateOfAssigne,
                };

                foreach (var rate in entity.Rates)
                {
                    if (rate.ParentID == null)
                        continue;

                    var scoreKey = ExpressionParser.Escape($"score_{rate.Number}");
                    var dataKey = $"score_{rate.ID}";

                    dict[scoreKey] = item.Params.GetValueOrDefault(dataKey);
                }

                yield return dict;
            }
        }
    }
}