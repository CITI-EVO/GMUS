using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Extensions;
using CITI.EVO.Tools.Utils;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Entities.Others;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Models.Helpers;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;

namespace Gms.Portal.Web.Controls.Management
{
    public partial class ElementPasteControl : BaseUserControlExtend<ElementPasteModel>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void SetModel(ElementPasteModel model)
        {
            cbxForms.DataSource = model.Forms;
            cbxForms.DataBind();

            cbxMovingElements.DataSource = model.FormTree;
            cbxMovingElements.DataBind();
            base.SetModel(model);
        }

        protected void cbxForms_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnDataChanged(e);
        }
    }
}