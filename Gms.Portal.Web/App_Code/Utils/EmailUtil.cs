using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Gms.Portal.Web.Utils
{
    public static class EmailUtil
    {
        public static void SendEmail(String recipient, String subject, String text)
        {
            using (var smtpClient = new SmtpClient())
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(recipient);
                    message.Subject = subject;
                    message.Body = text;
                    message.IsBodyHtml = true;

                    smtpClient.Send(message);
                }
            }
        }

        public static void SendEmail(List<String> recipients, String subject, String text)
        {
            using (var smtpClient = new SmtpClient())
            {
                using (var message = new MailMessage())
                {
                    foreach (var recipient in recipients)
                        message.To.Add(recipient);

                    message.Subject = subject;
                    message.Body = text;
                    message.IsBodyHtml = true;

                    smtpClient.Send(message);
                }
            }
        }
    }
}