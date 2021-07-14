using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyShop.Helper
{
    public class FileHelper
    {
        public static bool UploadPhoto(MemoryStream memoryStream, string folderName, string fileName)
        {
            try
            {
                memoryStream.Position = 0;
                var path = Path.Combine(folderName, fileName);
                File.WriteAllBytes(path, memoryStream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
