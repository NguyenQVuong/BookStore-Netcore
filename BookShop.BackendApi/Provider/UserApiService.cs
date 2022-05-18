    using BookShop.BackendApi.Configuration;
using BookShop.BackendApi.Models;
using BookShop.BackendApi.Services.Email;
using BookShop.BackendApi.Utils;
using BookShop.Data.EF;
using BookShop.Data.Entities;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Provider
{
    public class UserApiService : BaseProvider, IUserApiService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private static int PAGE_SIZE = 5;

        public UserApiService(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<UserViewModel> GetUserById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return null;
            }
            var userVm = new UserViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                DOB = user.DOB,
                Id = user.Id,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
            return userVm;
        }
        public async Task<int> DeleteUser(Guid id)
        {
            var deleteUser = await _userManager.FindByIdAsync(id.ToString());
            if (deleteUser == null)
            {
                return 0;
            }
            db.Users.Remove(deleteUser);
            return await db.SaveChangesAsync();
        }
        public async Task<int> UpdateUser(Guid Id, UpdateUserRequest request)
        {
            if(await _userManager.Users.AnyAsync(x=>x.Email == request.Email && x.Id !=Id))
            {
                return 0;
            }
            var user = await _userManager.FindByIdAsync(Id.ToString());
            user.DOB = request.DOB;
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.UserName = request.UserName;
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return 1;
            }
            return 0;
        }
        public async Task<User> UploadAvatar(Guid Id, IFormFile file)
        {
            var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssffff") + new Random().Next(1000, 99999).ToString() + ReflectionHelper.RandomString(15) + Path.GetExtension(file.FileName);
            User userUploadAvatar = await db.Users.Where(c => c.Id == Id).FirstOrDefaultAsync();
            // check to delete old avatar
            if (File.Exists(userUploadAvatar.Avatar))
            {
                File.Delete(userUploadAvatar.Avatar);
            }
            userUploadAvatar.Avatar = Path.Combine(fileName);
            await db.SaveChangesAsync();
            await ProcessFileUtils.SaveAs(userUploadAvatar.Avatar, file);
            return userUploadAvatar;
        } 

        public async Task<string> Login(LoginRequest request)
        {
            var user = await GetUserByEmail(request);
            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public async Task<string> Register(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return "UserName already exists";
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return "Email already exists";
            }
            user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                Email = request.Email,
                DOB = request.DOB,
                PhoneNumber = request.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            //await db.Users.AddAsync(user);
/*            var email = EmailTemplateFactory.WelcomeNewUserEmail(result.UserName, "Welcome Account", result.Email);
            await email.SendAsync();*/

            if (result.Succeeded)
            {
                return "Register Successfull";
            }
            return "register failed";
        }

        public string ExportExcel()
        {
            string rootPath = Path.Combine(ConfigEnviroment.STORAGE_ROOT_PATH + ConfigEnviroment.STORAGE_RESOURCES + ConfigEnviroment.STORAGE_EXCEL);
            var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssffff") + new Random().Next(1000, 99999).ToString() + ReflectionHelper.RandomString(10) + ".xlsx";
            FileInfo file = new FileInfo(Path.Combine(rootPath, fileName));
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(file))
            {
                List<User> userList = db.Users.ToList();
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Users");
                int totalRows = userList.Count();

                worksheet.Cells[1, 1].Value = " UserId";
                worksheet.Cells[1, 2].Value = " UserName";
                worksheet.Cells[1, 3].Value = " FirstName";
                worksheet.Cells[1, 4].Value = " LastName";
                worksheet.Cells[1, 5].Value = "Date of Birth";
                worksheet.Cells[1, 6].Value = " Email";
                worksheet.Cells[1, 7].Value = " EmailConfirmed";
                worksheet.Cells[1, 8].Value = " PhoneNumber";
                worksheet.Cells[1, 9].Value = " Avatar";
                worksheet.Cells[1, 10].Value = " Unsubscribe";

                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = userList[i].Id;
                    worksheet.Cells[row, 2].Value = userList[i].UserName;
                    worksheet.Cells[row, 3].Value = userList[i].FirstName;
                    worksheet.Cells[row, 4].Value = userList[i].LastName;
                    worksheet.Cells[row, 5].Value = userList[i].DOB.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 6].Value = userList[i].Email;
                    worksheet.Cells[row, 7].Value = userList[i].EmailConfirmed ? "Actived" : "No Actived";
                    worksheet.Cells[row, 8].Value = userList[i].PhoneNumber;
                    worksheet.Cells[row, 9].Value = userList[i].Avatar;
                    worksheet.Cells[row, 10].Value = userList[i].Unsubscribe;
                    /*                    if (userList[i].avatar_path == null)
                                            worksheet.Cells[row, 10].Value = userList[i].avatar_path;
                                        else
                                        {
                                            var avatarPath = Path.Combine(PathConfigs.rootPath, PathConfigs.assetPath, PathConfigs.imagesPath
                                                , PathConfigs.assetUserPath, PathConfigs.userAvatarPath, userList[i].avatar_path);
                                            Image avtImag = Image.FromFile(avatarPath);

                                            worksheet.Row(row).Height = 80.00D;
                                            worksheet.Column(10).Width = 40.00D;
                                            var picture = worksheet.Drawings.AddPicture(row.ToString(), avtImag);
                                            picture.SetPosition(row - 1, 9, 9, 10);
                                            picture.SetSize((int)(80 * picture.Size.Width / picture.Size.Height), 80);
                                        }*/
                    i++;
                }
                 package.Save();
            }
            return "export success"; 
        }
        public async Task ImportExcel(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<User> userList = new List<User>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    int totalRows = workSheet.Dimension.Rows;
                    for (int i = 2; i <= totalRows; i++)
                    {
                        userList.Add(new User
                        {
                            FirstName = workSheet.Cells[i, 2].Value.ToString(),
                            LastName = workSheet.Cells[i, 3].Value.ToString(),
                            DOB = DateTime.Parse(workSheet.Cells[i, 4].Value.ToString()),
                            UserName = workSheet.Cells[i, 5].Value.ToString(),
                            Email = workSheet.Cells[i, 6].Value.ToString(),
                            PhoneNumber = workSheet.Cells[i, 8].Value.ToString(),
                            Avatar = workSheet.Cells[i, 9].Value.ToString()
                        });
                    }
                    db.Users.AddRange(userList);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<User> GetByPath(string path)
        {
            return await db.Users.Where(c => c.Avatar == path).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(LoginRequest request)
        {
            var user = await db.Users.SingleOrDefaultAsync(x => x.Email == request.Email && x.PasswordHash == request.Password);
            return user;
        }

        public Task UnsubscribeEmail(string Email)
        {
            var existedUser = db.Users.Where(x => x.Email == Email).FirstOrDefault();
            if (existedUser == null) return null;
            existedUser.Unsubscribe = !existedUser.Unsubscribe;
            return db.SaveChangesAsync();
        }

        public async Task<User> EmailConfirmed(Guid Id)
        {
            User existedUser = await db.Users.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (existedUser == null) return null;
            existedUser.EmailConfirmed = true;
            await db.SaveChangesAsync();
            return existedUser;
        }

        //search and paging
        public List<UserViewModel> GetUsers(string search, int page = 1)
        {
            var allUser = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                allUser = allUser.Where(x => x.UserName.Contains(search));
            }
            allUser = allUser.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

            var result = allUser.Select(x => new UserViewModel()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DOB = x.DOB,
            });
            return result.ToList();
        }
    }
}
