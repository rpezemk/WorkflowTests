using System;
using System.IO;

namespace DawWCFService
{
    internal static class Log
    {
        internal static void WriteLogEntry(string logText)
        {
            if (!File.Exists(@"C:\Users\Michael\Desktop\log2.txt")) 
                File.CreateText(@"C:\Users\Michael\Desktop\log2.txt");

            using (StreamWriter fs = File.AppendText(@"C:\Users\Michael\Desktop\log2.txt"))
            {
                fs.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}: {logText}");
            }
        }
    }
}
