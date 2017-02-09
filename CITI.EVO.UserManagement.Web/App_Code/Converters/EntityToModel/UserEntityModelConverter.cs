
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Converters.Common;
using CITI.EVO.UserManagement.Web.Models;
using NHibernate;

namespace CITI.EVO.UserManagement.Web.Converters.EntityToModel
{
    public class UserEntityModelConverter : SingleModelConverterBase<UM_User, UserModel>
    {
        public UserEntityModelConverter(ISession session) : base(session)
        {
        }

        public override UserModel Convert(UM_User source)
        {
            var model = new UserModel();
            FillObject(model, source);

            return model;
        }

        public override void FillObject(UserModel target, UM_User source)
        {
            target.ID = source.ID;
            target.LoginName = source.LoginName;
            target.Password = source.Password;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
            target.Address = source.Address;
            target.Email = source.Email;
            target.IsActive = source.IsActive;
            target.IsSuperAdmin = source.IsSuperAdmin;
            target.PasswordExpire = source.PasswordExpirationDate;
            target.UserCategoryID = source.UserCategoryID;
            target.UserCode = source.UserCode;
        }
    }
}