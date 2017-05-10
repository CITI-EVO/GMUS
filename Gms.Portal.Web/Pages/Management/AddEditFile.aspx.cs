using System;
using System.Linq;
using System.Net.Mime;
using System.Web;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Models;
using NHibernate.Linq;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class AddEditFile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillFilesGrid();
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            var model = new FileModel();
            model.ParentID = DataConverter.ToGuid(RequestUrl["ParentID"]);

            fileControl.Model = model;
            mpeFile.Show();
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Management/FormsList.aspx");
        }

        protected void filesControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Get<GM_File>(e.Value);
            if (entity == null)
                return;

            var converter = new FileEntityModelConverter(HbSession);
            var model = converter.Convert(entity);

            fileControl.Model = model;
            mpeFile.Show();
        }

        protected void filesControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_File>().FirstOrDefault(n => n.ID == e.Value);
            if (entity != null)
                entity.DateDeleted = DateTime.Now;

            HbSession.SubmitChanges(entity);

            FillFilesGrid();
        }

        protected void filesControl_OnDownload(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_File>().FirstOrDefault(n => n.ID == e.Value);
            Download(entity);
        }

        protected void btnFileOK_OnClick(object sender, EventArgs e)
        {
            var model = fileControl.Model;

            var converter = new FileModelEntityConverter(HbSession);

            var entity = HbSession.Get<GM_File>(model.ID);
            if (entity == null)
                entity = converter.Convert(model);
            else
                converter.FillObject(entity, model);

            HbSession.SubmitChanges(entity);

            FillFilesGrid();

            mpeFile.Hide();
        }

        protected void btnFileCancel_OnClick(object sender, EventArgs e)
        {
            mpeFile.Hide();
        }

        private void FillFilesGrid()
        {
            var parentId = DataConverter.ToNullableGuid(RequestUrl["ParentID"]);

            var entities = (from n in HbSession.Query<GM_File>()
                            where n.DateDeleted == null && n.ParentID == parentId
                            orderby n.DateCreated descending
                            select n).ToList();

            var converter = new FileEntityModelConverter(HbSession);

            var models = (from n in entities
                          let m = converter.Convert(n)
                          select m).ToList();

            var model = new FilesModel();
            model.List = models;

            filesControl.Model = model;
            filesControl.DataBind();
        }

        private void Download(GM_File file)
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";

            var contentDisposition = new ContentDisposition();
            contentDisposition.FileName = HttpUtility.UrlPathEncode(file.FileName);
            contentDisposition.Size = file.FileData.Length;
            Response.Headers["content-disposition"] = contentDisposition.ToString();
            Response.BinaryWrite(file.FileData);
            Response.End();
        }

    }
}