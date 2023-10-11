using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace System_analyzer
{
    public class DataServices
    {
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionPopularity(IEnumerable<IFileSystemItem> data)
        {
            var result = data
                .GroupBy(x => x.Extension)
                .OrderByDescending(x => x.Count())
                .Take(10)
                .SelectMany(x => x)
                .DistinctBy(x => x.Extension)
                .Select(x => (x.Name, x.Extension));
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByFileSize(IEnumerable<IFileSystemItem> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .Select(x => (x.Name, x.Size.ToString()))
             .Take(10);
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionSize(IEnumerable<IFileSystemItem> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .DistinctBy(x => x.Extension)
             .Take(10)
             .Select(x => (x.Extension, x.Size.ToString()));
            return result;
        }
        public static IFileSystemItem[] FindDuplicateFiles(IEnumerable<IFileSystemItem> data)
        {
            int count = 0;
            var result = new List<IFileSystemItem>();
            foreach (var model in data)
            {
                if (model.Size > 65000)
                    count = data.Count(x => x.Equals(model));
                if (count > 1)
                    result.Add(model);
            }
            return result.GroupBy(x => x.Name).Select(x => x.First()).ToArray();
        }
        public static IFileSystemItem[] FindDiffNameDuplicateFiles(IEnumerable<IFileSystemItem> data) 
        {     
            var model = data.ToArray();
            var result = new List<IFileSystemItem>();
            for (var i = 0; i < model.Length; i++)
            {
                for (var j = i + 1; j < model.Length; j++) 
                {
                    var hash = MD5.Create();
                    var tmpNewHash1 = hash.ComputeHash(File.ReadAllBytes(model[i].Path));
                    var tmpNewHash2 = hash.ComputeHash(File.ReadAllBytes(model[j].Path));
                    if (model[i].Size > 65000 && tmpNewHash1 == tmpNewHash2)
                        result.Add(model[i]);
                }
            }
            return result.GroupBy(x => x.Extension).Select(x => x.First()).ToArray();
        }
        public static IEnumerable<(string Name, string Value)> 
            GetDirectoryAttachmentInfo(IEnumerable<IFileSystemItem> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .Select(x => (x.Name, x.Size.ToString()));
            return result;
        }

    }
}
