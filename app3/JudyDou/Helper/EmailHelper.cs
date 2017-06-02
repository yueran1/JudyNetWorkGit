using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace JudyDou.Helper
{
    public class Email
    {
        public string EmailAddress { get; set; }
        public List<ContentReplacement> EmailContent { get; set; }

        public Email()
        {
            EmailContent = new List<ContentReplacement>();
        }

        public void Replace(string tag, string content)
        {
            EmailContent.Add(new ContentReplacement(tag, content));
        }
    }

    public class ContentReplacement
    {
        public string Tag { get; set; }
        public string EmailContent { get; set; }

        public ContentReplacement(string tag, string content)
        {
            Tag = tag;
            EmailContent = content;
        }
    }

    public static class EmailHelper
    {
        /// <summary>
        /// send email to the email address passed in
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="title">email subject</param>
        /// <param name="template">file name of the email template. NOTE: do not include file path</param>
        public static void SendEmail(Email email, string title, string template)
        {
            SendEmail(new List<Email>() { email }, title, template);
        }

        /// <summary>
        /// send email to the list of emails passed in
        /// </summary>
        /// <param name="emails">list of emails</param>
        /// <param name="title">email subject</param>
        /// <param name="template">file name of the email template. NOTE: do not include file path</param>
        public static void SendEmail(List<Email> emails, string title, string template)
        {
            string password = "judydou.ca";
            var fromAddress = new MailAddress("judydou.com@gmail.com", "JudyDou");

            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };

            string bodyTemplate = new StreamReader(HostingEnvironment.MapPath("~/Content/emails/") + template).ReadToEnd();

            foreach (var item in emails)
            {
                try
                {
                    MailMessage email = new MailMessage();

                    email.From = fromAddress;
                    email.To.Add(item.EmailAddress);

                    string body = bodyTemplate;

                    foreach (var content in item.EmailContent)
                        body = body.Replace(content.Tag, content.EmailContent);

                    email.IsBodyHtml = true;
                    email.Subject = title;
                    email.Body = body;

                    smtpClient.Send(email);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// check if it is a valid email address
        /// </summary>
        /// <param name="email">email address</param>
        /// <returns>if is valid</returns>
        public static bool ValidEmail(string email)
        {
            string pattern = @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?";
            return email != null && System.Text.RegularExpressions.Regex.Match(email.Trim(), pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Success;
        }
    }
}
