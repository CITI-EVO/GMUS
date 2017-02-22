using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Helpers;
using CITI.EVO.Tools.Utils;
using CITI.EVO.Tools.Web.UI.Helpers;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
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

        public Guid? UserID
        {
            get
            {
                if (UserUtil.IsSuperAdmin())
                    return DataConverter.ToNullableGuid(RequestUrl["ParentID"]);

                return UserUtil.GetCurrentUserID();
            }
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

        protected void formDataGridControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "Edit";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = e.Value;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = RequestUrl.ToEncodedUrl();

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var urlHelper = new UrlHelper("~/Pages/User/FormDataView.aspx");
            urlHelper["Mode"] = "View";
            urlHelper["FormID"] = FormID;
            urlHelper["OwnerID"] = (OwnerID ?? FormID);
            urlHelper["RecordID"] = e.Value;
            urlHelper["ParentID"] = ParentID;
            urlHelper["ReturnUrl"] = RequestUrl.ToEncodedUrl();

            Response.Redirect(urlHelper.ToEncodedUrl());
        }

        protected void formDataGridControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var collection = MongoDbUtil.GetCollection(OwnerID);

            var filter = Builders<BsonDocument>.Filter.Eq("ID", e.Value);
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

            var formEntity = model.Entity;
            if (formEntity == null)
                return;

            var controls = FormStructureUtil.OrderedFirstLevelTraversal(formEntity);

            var list = (from n in controls.OfType<FieldEntity>()
                        where n.Visible && 
                              n.DisplayOnGrid
                        select n).ToList();

            var formFields = list.Select(n => Convert.ToString(n.ID)).Union(FormDataUnit.DefaultFields);
            var fieldSet = formFields.ToHashSet();

            var userID = UserID;
            if (!UserUtil.IsSuperAdmin())
                userID = UserUtil.GetCurrentUserID();

            var formDataList = new FormDataLazyList(FormID, OwnerID, ParentID, userID);
            var formDataView = new DictionaryDataView(formDataList, fieldSet);

            var dataGridModel = new FormDataGridModel
            {
                Fields = list,
                DataView = formDataView
            };

            formDataGridControl.Model = dataGridModel;
            formDataGridControl.DataBind();
        }
    }
}