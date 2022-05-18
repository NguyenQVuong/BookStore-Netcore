using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace BookShop.BackendApi.Utils
{
    public class ReflectionHelper
    {
        public static int RandomString(int number)
        {
            return (new Random()).Next(number);
        }
        public static bool IsImage(IFormFile image)
        {
            var imgContent = image.ContentType.ToLower();
            if (imgContent != "image/jpg" && imgContent != "image/jpeg" &&
                imgContent != "image/gif" && imgContent != "image/x-png" &&
                imgContent != "image/png")
            {
                return false;
            }
            var fileExtension = Path.GetExtension(image.FileName).ToLower();
            if (fileExtension != ".jpg" &&
                fileExtension != ".pjpeg" &&
                fileExtension != ".gif" &&
                fileExtension != ".png")
            {
                return false;
            }
            return true;
        }
        public static bool IsExcelFile(IFormFile excel)
        {
            var excelContentType = excel.ContentType.ToLower();
            if (excelContentType != "application/vnd.ms-excel"
                && excelContentType != "application/msexcel"
                && excelContentType != "application/x-msexcel"
                && excelContentType != "application/x-ms-excel"
                && excelContentType != "application/x-excel"
                && excelContentType != "application/x-dos_ms_excel"
                && excelContentType != "application/xls"
                && excelContentType != "application/x-xls"
                && excelContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            )
            {
                return false;
            }
            var pathExtension = Path.GetExtension(excel.FileName).ToLower();
            if (pathExtension != ".xlsx" && pathExtension != ".xlsm" &&
                pathExtension != "xlsb" && pathExtension != ".xltx")
            {
                return false;
            }
            return true;
        }

    }
}
