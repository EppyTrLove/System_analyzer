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
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionPopularity(IEnumerable<FileModel> data)
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
        public static IEnumerable<(string Name, string Value)> GetTop10ByFileSize(IEnumerable<FileModel> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .Select(x => (x.Name, x.Size.ToString()))
             .Take(10);
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionSize(IEnumerable<FileModel> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .DistinctBy(x => x.Extension)
             .Take(10)
             .Select(x => (x.Extension, x.Size.ToString()));
            return result;
        }
        public static FileModel[] FindDuplicateFiles(IEnumerable<FileModel> data)
        {
            int count = 0;
            var result = new List<FileModel>();
            foreach (var model in data)
            {
                if (model.Size > 65000)
                    count = data.Count(x => x.Equals(model));
                if (count > 1)
                    result.Add(model);
            }
            return result.GroupBy(x => x.Name).Select(x => x.First()).ToArray();
        }
        public static FileModel[] FindDiffNameDuplicateFiles(IEnumerable<FileModel> data) 
        {     
            var modelArr = data.ToArray();
            var result = new List<FileModel>();
            for (var i = 0; i < modelArr.Length; i++)
            {
                for (var j = i + 1; j < modelArr.Length; j++) // TODO: Уточнить, что скармливаем для GetBytes
                {
                    MD5 MD5Hash = MD5.Create();
                    var tmpNewHash1 =  MD5Hash.ComputeHash
                        (Encoding.ASCII.GetBytes(modelArr[i].Size.ToString()));
                    var tmpNewHash2 =  MD5Hash.ComputeHash
                        (Encoding.ASCII.GetBytes(modelArr[j].Size.ToString()));
                    if (modelArr[i].Size > 65000 && tmpNewHash1 == tmpNewHash2)
                        result.Add(modelArr[i]);
                }
            }
            return result.GroupBy(x => x.Extension).Select(x => x.First()).ToArray();
        }
        public static FileModel[] ReScanDirectory(List<FileModel> data, string newPath) 
        {
            var newData = FileSystemProvider.Get(newPath); // TODO: Работает некорректно
                                                           // (Value does not fall within the expected range)
            for (var i = 0; i < data.Count-1; i++) 
                for (var j = 0; j < newData.Count-1; j++)
                    if (data[i].Path == newData[j].Path)
                    {
                        data.Remove(data[i]);
                        data.Add(newData[i]);
                    }
                return data.ToArray();
        }
        public static IEnumerable<(string Name, string Value)> GetDirectoryAttachmentInfo(IEnumerable<FileModel> data)
        {
            var result = data
             .OrderByDescending(x => x.Size)
             .Select(x => (x.Name, x.Size.ToString()));
            return result;
        }

    }
}
