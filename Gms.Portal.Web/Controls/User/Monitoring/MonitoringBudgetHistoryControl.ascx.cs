using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Monitoring;
using Gms.Portal.Web.Helpers;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringBudgetHistoryControl : BaseUserControl
    {
        private IDictionary<Guid?, BudgetParagraphEntity> _paragraphs;
        private IDictionary<Guid?, MonitoringFlewEntity> _flews;

        protected void Page_Init(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void gvData_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var entity = e.Row.DataItem as MoneyTransferEntity;
                if (entity != null)
                {
                    if (entity.Status == MonitoringItemStatuses.Accepted)
                        e.Row.BackColor = Color.FromArgb(0, 210, 255, 191);

                    if (entity.Status == MonitoringItemStatuses.Rejected)
                        e.Row.BackColor = Color.FromArgb(0, 255, 217, 191);
                }
            }
        }

        public void BindData(IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<MonitoringFlewEntity> flews, IEnumerable<MoneyTransferEntity> transfers)
        {
            _paragraphs = paragraphs.ToDictionary(n => n.ID);
            _flews = flews.ToDictionary(n => n.ID);

            gvData.DataSource = transfers;
            gvData.DataBind();
        }

        public IEnumerable<MonitoringBudgetDataEntity> GetData(DateTime? startDate, DateTime? endDate, IEnumerable<BudgetParagraphEntity> paragraphs, IEnumerable<IDictionary<String, Object>> dictionaries)
        {
            var transfers = dictionaries.Select(n => new MoneyTransferEntity(n));

            if (startDate != null)
            {
                transfers = (from n in transfers
                             where n.DateOfTransfer >= startDate
                             select n);
            }

            if (endDate != null)
            {
                transfers = (from n in transfers
                             where n.DateOfTransfer <= endDate
                             select n);
            }

            var result = (from n in transfers
                          select new MonitoringBudgetDataEntity
                          {
                              DateOfTransfer = n.DateOfTransfer,
                              Incoming = n.Incoming,
                              Outgoing = n.Outgoing,
                              Remain = n.Remain,
                              Paragraph = GetParagraphName(n.ParagraphID),
                              Goal = n.Goal ?? String.Empty,
                              Comment = n.Comment ?? String.Empty,
                              Status = n.Status ?? String.Empty,
                              StatusUser = GetUserName(n.StatusUserID) ?? String.Empty,
                              StatusDate = n.StatusDate,
                              Flaws = GetFlewsNames(n.FlawsID),
                              FlawsScore = GetFlewsScores(n.FlawsID),
                              DateCreated = n.DateCreated.GetValueOrDefault()
                          });

            return result;
        }

        protected String GetFlewsNames(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Name);

            var flews = String.Join("; ", query);
            return flews;
        }

        protected double? GetFlewsScores(object eval)
        {
            var flewsID = eval as IEnumerable<Guid?>;
            if (flewsID == null)
                return null;

            var query = (from n in flewsID
                         where n != null
                         let f = _flews.GetValueOrDefault(n)
                         where f != null
                         select f.Score);

            var sum = query.Sum();
            return sum;
        }

        protected String GetDatePart(object eval)
        {
            return $"{eval:dd.MM.yyyy}";
        }

        protected String GetUserName(object eval)
        {
            var userID = DataConverter.ToNullableGuid(eval);

            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            var name = $"{user.LoginName} - {user.FirstName} {user.LastName}";
            return name;
        }

        protected String GetParagraphName(object eval)
        {
            var itemID = DataConverter.ToNullableGuid(eval);
            if (itemID == null)
                return null;

            if (_paragraphs == null)
                return null;

            var entity = _paragraphs.GetValueOrDefault(itemID.Value);
            if (entity == null)
                return null;

            return entity.Name;
        }

        protected String GetCorrectName(Object val)
        {
            var name = Convert.ToString(val);

            if (String.IsNullOrWhiteSpace(name))
                return "[Name Is Empty]";

            return name;
        }

        protected Object GetControlValue(Control control)
        {
            if (control == null)
                return null;

            return Request.Form[control.UniqueID];
        }

        protected DictionaryDataView GenEmptyData(ISet<String> fields)
        {
            var dict = new Dictionary<String, Object>();
            foreach (var field in fields)
                dict[field] = String.Empty;

            var dataView = new DictionaryDataView(new[] { dict }, fields);
            return dataView;
        }
    }
}


