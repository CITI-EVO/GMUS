namespace CITI.EVO.UserManagement.Web.Models
{
    public class CreateUserModel
    {
        public UserModel User { get; set; }

        public SelectGroupsModel Groups { get; set; }
    }
}