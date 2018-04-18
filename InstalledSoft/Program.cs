using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InstalledSoft
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, DateTime> folderNameandTime = new Dictionary<string, DateTime>();

            foreach (var files in Directory.GetDirectories(@"C:\Program Files (x86)"))
            {
                FileInfo info = new FileInfo(files);
                folderNameandTime.Add(info.FullName, info.CreationTimeUtc);
            }

            foreach (var files in Directory.GetDirectories(@"C:\Program Files"))
            {
                FileInfo info = new FileInfo(files);
                folderNameandTime.Add(info.FullName, info.CreationTimeUtc);
            }

            Dictionary<DateTime, List<string>> folderName = folderNameandTime.GroupBy(a => a.Value.Date.Date).ToDictionary(t => t.Key, t => t.Select(a => string.Format("{0}, {1}", a.Key, a.Value)).ToList());

            using (StreamWriter file = new StreamWriter(@"C:\InstalledSoftware.txt"))
            {
                foreach (var entry in folderName.OrderBy(a=>a.Key.Date.Date))
                {
                    file.WriteLine("[Created Date - {0}]", entry.Key);

                    foreach (var item in entry.Value)
                    {
                        file.WriteLine("Name - {0}", item);
                    }
                }
            }
        }
    }
}
