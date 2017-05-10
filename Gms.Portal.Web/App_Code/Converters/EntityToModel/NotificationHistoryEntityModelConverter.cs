using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class NotificationHistoryEntityModelConverter : SingleModelConverterBase<GM_NotificationHistory, NotificationHistoryModel>
    {
        public NotificationHistoryEntityModelConverter(ISession session) : base(session)
        {
        }

        public override NotificationHistoryModel Convert(GM_NotificationHistory source)
        {
            var target = new NotificationHistoryModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(NotificationHistoryModel target, GM_NotificationHistory source)
        {
            target.ID = source.ID;
            target.RecipientID = source.RecipientID;
            target.UserID = source.UserID;
            target.Phone = source.Phone;
            target.Email = source.Email;
            target.ContactType = source.ContactType;
            target.Subject = source.Subject;
            target.Body = source.Body;
            target.DateCreated = source.DateCreated;

            var user = UmUsersCache.GetUser(source.UserID);
            if (user != null)
            {
                target.FirstName = user.FirstName;
                target.LastName = user.LastName;
            }
        }
    }
}