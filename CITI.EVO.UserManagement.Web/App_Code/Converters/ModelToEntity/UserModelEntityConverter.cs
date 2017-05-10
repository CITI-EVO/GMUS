using CITI.EVO.Core.Common;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.ModelToEntity
{
    public class UserModelEntityConverter : SingleModelConverterBase<UserModel, UM_User>
    {
        public UserModelEntityConverter(ISession session) : base(session)
        {
        }

        public override UM_User Convert(UserModel source)
        {
            var target = EntityFactory.CreateEntity<UM_User>();
            FillObject(target, source);

            return target;
        }

        public override void FillObject(UM_User target, UserModel source)
        {
            //target.ID = source.ID;
            target.LoginName = source.LoginName;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Address = source.Address;
            target.Email = source.Email;
            target.Phone = source.Phone;
            target.IsActive = source.IsActive.GetValueOrDefault();
            target.IsSuperAdmin = source.IsSuperAdmin.GetValueOrDefault();
            target.PasswordExpirationDate = source.PasswordExpire;
            target.UserCategoryID = source.UserCategoryID;
            target.UserCode = source.UserCode;

            if (source.ChangePassword.GetValueOrDefault())
                target.Password = source.Password;
        }
    }
}