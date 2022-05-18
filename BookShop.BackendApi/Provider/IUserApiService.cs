using BookShop.BackendApi.Models;
using BookShop.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Provider
{
    public interface IUserApiService
    {
        Task<UserViewModel> GetUserById(Guid Id);
        Task<string> Register(RegisterRequest request);
        Task<int> DeleteUser(Guid id);
        Task<int> UpdateUser(Guid Id ,UpdateUserRequest request);
        Task<User> UploadAvatar(Guid Id, IFormFile file);
        Task<User> GetByPath(string path);
        string ExportExcel();
        Task ImportExcel(IFormFile file);
        Task UnsubscribeEmail(string Email);
        Task<User> EmailConfirmed(Guid Id);
        Task<User> GetUserByEmail(LoginRequest request);
        Task<string> Login(LoginRequest request);
        List<UserViewModel> GetUsers(string search, int page = 1);
    }
}
