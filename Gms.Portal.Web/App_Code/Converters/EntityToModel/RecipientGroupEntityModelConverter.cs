using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class RecipientGroupEntityModelConverter : SingleModelConverterBase<GM_RecipientGroup, RecipientGroupModel>
    {
        public RecipientGroupEntityModelConverter(ISession session) : base(session)
        {
        }

        public override RecipientGroupModel Convert(GM_RecipientGroup source)
        {
            var target = new RecipientGroupModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(RecipientGroupModel target, GM_RecipientGroup source)
        {
            target.ID = source.ID;
            target.Type = source.Type;
            target.Name = source.Name;
            target.FormID = source.FormID;
            target.Expression = source.Expression;
            target.Description = source.Description;
        }
    }
}