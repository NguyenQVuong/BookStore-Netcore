using BookShop.BackendApi.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Services.Email.EmailTemplate
{
    public abstract class EmailContent
    {
        public string EmailTemplatePath = Environment.CurrentDirectory + "\\Resources\\EmailTemplates";
        public string EmailSubject { get; set; }
        public string Receiver { get; set; }
        public abstract string GetContent();
        public abstract bool Send();
        public abstract Task SendAsync();

        //Config enviroment email 
        private readonly string smtpserver = ConfigEnviroment.SMTP_SERVER;
        private readonly int smtpport = ConfigEnviroment.SMTP_PORT;
        private readonly string username = ConfigEnviroment.USERNAME;
        private readonly string password = ConfigEnviroment.PASSWORD;

        protected bool SendEmail(string Reciever, string EmailSubject, string Body)
        {
            SmtpClient smtpClient = new SmtpClient(smtpserver, smtpport);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(username, password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(username);
            mailMessage.To.Add(Reciever);
            mailMessage.Subject = EmailSubject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            smtpClient.Send(mailMessage);
            return true;
        }
        protected async Task<bool> SendEmailAsync(string Receiver, string EmailSubject, string Body)
        {
            SmtpClient smtpClient = new SmtpClient(smtpserver, smtpport);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(username, password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(username);
            mailMessage.To.Add(Receiver);   
            mailMessage.Subject = EmailSubject;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
    }
}
