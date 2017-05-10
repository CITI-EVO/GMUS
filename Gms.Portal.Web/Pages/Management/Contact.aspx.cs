using System;
using System.Collections.Generic;
using System.Linq;
using CITI.EVO.Proxies;
using CITI.EVO.UserManagement.Svc.Contracts;
using Gms.Portal.DAL.Domain;
using Gms.Portal.Web.Bases;
using Gms.Portal.Web.Caches;
using Gms.Portal.Web.Converters.EntityToModel;
using Gms.Portal.Web.Converters.ModelToEntity;
using Gms.Portal.Web.Entities.DataContainer;
using Gms.Portal.Web.Helpers;
using Gms.Portal.Web.Models;
using Gms.Portal.Web.Utils;
using NHibernate.Linq;
using NVelocityTemplateEngine;


namespace Gms.Portal.Web.Pages.Management
{
    public partial class Contact : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_OnClick(object sender, EventArgs e)
        {
            var model = mailContractControl.Model;
            var formID = Guid.Empty;

            var recipientIds = model.RecipientsID;

            var recipientUsers = (from n in recipientIds
                                  let u = UmUsersCache.GetUser(n)
                                  select u).ToList();

            if (model.ContactType == "All" || model.ContactType == "Email")
            {
                var users = (from n in recipientUsers
                             where RegexUtil.EmailCheckRx.IsMatch(n.Email)
                             select n).ToList();

                foreach (var user in users)
                    SendMessage(user, model);
            }

            if (model.ContactType == "All" || model.ContactType == "Sms")
            {
                foreach (var user in recipientUsers)
                {
                    if (String.IsNullOrWhiteSpace(user.Phone))
                        continue;

                    SendMessage(user, model);
                }
            }

            //AddNotificationHistory(recipients, model);
        }

        protected void SendMessage(UserContract user, MailContactModel model)
        {
            var formID = model.FormID;
            var engine = NVelocityEngineFactory.CreateNVelocityMemoryEngine();

            var formData = LoadUserFormData(formID, user.ID);

            var context = new Dictionary<String, Object>
            {
                ["UserData"] = user,
                ["FormData"] = formData
            };

            var mailText = engine.Process(context, model.Body);

            if (model.ContactType == "All" || model.ContactType == "Sms")
                CommonProxy.SendSmsUsingCuratio(user.Phone, model.Body);

            if (model.ContactType == "All" || model.ContactType == "Email")
                EmailUtil.SendEmail(user.Email, model.Subject, mailText);
        }

        private IDictionary<String, Object> LoadUserFormData(Guid? formID, Guid? userID)
        {
            if (formID == null || userID == null)
                return null;

            var dbForm = HbSession.Query<GM_Form>().FirstOrDefault(n => n.ID == formID);
            if (dbForm == null)
                return null;

            var converter = new FormEntityModelConverter(HbSession);
            var model = converter.Convert(dbForm);

            var entity = model.Entity;
            if (entity == null)
                return null;

            var filter = new Dictionary<String, Object>
            {
                {FormDataConstants.UserIDField, userID},
                {FormDataConstants.DateDeletedField, (DateTime?)null},
            };

            var document = MongoDbUtil.FindDocuments(dbForm.ID, filter).FirstOrDefault();

            var formData = BsonDocumentConverter.ConvertToFormDataUnit(document);
            var dictionary = BsonDocumentConverter.TransferToNamedContainer(formData, entity);

            return dictionary;
        }

        protected void AddNotificationHistory(List<GM_Recipient> recipients, MailContactModel mailModel)
        {
            using (var transaction = HbSession.BeginTransaction())
            {
                foreach (var recipient in recipients)
                {
                    var user = UmUsersCache.GetUser(recipient.UserID);

                    var model = new NotificationHistoryModel
                    {
                        RecipientID = recipient.ID,
                        UserID = recipient.UserID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Phone = user.Phone,
                        ContactType = mailModel.ContactType,
                        Subject = mailModel.Subject,
                        Body = mailModel.Body
                    };

                    var converter = new NotificationHistoryModelEntityConverter(HbSession);
                    var entity = converter.Convert(model);
                    HbSession.Save(entity);
                }
                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
        }
    }
}