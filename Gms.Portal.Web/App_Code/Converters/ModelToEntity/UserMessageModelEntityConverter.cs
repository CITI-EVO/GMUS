using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.ModelToEntity
{
    public class UserMessageModelEntityConverter : SingleModelConverterBase<UserMessageModel, GM_UserMessage>
    {
        public UserMessageModelEntityConverter(ISession session) : base(session)
        {
        }

        public override GM_UserMessage Convert(UserMessageModel source)
        {
            var target = EntityFactory.CreateEntity<GM_UserMessage>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(GM_UserMessage target, UserMessageModel source)
        {
            //target.ID = source.ID;
            target.Subject = source.Subject;

            target.ParentID = source.ParentID;
            target.FormID = source.FormID;

            target.StatusUserID = source.StatusUserID;
            target.StatusID = source.StatusID;
            target.FromUserID = source.FromUserID;
            target.ToUserID = source.ToUserID;
            target.RecordID = source.RecordID;

            target.Readed = source.Readed;
            target.Text = source.Text;
            target.Description = source.Description;
        }
    }
}