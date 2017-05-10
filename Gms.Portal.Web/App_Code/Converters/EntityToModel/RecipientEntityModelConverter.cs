using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class RecipientEntityModelConverter : SingleModelConverterBase<GM_Recipient, RecipientModel>
    {
        public RecipientEntityModelConverter(ISession session) : base(session)
        {
        }

        public override RecipientModel Convert(GM_Recipient source)
        {
            var target = new RecipientModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(RecipientModel target, GM_Recipient source)
        {
            target.ID = source.ID;
            target.GroupID = source.GroupID;
            target.UserID = source.UserID;
            var user = UmUsersCache.GetUser(source.UserID);
            if (user != null)
            {
                target.UserName = user.LoginName;
                target.FirstName = user.FirstName;
                target.LastName = user.LastName;
                target.Phone = user.Phone;
                target.Email = user.Email;
            }
        }
    }
}