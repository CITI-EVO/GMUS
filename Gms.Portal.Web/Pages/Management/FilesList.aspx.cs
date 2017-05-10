using System;
using System.Linq;
using System.Net.Mime;
using System.Web;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Models;
using NHibernate.Linq;
using CITI.EVO.Tools.Helpers;
using Gms.Portal.Web.Utils;

namespace Gms.Portal.Web.Pages.Management
{
    public partial class FilesList : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillFilesGrid();
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

            filesDownloadControl.Model = model;
            filesDownloadControl.DataBind();
        }

        protected void filesDownloadControl_OnDownload(object sender, GenericEventArgs<Guid> e)
        {
            var entity = HbSession.Query<GM_File>().FirstOrDefault(n => n.ID == e.Value);
            Download(entity);
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

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            var returnUrl = DataConverter.ToString(RequestUrl["ReturnUrl"]);
            var cleanUrl = GmsCommonUtil.ConvertFromBase64(returnUrl);

            var returnUrlHelper = new UrlHelper(cleanUrl);
            Response.Redirect(returnUrlHelper.ToEncodedUrl());
        }
    }
}