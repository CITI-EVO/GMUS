using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class NotificationHistoryModelEntityConverter : SingleModelConverterBase<NotificationHistoryModel, GM_NotificationHistory>
    {
        public NotificationHistoryModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_NotificationHistory Convert(NotificationHistoryModel source)
        {
            var target = EntityFactory.CreateEntity<GM_NotificationHistory>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_NotificationHistory target, NotificationHistoryModel source)
        {
            target.RecipientID = source.RecipientID.GetValueOrDefault();
            target.UserID = source.UserID.GetValueOrDefault();
            target.Email = source.Email;
            target.Phone = source.Phone;
            target.ContactType = source.ContactType;
            target.Subject = source.Subject;
            target.Body = source.Body;
        }

    }
}