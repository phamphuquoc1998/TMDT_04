using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace TMDT.Mail
{
    public class MailHelper
    {
        public void SendMail(string toEmailAddress, string subject, string content, Order order)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();

            bool enabledSsl = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());
            string FilePath = "~\\Mail\\Email.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[CustomerName]", order.NameRec.ToString().Trim());
            MailText = MailText.Replace("[CusAddress]", order.AddressOrder.ToString().Trim());
            MailText = MailText.Replace("[Phone]", order.PhoneOrder.ToString().Trim());
            MailText = MailText.Replace("[orderID]", order.OrderID.ToString().Trim());
            MailText = MailText.Replace("[OrderDetails]", strMessage);
            string body = MailText;
            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.UseDefaultCredentials = true;
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enabledSsl;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.Send(message);
        }


    }
}
