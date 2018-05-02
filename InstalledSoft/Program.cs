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
            List<string> excludedFolder = new List<string>() { "Common Files", "Internet Explorer", "Windows NT", "Intel", "Bonjour", "Common7", "AppInsights", "Goodix", "Dell" };

            Dictionary<string, DateTime> folderNameandTime = new Dictionary<string, DateTime>();

            foreach (var files in Directory.GetDirectories(@"C:\Program Files (x86)"))
            {
                FileInfo info = new FileInfo(files);
                if (!excludedFolder.Contains(info.Name))
                    folderNameandTime.Add(info.FullName, info.CreationTimeUtc);
            }

            foreach (var files in Directory.GetDirectories(@"C:\Program Files"))
            {
                FileInfo info = new FileInfo(files);
                if (!excludedFolder.Contains(info.Name))
                    folderNameandTime.Add(info.FullName, info.CreationTimeUtc);
            }

            Dictionary<DateTime, List<string>> folderName = folderNameandTime.GroupBy(a => a.Value.Date).ToDictionary(t => t.Key, t => t.Select(a => string.Format("{0}, {1}", Path.GetFileName(a.Key), a.Value)).ToList());

            Dictionary<string, List<Dictionary<string, string>>> latestFolder = new Dictionary<string, List<Dictionary<string, string>>>();

            using (StreamWriter file = new StreamWriter(@"C:\InstalledSoftware.txt"))
            {
                foreach (var entry in folderName.OrderBy(a => a.Key.Date))
                {
                    var folderData = entry.Value.Select(a => a.Split(',').ToList());
                    
                    #region Result In Dictionary

                    List<Dictionary<string, string>> listSoftware = new List<Dictionary<string, string>>();
                    foreach (var item in folderData.GroupBy(a => Convert.ToDateTime(a[1]).ToString("HH:mm:ss")))
                    {
                        Dictionary<string, string> keyValueList = new Dictionary<string, string>();
                        foreach (var items in item)
                        {
                            if (keyValueList.Any() && !keyValueList.Any(a => a.Key == items[0]))
                                keyValueList.Add(items[0], item.Key);
                            if (keyValueList.Count == 0)
                                keyValueList.Add(items[0], item.Key);
                        }
                        listSoftware.Add(keyValueList);
                    }
                    latestFolder.Add(entry.Key.ToString("MM/dd/yyyy"), listSoftware);

                    #endregion

                    #region Result in Physical File

                    file.WriteLine();
                    file.WriteLine("[Created Date - {0}]", entry.Key.ToString("MM/dd/yyyy"));

                    var listname = folderData.GroupBy(a => Convert.ToDateTime(a[1]).ToString("HH:mm:ss")).ToDictionary(t => t, t => t.Select(a => string.Format("{0}: {1}", Path.GetFileName(a[0]), a[1])).ToList());

                    foreach (var item in listname.OrderBy(a => a.Key.Key))
                    {
                        file.WriteLine(" Time :: {0} ", item.Key.Key);
                        foreach (var items in item.Value)
                        {
                            file.WriteLine("     FolderName :: {0}", items);
                        }
                    } 
                    #endregion
                }
            }
        }
    }
}
