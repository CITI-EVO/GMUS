using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class RecipientModelEntityConverter : SingleModelConverterBase<RecipientModel, GM_Recipient>
    {
        public RecipientModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_Recipient Convert(RecipientModel source)
        {
            var target = EntityFactory.CreateEntity<GM_Recipient>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_Recipient target, RecipientModel source)
        {
            target.UserID = source.UserID.GetValueOrDefault();
            target.GroupID = source.GroupID;
        }

    }
}