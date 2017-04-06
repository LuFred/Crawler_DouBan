using System.IO;

namespace Crawler.Core.Common
{
    public static class FSHelper
    {
        //read file
        public static string Read(string file)
        {
          
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                long fileLength = fs.Length;
                byte[] data = new byte[fileLength];
                 fs.Read(data, 0, data.Length);
                return System.Text.Encoding.UTF8.GetString(data);
            }
        }
    }
}
