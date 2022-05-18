using BookShop.BackendApi.Models;
using System;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Provider
{
    public interface ICronjob
    {
        //fire and forget
        public void SendEmailAccount();
        //delay
        public void SendDiscount();
        //recurring
        public void SendEmailEveryday();
        //Continuations
        public void Continuations();
    }
}
