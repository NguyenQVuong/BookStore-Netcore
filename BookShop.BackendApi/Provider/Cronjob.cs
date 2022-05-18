using BookShop.BackendApi.Models;
using BookShop.BackendApi.Services.Email;
using BookShop.Data.Entities;
using Hangfire;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Provider
{
    public class Cronjob:BaseProvider, ICronjob
    {
        // fire and forget
        public void SendEmailAccount()
        {
            var user = db.Users.Where(u => u.EmailConfirmed == false);
            if (user != null)
            {
                foreach(var userNoConfirmedEmail in user )
                {
                    var email = EmailTemplateFactory.SendConfirmedEmail("EmailConfirmed ", userNoConfirmedEmail.Email);
                    email.Send();
                }
            }
        }
        //Delay
        public void SendDiscount()
        {
            var user = db.Users.Where(u => u.EmailConfirmed == true && u.Unsubscribe == false);
            if (user != null)
            {
                foreach (var sendUserEmail in user)
                {
                    Console.WriteLine($"Sending discount mail to {sendUserEmail.Email}.");
                    var email = EmailTemplateFactory.SendDiscountMail(sendUserEmail.UserName, "Discount ", sendUserEmail.Email);
                    email.Send();
                    Console.WriteLine($"Sending discount mail successfull to {sendUserEmail.Email} at {DateTime.Now.ToLongTimeString()}");
                }
            }
        }
        //RecurringJob
        public void SendEmailEveryday()
        {
            var user = db.Users.Where(u => u.EmailConfirmed == true && u.Unsubscribe == false);
            if(user != null)
            {
                foreach(var sendUserEmail in user)
                {
                    Console.WriteLine($"Sending mail to {sendUserEmail.Email}.");
                    var email = EmailTemplateFactory.SendHappyNewDay(sendUserEmail.UserName, "Good Morning", sendUserEmail.Email);
                    email.Send();
                    Console.WriteLine($"Sending mail successfull to {sendUserEmail.Email} at: {DateTime.Now.ToLongTimeString()}");
                }
            }
        }
        //Continuations
        public void Continuations()
        {
            var user = db.Users.Where(u => u.EmailConfirmed == true && u.Unsubscribe == false );
            foreach (var UserEmail in user)
            {
                var unsubcribeEmail = EmailTemplateFactory.SendUnsubcribeEmail("Notification", UserEmail.Email);
                unsubcribeEmail.Send();
                Console.WriteLine($"Sending unsubcribe mail successfull to {UserEmail.Email} at: {DateTime.Now.ToLongTimeString()}");
            }
                 
        }
    }
}
