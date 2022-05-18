using System.IO;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Services.Email.EmailTemplate
{
    public class UnsubcribeEmail : EmailContent
    {
        public UnsubcribeEmail(string EmailSubject, string Receiver)
        {
            EmailTemplatePath += "\\UnsubcribeEmail.html";
            this.EmailSubject = EmailSubject;
            this.Receiver = Receiver;
        }
        public override string GetContent()
        {
            var fullBody = "Unsubcribe";
            if (File.Exists(EmailTemplatePath))
            {
                fullBody = File.ReadAllText(EmailTemplatePath);
            }
            else
            {
                fullBody = "Cann't find template at path: " + EmailTemplatePath;
            }
            return fullBody;
        }

        public override bool Send()
        {
            SendEmail(Receiver, EmailSubject, GetContent());
            return true;
        }

        public override async Task SendAsync()
        {
            await Task.Run(() => SendEmailAsync(Receiver, EmailSubject, GetContent()));
        }
    }
}
