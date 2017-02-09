using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web;
using CITI.EVO.UserManagement.DAL.Domain;
using NVelocityTemplateEngine;

namespace CITI.EVO.UserManagement.Web.Utils
{
    public static class EmailUtil
    {
        public static void SendActivationEmail(UM_User user)
        {
            if (user == null || String.IsNullOrWhiteSpace(user.Email))
                return;

            var emailText = GetTemplate("UserActivationTemplate", user);

            SendEmail("Account Activation", user.Email, emailText);
        }

        public static void SendActivatedEmail(UM_User user)
        {
            if (user == null || String.IsNullOrWhiteSpace(user.Email))
                return;

            var emailText = GetTemplate("UserActivatedTemplate", user);

            SendEmail("Account Activated", user.Email, emailText);
        }

        public static void SendRecoveryEmail(UM_User user)
        {
            if (user == null || String.IsNullOrWhiteSpace(user.Email))
                return;

            var emailText = GetTemplate("UserRecoveryTemplate", user);
            SendEmail("Account Recovery", user.Email, emailText);
        }

        private static void SendEmail(String subject, String email, String text)
        {
            using (var smtpClient = new SmtpClient())
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(email);
                    message.Subject = subject;
                    message.Body = text;
                    message.IsBodyHtml = true;

                    smtpClient.Send(message);
                }
            }
        }

        private static String GetTemplate(String key, UM_User user)
        {
            var virtPath = ConfigurationManager.AppSettings[key];
            var physicalPath = HttpContext.Current.Server.MapPath(virtPath);
            var templateText = File.ReadAllText(physicalPath);

            var context = new Dictionary<String, Object>
            {
                {"user", user}
            };

            var engine = NVelocityEngineFactory.CreateNVelocityMemoryEngine();

            var text = engine.Process(context, templateText);
            return text;
        }
    }
}