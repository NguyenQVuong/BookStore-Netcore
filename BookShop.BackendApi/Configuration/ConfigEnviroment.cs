using System;
using System.IO;

namespace BookShop.BackendApi.Configuration
{
    public class ConfigEnviroment
    {
        public static string STORAGE_ROOT_PATH = Directory.GetCurrentDirectory();
        public static string STORAGE_IMG_USER_PATH = "wwwroot\\image\\users";
        public static string STORAGE_PUBLIC_AVATAR = Path.Combine(STORAGE_ROOT_PATH, STORAGE_IMG_USER_PATH);
        public static string STORAGE_RESOURCES = "\\Resources";
        public static string STORAGE_EXCEL = "\\Excel\\";
        public static string STORAGE_WORD = "\\Word\\";
        public static string STORAGE_LOGPATH = "wwwroot\\Logs\\";
        public static string SMTP_SERVER = "smtp.gmail.com";
        public static int SMTP_PORT = 587;
        public static string USERNAME = "vn496782@gmail.com";
        public static string PASSWORD = "nguyenvuong";
    }
}
