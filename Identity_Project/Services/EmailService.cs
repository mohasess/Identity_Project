using System.Net;
using System.Net.Mail;
using System.Text;

namespace Identity_Project.Services
{
    public class EmailService
    {
        public Task SendEmail(string userEmail,string body, string subject)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("Alireezaad@gmail.com", "vfoi exlo pvlr vcde");
            MailMessage mail= new MailMessage("Alireezaad@gmail.com",userEmail,subject,body);
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            client.Send(mail);
            return Task.CompletedTask;
        }
    }
}
