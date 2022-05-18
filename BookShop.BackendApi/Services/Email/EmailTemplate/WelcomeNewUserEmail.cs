using System.IO;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Services.Email.EmailTemplate
{
    public class WelcomeNewUserEmail : EmailContent
    {
        protected string _UserName = "";

        public WelcomeNewUserEmail(string UserName, string EmailSubject, string Receiver)
        {
            EmailTemplatePath += "\\WelcomeNewUseremail.html";
            _UserName = UserName;
            this.EmailSubject = EmailSubject;
            this.Receiver = Receiver;
        }
        public override string GetContent()
        {
            var fullBody = "xin chuc mung {{UserName}} da tao thanh cong tai khoan cho dich vu nay";
            if (File.Exists(EmailTemplatePath))
            {
                fullBody = File.ReadAllText(EmailTemplatePath);
            }
            else
            {
                fullBody = "Cann't find template at path: " + EmailTemplatePath;
            }
            fullBody = fullBody.Replace("{{UserName}}", _UserName);
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
