using CITI.EVO.Core.Common;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Models;
using NHibernate;

namespace Gms.Portal.Web.Converters.EntityToModel
{
    public class UserMessageEntityModelConverter : SingleModelConverterBase<GM_UserMessage, UserMessageModel>
    {
        public UserMessageEntityModelConverter(ISession session) : base(session)
        {
        }

        public override UserMessageModel Convert(GM_UserMessage source)
        {
            var target = new UserMessageModel();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UserMessageModel target, GM_UserMessage source)
        {
            target.ID = source.ID;
            target.Subject = source.Subject;

            target.RecordID = source.RecordID;
            target.ParentID = source.ParentID;
            target.FormID = source.FormID;
            target.FromUserID = source.FromUserID;
            target.ToUserID = source.ToUserID;
            target.StatusUserID = source.StatusUserID;
            target.StatusID = source.StatusID;
            target.Readed = source.Readed;
            target.Text = source.Text;
            target.Description = source.Description;

            target.DateCreated = source.DateCreated;
            target.DateChanged = source.DateChanged;
            target.DateDeleted = source.DateDeleted;
        }
    }
}