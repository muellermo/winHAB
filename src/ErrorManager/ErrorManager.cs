using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace winHAB.ErrorManager
{
    /// <summary>
    /// This Class is static.
    /// It is used to print Errors to an Logfile.
    /// </summary>
    public static class ErrorManager
    {
        public static async void printErrorToLog(String stackTrace, String message)
        {
            StorageFolder storageFolder = KnownFolders.VideosLibrary;
            Random rnd = new Random();
            String fileName = DateTime.Now.Day.ToString()+DateTime.Now.Month+"."+DateTime.Now.Ticks+".log";// Convert.ToString(rnd.Next()) + ".txt";
            StorageFile storageFile = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            //MemoryStream memoryStream = new MemoryStream(UTF8Encoding.Convert(json));
            Stream fileStream = await storageFile.OpenStreamForWriteAsync();
            fileStream.Write(UTF8Encoding.UTF8.GetBytes("DATUM: " + DateTime.Now.ToString() + "\n" + stackTrace), 0, UTF8Encoding.UTF8.GetBytes(stackTrace).Length);
            fileStream.Write(UTF8Encoding.UTF8.GetBytes("DATUM: " + DateTime.Now.ToString() + "\n" + message), 0, UTF8Encoding.UTF8.GetBytes(message).Length);
            fileStream.Flush();
        }
    }
}
