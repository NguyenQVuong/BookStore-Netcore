using BookShop.BackendApi.Services.Email.EmailTemplate;

namespace BookShop.BackendApi.Services.Email
{
    public class EmailTemplateFactory
    {
        public static HappyNewDay SendHappyNewDay(string UserName, string EmailSubject, string Receiver)
        {
            return new HappyNewDay(UserName, EmailSubject, Receiver);
        }
        public static WelcomeNewUserEmail SendWelcomeNewUserEmail(string UserName, string EmailSubject, string Receiver)
        {
            return new WelcomeNewUserEmail(UserName, EmailSubject, Receiver);
        }
        public static DiscountMail SendDiscountMail(string UserName, string EmailSubject, string Receiver)
        { 
            return new DiscountMail(UserName, EmailSubject, Receiver);
        }
        public static UnsubcribeEmail SendUnsubcribeEmail( string EmailSubject, string Receiver) 
        {
            return new UnsubcribeEmail(EmailSubject, Receiver);
        }
        public static EmailConfimed SendConfirmedEmail(string Link, string EmailSubject, string Receiver)
        {
            return new EmailConfimed( EmailSubject, Receiver);
        }
    }
}
