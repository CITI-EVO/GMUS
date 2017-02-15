using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using CITI.EVO.UserManagement.Web.Bases;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gms.Portal.Web.Pages.User
{
    public partial class FormDataGrid : BasePage
    {
        public Guid? FormID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["FormID"]); }
        }

        public Guid? OwnerID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["OwnerID"]); }
        }

        public Guid? ParentID
        {
            get { return DataConverter.ToNullableGuid(RequestUrl["ParentID"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            FillGridView();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "Edit";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = null;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = RequestUrl.ToEncodedUrl();

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnEdit_OnCommand(object sender, CommandEventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (recordID == null)
                return;

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "Edit";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = recordID;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = RequestUrl.ToEncodedUrl();

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnView_OnCommand(object sender, CommandEventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (recordID == null)
                return;

            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "View";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = recordID;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = RequestUrl.ToEncodedUrl();

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void btnDelete_OnCommand(object sender, CommandEventArgs e)
        {
            var recordID = DataConverter.ToNullableGuid(e.CommandArgument);
            if (recordID == null)
                return;

            var collection = MongoDbUtil.GetCollection(OwnerID);

            var filter = Builders<BsonDocument>.Filter.Eq("ID", recordID);
            var update = Builders<BsonDocument>.Update.Set("DateDeleted", DateTime.Now);

            collection.UpdateMany(filter, update);

            FillGridView();
        }

        protected void FillGridView()
        {
            if (FormID == null)
                return;

            var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == FormID);
            if (dbForm == null)
                return;

            var converter = new FormEntityModelConverter(HbSession);
            var model = converter.Convert(dbForm);

            var formEntity = model.FormEntity;
            if (formEntity == null)
                return;

            var formDataList = new FormDataLazyList(FormID, OwnerID, ParentID);

            var collection = formDataList.Cast<IDictionary<String, Object>>();
            var fieldSet = formDataList.SelectMany(n => n.Keys).ToHashSet();

            var formDataView = new DictionaryDataView(collection, fieldSet);

            var controls = FormStructureUtil.PreOrderFirstLevelTraversal(formEntity);

            var fields = (from n in controls
                          let f = n as FieldEntity
                          where f != null
                          select f);

            var existFields = (from n in gvData.Columns.Cast<DataControlField>()
                               let c = n as BoundField
                               where c != null
                               select c.DataField).ToHashSet();

            foreach (var field in fields)
            {
                var dataField = Convert.ToString(field.ID);
                if (existFields.Contains(dataField))
                    continue;

                var column = new BoundField();
                column.HeaderText = field.Name;
                column.DataField = Convert.ToString(field.ID);

                gvData.Columns.Add(column);
            }

            gvData.DataSource = formDataView;
            gvData.DataBind();
        }
    }
}