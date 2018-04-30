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

            Dictionary<DateTime, List<string>> folderName = folderNameandTime.GroupBy(a => a.Value.Date).ToDictionary(t => t.Key, t => t.Select(a => string.Format("{0}, {1}", Path.GetFileName(a.Key), a.Value)).ToList());
            
            using (StreamWriter file = new StreamWriter(@"C:\InstalledSoftware.txt"))
            {
                foreach (var entry in folderName.OrderBy(a=>a.Key.Date))
                {
                    file.WriteLine();
                    file.WriteLine("[Created Date - {0}]", entry.Key.ToString("MM/dd/yyyy"));

                    var folderData = entry.Value.Select(a => a.Split(',').ToList());
                    var listname = folderData.GroupBy(a => Convert.ToDateTime(a[1]).ToString("HH:mm")).ToDictionary(t => t, t => t.Select(a => string.Format("{0}, {1}", Path.GetFileName(a[0]), a[1])).ToList());

                    foreach (var item in listname.OrderBy(a => a.Key.Key))
                    {
                        file.WriteLine(" Time :: {0} ", item.Key.Key);
                        foreach (var items in item.Value)
                        {
                            file.WriteLine("     FolderName :: {0}", items);
                        }
                    }
                }
            }
        }
    }
}
