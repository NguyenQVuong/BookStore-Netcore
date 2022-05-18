using BookShop.BackendApi.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BookShop.BackendApi.Utils
{
    public class ProcessFileUtils
    {
        public static async Task SaveAs(string filePath, IFormFile image)
        {
            try
            {
                var physicalPath = Path.Combine( ConfigEnviroment.STORAGE_ROOT_PATH, ConfigEnviroment.STORAGE_IMG_USER_PATH, filePath);
                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            catch (IOException ioExp)
            {
                using (StreamWriter sw = File.AppendText(Path.Combine(ConfigEnviroment.STORAGE_ROOT_PATH, ConfigEnviroment.STORAGE_LOGPATH, "ProcessFileErrorLogs.txt")))
                {
                    sw.WriteLine($"{DateTime.Now} Remove file unseccessfully: Path = {filePath}");
                    sw.WriteLine($"Detail: {ioExp}");
                }
            }
        }

        public static byte[] ReadAllBytes( string url)
        {
            return File.ReadAllBytes(url);
        }
    }
}
