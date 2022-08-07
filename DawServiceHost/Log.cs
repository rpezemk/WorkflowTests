using System;
using System.IO;

namespace DawServiceHost
{
    internal static class Log
    {
        internal static void WriteLogEntry(string logText)
        {
            if (!File.Exists(@"C:\Users\Michael\Desktop\log.txt")) 
                File.CreateText(@"C:\Users\Michael\Desktop\log.txt");

            using (StreamWriter fs = File.AppendText(@"C:\Users\Michael\Desktop\log.txt"))
            {
                fs.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {logText}");
            }
        }
    }
}
