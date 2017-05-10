using System;
using System.Linq;
using CITI.EVO.Tools.EventArguments;
using CITI.EVO.Tools.Security;
using CITI.EVO.Tools.Web.UI.Controls;
using CITI.EVO.UserManagement.DAL.Domain;
using CITI.EVO.UserManagement.Web.Bases;
using CITI.EVO.UserManagement.Web.Converters.EntityToModel;
using CITI.EVO.UserManagement.Web.Converters.ModelToEntity;
using CITI.EVO.UserManagement.Web.Models;
using CITI.EVO.UserManagement.Web.Utils;
using NHibernate.Linq;
using CITI.EVO.Tools.Extensions;

namespace CITI.EVO.UserManagement.Web.Pages.Management
{
    public partial class UsersList : BasePage
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyPermissions();

            FillUserGrid();
        }

        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            var model = new CreateUserModel
            {
                User = new UserModel
                {
                    Password = "resetpass",
                    PasswordExpire = DateTime.Now,
                    ChangePassword = true,
                }
            };

            btUserOK.Visible = true;
            pnlCreateUser.Enabled = true;

            createUserControl.Model = model;
            mpeUserForm.Show();
        }

        protected void btnUserOK_Click(object sender, EventArgs e)
        {
            var model = createUserControl.Model;

            if (!ValidateUser(model))
            {
                mpeUserForm.Show();
                return;
            }

            var user = HbSession.Query<UM_User>().FirstOrDefault(n => n.ID == model.User.ID);
            if (user == null)
            {
                user = new UM_User
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                };
            }

            var converter = new UserModelEntityConverter(HbSession);
            converter.FillObject(user, model.User);

            var groups = model.Groups;
            if (groups != null && groups.Groups != null && groups.Groups.GroupsID != null)
            {

                foreach (var groupID in groups.Groups.GroupsID)
                {
                    var groupUser = new UM_GroupUser
                    {
                        ID = Guid.NewGuid(),
                        DateCreated = DateTime.Now,
                        GroupID = groupID.Value,
                        UserID = user.ID,
                        AccessLevel = groups.AccessLevel
                    };

                    user.GroupUsers.Add(groupUser);
                }
            }

            HbSession.SubmitChanges(user);

            FillUserGrid();

        }

        protected void btSetAttributesOK_Click(object sender, EventArgs eventArgs)
        {
            var model = objectAttributeControl.Model;

            using (var transaction = HbSession.BeginTransaction())
            {
                var oldAttribute = GetRelatedAttributes(model.ParentID, model.FieldID);
                if (oldAttribute != null)
                {
                    oldAttribute.DateDeleted = DateTime.Now;
                    HbSession.Update(oldAttribute);
                }

                var converter = new AttributeValueModelEntityConverter(HbSession);
                var newAttribute = converter.Convert(model);

                HbSession.Save(newAttribute);

                transaction.Commit();
            }

            mpeSetAttributes.Hide();
        }

        protected void btSetAttributesCancel_OnClick(object sender, EventArgs e)
        {
            mpeSetAttributes.Hide();
        }

        protected void btViewAttributesCancel_OnClick(object sender, EventArgs e)
        {
            mpeViewAttributes.Hide();
        }

        protected void btnBindData_Click(object sender, EventArgs e)
        {
            FillUserGrid();
        }

        protected void usersControl_OnView(object sender, GenericEventArgs<Guid> e)
        {
            var user = HbSession.Query<UM_User>().FirstOrDefault(n => n.ID == e.Value);
            if (user == null)
                return;

            var converter = new UserEntityModelConverter(HbSession);
            var userModel = converter.Convert(user);

            var createUserModel = new CreateUserModel
            {
                User = userModel
            };

            btUserOK.Visible = false;
            pnlCreateUser.Enabled = false;

            createUserControl.Model = createUserModel;

            mpeUserForm.Show();
        }

        protected void usersControl_OnEdit(object sender, GenericEventArgs<Guid> e)
        {
            var user = HbSession.Query<UM_User>().FirstOrDefault(n => n.ID == e.Value);
            if (user == null)
                return;

            var converter = new UserEntityModelConverter(HbSession);

            var userModel = converter.Convert(user);
            userModel.Password = String.Empty;

            var createUserModel = new CreateUserModel
            {
                User = userModel
            };

            btUserOK.Visible = true;
            pnlCreateUser.Enabled = true;

            createUserControl.Model = createUserModel;

            mpeUserForm.Show();
        }

        protected void usersControl_OnDelete(object sender, GenericEventArgs<Guid> e)
        {
            var user = HbSession.Query<UM_User>().FirstOrDefault(n => n.ID == e.Value);
            if (user == null)
                return;

            using (var transaction = HbSession.BeginTransaction())
            {
                user.DateDeleted = DateTime.Now;
                HbSession.Update(user);

                foreach (var item in user.GroupUsers)
                {
                    if (item.DateDeleted == null)
                    {
                        item.DateDeleted = user.DateDeleted;
                        HbSession.Update(user);
                    }
                }

                transaction.Commit();
            }

            FillUserGrid();
        }

        protected void usersControl_OnNewMessage(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void usersControl_OnSetAttribute(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void usersControl_OnViewAttributes(object sender, GenericEventArgs<Guid> e)
        {

        }

        protected void createUserControl_OnDataChanged(object sender, EventArgs e)
        {
            mpeUserForm.Show();
        }

        protected void btUserCancel_OnClick(object sender, EventArgs e)
        {
            mpeUserForm.Hide();
        }

        #endregion

        #region Methods

        protected UM_AttributeValue GetRelatedAttributes(Guid? parentID, Guid? attributeFieldID)
        {
            if (parentID == null || attributeFieldID == null)
                return null;

            var attributeValue = (from v in HbSession.Query<UM_AttributeValue>()
                                  where v.ParentID == parentID &&
                                        v.DateDeleted == null &&
                                        v.AttributeFieldID == attributeFieldID
                                  select v).FirstOrDefault();

            if (attributeValue == null)
                return null;

            return attributeValue;
        }

        protected bool ValidateUser(CreateUserModel model)
        {
            var userModel = model.User;

            if (String.IsNullOrWhiteSpace(userModel.LoginName))
            {
                lblUserError.Text = "მომხმარებლის სახელი";
                return false;
            }

            if (String.IsNullOrWhiteSpace(userModel.FirstName))
            {
                lblUserError.Text = "გთხოვთ შეავსეთ სახელი";
                return false;
            }

            if (String.IsNullOrWhiteSpace(userModel.LastName))
            {
                lblUserError.Text = "გთხოვთ შეავსეთ გვარი";
                return false;
            }

            if (String.IsNullOrWhiteSpace(userModel.Email))
            {
                lblUserError.Text = "გთხოვთ შეავსეთ ელ-ფოსტა";
                return false;
            }

            if (userModel.PasswordExpire == null)
            {
                lblUserError.Text = "გთხოვთ შეავსეთ პაროლის ვალიდურობის თარიღი";
                return false;
            }

            if (String.IsNullOrWhiteSpace(userModel.Address))
            {
                lblUserError.Text = "გთხოვთ შეავსეთ მისამართი";
                return false;
            }

            var count = (from n in HbSession.Query<UM_User>()
                         where n.LoginName.ToLower() == userModel.LoginName.ToLower() &&
                               n.DateDeleted == null &&
                               n.ID != userModel.ID
                         select n.ID).Count();

            if (count > 0)
            {
                lblUserError.Text = "მომხმარებელი მითითებული სახელით უკვე არსებობს";
                return false;
            }

            count = (from n in HbSession.Query<UM_User>()
                     where n.Email.ToLower() == userModel.Email.ToLower() &&
                           n.DateDeleted == null &&
                           n.ID != userModel.ID
                     select n.ID).Count();

            if (count > 0)
            {
                lblUserError.Text = "მომხმარებელი მითითებული ელ.ფოსტის მისამართით უკვე რეგისტრირებულია";
                return false;
            }

            return true;
        }

        protected void FillUserGrid()
        {
            var filterModel = usersFilterControl.Model;

            var keyword = filterModel.Keyword;

            var query = from n in HbSession.Query<UM_User>()
                        where n.DateDeleted == null
                        select n;

            if (filterModel.LoginName != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.LoginName.Contains(keyword)
                        select n;
            }

            if (filterModel.FirstName != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.FirstName.Contains(keyword)
                        select n;
            }

            if (filterModel.LastName != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.LastName.Contains(keyword)
                        select n;
            }

            if (filterModel.Email != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.Email.Contains(keyword)
                        select n;
            }

            if (filterModel.Address != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.Address.Contains(keyword)
                        select n;
            }

            if (filterModel.Password != null && !String.IsNullOrWhiteSpace(keyword))
            {
                query = from n in query
                        where n.Password.Contains(keyword)
                        select n;
            }

            if (filterModel.Status != null)
            {
                query = from n in query
                        where n.IsActive == filterModel.Status
                        select n;
            }

            if (filterModel.CategoryID != null)
            {
                query = from n in query
                        where n.UserCategoryID == filterModel.CategoryID
                        select n;
            }

            query = from n in query
                    orderby n.DateCreated descending
                    select n;

            var entities = query.OrderBy(n => n.LoginName).ToList();

            var converter = new UserEntityModelConverter(HbSession);

            var model = new UsersModel
            {
                List = entities.Select(n => converter.Convert(n)).ToList()
            };

            usersControl.Model = model;
            usersControl.DataBind();
        }

        protected UM_Group GetRootGroup(UM_Group upperLevelGroup, UM_Group group)
        {
            while (group.Parent != null && group.ID != upperLevelGroup.ID)
            {
                group = group.Parent;
            }

            return group;
        }

        protected void SetUserGlobalAttribute(Guid userID, String attributeName, String value)
        {
            var globalAttr = (from n in HbSession.Query<UM_User>()
                              join m in HbSession.Query<UM_AttributeValue>() on n.ID equals m.ParentID
                              where n.ID == userID
                              where m.DateDeleted == null
                              let node = m.AttributeField
                              let schema = node.AttributeSchema
                              where node != null &&
                                    node.Name == attributeName &&
                                    schema.ProjectID == null
                              select m).FirstOrDefault();

            if (globalAttr == null)
            {
                globalAttr = new UM_AttributeValue
                {
                    ID = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    ParentID = userID,
                };



                var globalAttrNode = (from n in HbSession.Query<UM_AttributeField>()
                                      where n.Name == attributeName &&
                                            n.DateDeleted == null
                                      let schema = n.AttributeSchema
                                      where schema.ProjectID == null
                                      select n).SingleOrDefault();

                if (globalAttrNode == null)
                {
                    var globalSchema = (from n in HbSession.Query<UM_AttributeSchema>()
                                        where n.DateDeleted == null &&
                                              n.ProjectID == null
                                        select n).Single();


                    globalAttrNode = new UM_AttributeField
                    {
                        ID = Guid.NewGuid(),
                        Name = attributeName,
                        DateCreated = DateTime.Now,
                        AttributeSchema = globalSchema,
                    };
                }

                globalAttr.AttributeField = globalAttrNode;
            }

            globalAttr.Value = value;

            HbSession.SubmitInsert(globalAttr);
        }

        protected String GetUserGlobalAttribute(Guid userID, String attributeName)
        {
            var globalAttr = (from n in HbSession.Query<UM_User>()
                              join m in HbSession.Query<UM_AttributeValue>() on n.ID equals m.ParentID
                              where n.ID == userID
                              where m.DateDeleted == null
                              let node = m.AttributeField
                              let schema = node.AttributeSchema
                              where node != null &&
                                    node.Name == attributeName &&
                                    schema.ProjectID == null
                              select m).FirstOrDefault();

            if (globalAttr != null)
                return globalAttr.Value;

            return null;
        }

        private void ApplyPermissions()
        {
            if (!UmUtil.Instance.HasAccess("UsersList"))
                UmUtil.Instance.GoToLogin();
        }

        #endregion


    }
}

