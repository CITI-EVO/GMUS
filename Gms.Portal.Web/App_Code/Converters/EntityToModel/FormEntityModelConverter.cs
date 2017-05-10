using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Entities.FormStructure;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class FormEntityModelConverter : SingleModelConverterBase<GM_Form, FormModel>
    {
        public FormEntityModelConverter(ISession session) : base(session)
        {
        }

        public override FormModel Convert(GM_Form source)
        {
            var target = new FormModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(FormModel target, GM_Form source)
        {
            target.ID = source.ID;
            target.Name = source.Name;
            target.Number = source.Number;
            target.Visible = source.Visible;
            target.VisibleExpression = source.VisibleExpression;
            target.FillingValidationExpression = source.FillingValidationExpression;
            target.FillingValidationMessage = source.FillingValidationMessage;
            target.OrderIndex = source.OrderIndex;
            target.CategoryID = source.CategoryID;
            target.UserMode = source.UserMode;
            target.RequiresApprove = source.RequiresApprove;
            target.ApprovalDeadline = source.ApprovalDeadline;

            target.Entity = XmlUtil.Deserialize<FormEntity>(source.XmlData);
        }
    }
}