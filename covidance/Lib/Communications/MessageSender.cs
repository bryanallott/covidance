using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace covidance.Lib.Communications
{
    public class MessageSender : IMyEmailSender, ISmsSender
    {
        public MessageSender(IOptions<CovidanceSettings> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }
        public MessageSender(CovidanceSettings settings)
        {
            Options = settings;
        }
        public CovidanceSettings Options { get; }
        public async Task<dynamic> SendEmailAsync(string gmailkey, string from, string to, string subject, string messageHtml, string messageText)
        {
            if (string.IsNullOrEmpty(gmailkey))
            {
                return await SendEmailAsync(Options.SendgridApiKey, from, to, subject, messageHtml, messageText, new List<string> { "covidance" });
            }
            else
            {
                return SendGmail(gmailkey, from, to, subject, messageHtml);
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        private static dynamic SendGmail(string gmailAppKey, string gmailAddress, string to, string subject, string message)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            client.Timeout = 30000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(gmailAddress, gmailAppKey);
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(gmailAddress);

            if (!string.IsNullOrEmpty(to))
            {
                mm.To.Add(new MailAddress(to));
            }

            mm.Subject = subject;
            mm.Body = message;

            mm.IsBodyHtml = true;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            client.Send(mm);
            return new { StatusCode = 200 };
        }
        private static async Task<dynamic> SendEmailAsync(string apikey, string from, string to,
            string subject, string message, string text, List<string> categories)
        {
            var myMessage = new SendGridMessage();

            if (null != categories && 0 < categories.Count)
            {
                myMessage.Categories = categories;
            }

            myMessage.From = new EmailAddress(from);

            string[] tos = to.Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (null != tos && 0 < tos.Length)
            {
                for (int ii = 0; ii < tos.Length; ii++)
                {
                    var tmp = tos[ii].Trim();
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        myMessage.AddTo(new EmailAddress(tmp));
                    }
                }
            }
            myMessage.Subject = subject;

            myMessage.AddContent(MimeType.Text, text);
            myMessage.AddContent(MimeType.Html, message);

            dynamic sg = new SendGridClient(apikey);
            return await sg.SendEmailAsync(myMessage);
        }
    }
}
