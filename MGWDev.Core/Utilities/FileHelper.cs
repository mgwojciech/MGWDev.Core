using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGWDev.Core.Utilities
{
    public class FileHelper
    {
        public static string ReadFile(string path, bool autoCreate = true)
        {
            try
            {
                using (Stream fileStream = File.OpenRead(path))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                if (!autoCreate)
                    throw;
                else
                {
                    string folderPath = FileHelper.GetFolder(path);
                    Directory.CreateDirectory(folderPath);
                    FileHelper.OverwriteFile(path, "");
                    return "";
                }
            }
            catch (FileNotFoundException)
            {
                if (!autoCreate)
                    throw;
                else
                {
                    FileHelper.OverwriteFile(path, "");
                    return "";
                }
            }
        }

        public static string GetFolder(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static void OverwriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}
