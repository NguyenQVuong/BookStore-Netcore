using BookShop.BackendApi.Configuration;
using BookShop.BackendApi.Models;
using BookShop.BackendApi.Provider;
using BookShop.BackendApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Hangfire;

namespace BookShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserApiService _userApiService;

        public UserController(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }
        [HttpGet("/get-by-id/{Id}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetByUserId(Guid Id)
        {
            try
            {
                var user = await _userApiService.GetUserById(Id);
                if (user == null)
                {
                    return new JsonResult(new { msg = "No User Found" });
                }
                return new JsonResult(new { StatusCode = "200", data = user, msg = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = "Bad request" + ex.Message });
            }
        }

        [HttpGet("/get-by-path/{path}")]
        [AllowAnonymous]
        public async Task<JsonResult> GetByPath(string path)
        {
            try
            {
                var user = await _userApiService.GetByPath(path);
                return new JsonResult(new { StatusCode = "200", data = user, msg = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = "Bad request" + ex.Message });
            }
        }

        [HttpGet("/download")]
        [AllowAnonymous]
        public async Task<IActionResult> Download([FromQuery] string path)
        {
            var user = await _userApiService.GetByPath(path);
            var fileBytes = ProcessFileUtils.ReadAllBytes(ConfigEnviroment.STORAGE_PUBLIC_AVATAR + "\\" +user.Avatar);
            string mineType = ContentTypeFactory.GetMimeType(user.Avatar.GetType().ToString());
            return File(fileBytes, mineType, user.Avatar);

        }  

        [HttpPut("/update-user/{Id}")]
        [AllowAnonymous]
        public async Task<JsonResult> UpdateUser(Guid Id, [FromBody]UpdateUserRequest request)
        {
            try
            {
                await _userApiService.UpdateUser(Id, request);
                return new JsonResult(new { StatusCode = "200", msg = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = ex.Message });
            }
        }

        [HttpDelete("/{Id}")]
        [AllowAnonymous]
        public async Task<JsonResult> DeleteUser(Guid Id)
        {
            try
            {
                var user = await _userApiService.GetUserById(Id);
                if (user == null)
                {
                    return new JsonResult(new { StatusCode = 500, msg = " User not found" });
                }
                await _userApiService.DeleteUser(Id);
                return new JsonResult(new { StatusCode = "200", msg = "Delete success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = ex.Message });
            }
        }

        [HttpPost("/{Id}/uploadAvatar")]
        [AllowAnonymous]
        public async Task<JsonResult> UploadAvatar(Guid Id, IFormFile file)
        {
            if (!ReflectionHelper.IsImage(file))
            {
                return new JsonResult(new { statusCode = 400, msg = "Image is not valid" });
            }
            var result = await _userApiService.UploadAvatar(Id, file);
            if (result == null)
            {
                return new JsonResult(new { statusCode = 400, msg = "Cann't upload for user" + Id });
            }
            return new JsonResult(new { statusCode = 200, success = true, msg = "upload success" });
        }

        [HttpPost("/import-user")]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (!ReflectionHelper.IsExcelFile(file))
            {
                return BadRequest("file is not valid");
            }
            var result =  _userApiService.ImportExcel(file);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("/export-user")]
        [AllowAnonymous]
        public async Task<IActionResult> ExportExcel()
        {
            var result =  _userApiService.ExportExcel();
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpPost("/register-account")]
        [AllowAnonymous]
        public async Task<JsonResult> RegisterUser([FromForm] RegisterRequest request)
        {
            try
            {
                var registerUser = await _userApiService.Register(request);
                return new JsonResult(new { StatusCode = 201, data = registerUser, msg = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = ex.Message });
            }
        }

        //[HttpGet("/download-templates-word")]
        //[AllowAnonymous]
        //public async Task<IActionResult> MailMerge()
        //{
        //    FileStream fileStreamPath = new FileStream("D:\\Projects\\BookShop\\BookShop.BackendApi\\Resources\\Word\\Don_Xin_Nghi_Phep.docx", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //    WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx);
        //    var datetime = DateTime.Now;
        //    var date = datetime.Date.ToString();
        //    var month = datetime.Month.ToString();
        //    var year = datetime.Year.ToString();
        //    string[] fieldNames = new string[] { "Company", "LastName", "FirstName", "DOB", "PhoneNumber", "DATE_CREATE", "MONTH_CREATE", "YEAR_CREATE" };
        //    string[] fieldValues = new string[] { "TMA SOLUTIONS", "Nguyễn Quốc", "Vương", "07-07-1999", "0397621320", $"{date}", $"{month}", $"{year}" };
        //    document.MailMerge.Execute(fieldNames, fieldValues);
        //    MemoryStream stream = new MemoryStream();
        //    document.Save(stream, FormatType.Docx);
        //    document.Close();
        //    stream.Position = 0;
        //    //Download Word document in the browser
        //    return File(stream, "application/msword", "Template.docx");
        //}

        [HttpGet("/{Id}/active-account")]
        public async Task<JsonResult> ActiveAccount(Guid Id)
        {
            try
            {
                var activeEmail = await _userApiService.EmailConfirmed(Id);
                return new JsonResult(new { StatusCode = 200, msg = "success" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 500, msg = ex.Message });
            }
        }

        [HttpPost("dang-nhap")]
        public async Task<JsonResult> LoginUser(LoginRequest request)
        {
            try
            {
                var user = _userApiService.GetUserByEmail(request)
            }
        }

        [HttpGet("tim-kiem")]
        public async Task<IActionResult> Search([FromQuery] string search, int page = 1)
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    return NotFound(404);
                }
                var result = _userApiService.GetUsers(search, page);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}