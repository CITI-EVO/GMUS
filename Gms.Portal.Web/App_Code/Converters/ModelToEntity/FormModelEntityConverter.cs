using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class FormModelEntityConverter : SingleModelConverterBase<FormModel, GM_Form>
    {
        public FormModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Form Convert(FormModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Form>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Form target, FormModel source)
        {
            //target.ID = source.ID;
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

            target.XmlData = XmlUtil.Serialize(source.Entity);
        }
    }
}