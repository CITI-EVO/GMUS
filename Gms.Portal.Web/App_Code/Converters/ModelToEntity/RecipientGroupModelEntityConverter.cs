using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class RecipientGroupModelEntityConverter : SingleModelConverterBase<RecipientGroupModel, GM_RecipientGroup>
    {
        public RecipientGroupModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_RecipientGroup Convert(RecipientGroupModel source)
        {
            var target = EntityFactory.CreateEntity<GM_RecipientGroup>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_RecipientGroup target, RecipientGroupModel source)
        {
            target.Type = source.Type;
            target.Name = source.Name;
            target.FormID = source.FormID;
            target.Expression = source.Expression;
            target.Description = source.Description;
        }
    }
}