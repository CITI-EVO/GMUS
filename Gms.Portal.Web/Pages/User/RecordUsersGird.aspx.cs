using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using CITI.EVO.Tools.Comparers;

namespace Gms.Portal.Web.Pages.User
{
    public partial class RecordUsersGird : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var fields = new HashSet<String>
            {
                "UserName",
                "StatusName",
                "DateOfAssigne",
                "DateOfStatus",
                "Scores",
            };

            var result = LoadData();
            var dataView = new DictionaryDataView(result, fields);

            var model = new RecordUsersGridModel
            {
                DataView = dataView
            };

            recordUsersGridControl.Model = model;
            recordUsersGridControl.DataBind();
        }

        protected IEnumerable<IDictionary<String, Object>> LoadData()
        {
            var formID = DataConverter.ToNullableGuid(RequestUrl["formID"]);
            if (formID == null)
                yield break;

            var recordID = DataConverter.ToNullableGuid(RequestUrl["recordID"]);
            if (recordID == null)
                yield break;

            var docuemnt = MongoDbUtil.GetDocument(formID, recordID);
            var formData = BsonDocumentConverter.ConvertToFormDataUnit(docuemnt);

            if (formData.UserStatuses == null)
                yield break;

            var statusComparer = new ComparisonComparer<FormStatusUnit>(CompareFormStatusDates);
            var orderedStatuses = formData.UserStatuses.OrderBy(n => n, statusComparer);

            foreach (var statusUnit in orderedStatuses)
            {
                var dict = new Dictionary<String, Object>
                {
                    ["UserName"] = GetUserName(statusUnit.UserID),
                    ["StatusName"] = GetStatusName(statusUnit.StatusID),
                    ["DateOfAssigne"] = statusUnit.DateOfAssigne,
                    ["DateOfStatus"] = statusUnit.DateOfStatus,
                    ["Scores"] = GetScores(statusUnit)
                };

                yield return dict;
            }
        }

        protected int CompareFormStatusDates(FormStatusUnit x, FormStatusUnit y)
        {
            if (x == null && y != null)
                return 1;

            if (x != null && y == null)
                return -1;

            if (x == null && y == null)
                return 0;

            if (x.DateOfStatus == null && y.DateOfStatus != null)
                return 1;

            if (x.DateOfStatus != null && y.DateOfStatus == null)
                return -1;

            if (x.DateOfStatus == null && y.DateOfStatus == null)
                return 0;

            return x.DateOfStatus.Value.CompareTo(y.DateOfStatus.Value);
        }

        protected String GetUserName(Guid? userID)
        {
            var user = UmUsersCache.GetUser(userID);
            if (user == null)
                return null;

            return $"{user.LoginName} - {user.FirstName} {user.LastName}";
        }

        protected String GetStatusName(Guid? statusID)
        {
            var status = DataStatusCache.GetStatus(statusID);
            if (status == null)
                return null;

            return status.Name;

        }

        protected String GetScores(FormStatusUnit statusUnit)
        {
            if (statusUnit == null || statusUnit.Params == null)
                return String.Empty;

            var query = (from n in statusUnit.Params
                         where n.Key.StartsWith("score")
                         let m = DataConverter.ToNullableInt(n.Value)
                         where m != null
                         select m);

            var value = query.Sum();
            return Convert.ToString(value);
        }
    }
}