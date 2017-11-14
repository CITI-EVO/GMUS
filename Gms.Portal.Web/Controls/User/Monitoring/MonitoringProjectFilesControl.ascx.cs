using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.Comparers;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using LinkButton = CITI.EVO.Tools.Web.UI.Controls.LinkButton;

namespace Gms.Portal.Web.Controls.User.Monitoring
{
    public partial class MonitoringProjectItemControl : BaseUserControlExtend<MonitoringProjectFilesModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected string GetFileSize(object eval)
        {
            var fileData = eval as byte[];
            if (!fileData.IsNullOrEmpty())
                return $"{fileData.Length / 1024} kb";

            return "0 kb";
        }
        protected String GetUrl(object dataItem)
        {
            var entity = dataItem as FileEntity;
            if (entity == null)
                return "#";

            if (!String.IsNullOrWhiteSpace(entity.FileUrl))
                return entity.FileUrl;

            return $"~/Handlers/Download.ashx?FileID={Eval("ID")}";
        }
        protected bool HasFile(object eval)
        {
            var fileData = eval as byte[];
            return !fileData.IsNullOrEmpty();
        }

        public override void SetModel(MonitoringProjectFilesModel model)
        {
            gvData.DataSource = model.Files;
            gvData.DataBind();

            base.SetModel(model);
        }
        public override MonitoringProjectFilesModel GetModel()
        {
            var model = base.GetModel();
            model.PostedFiles = fuFiles.PostedFiles;

            return model;
        }

        protected bool GetDeleteVisible()
        {
            if (Model.Submited.GetValueOrDefault())
                return Model.Flaw.GetValueOrDefault();

            return true;
        }
    }
}