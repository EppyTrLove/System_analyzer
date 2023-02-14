using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;

namespace System_analyzer
{
    public class DataServices
    {
        public static IEnumerable<(string Name,  string Size)> GetInfoAboutAllFiles(IEnumerable<FileModel> data)
        {
            var result = data
                .OrderBy(x => x.Name)
                .OrderByDescending(x => x.Size)
                .Select(x => (x.Name, x.Size.ToString()));
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionPopularity(IEnumerable<FileModel>data)
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
             .DistinctBy(x=> x.Extension)
             .Take(10)
             .Select(x => (x.Extension, x.Size.ToString()));
            return result;
        }
        public static FileModel[] FindDuplicateFiles(IEnumerable<FileModel> data)
        {
            int count = 0;
            var result = new List<FileModel>();
            foreach(var model in data) 
            {
                if(model.Size > 65000)
                count = data.Count(x => x.Equals(model));
                if (count > 1)
                    result.Add(model);
            }
            return result.GroupBy(x => x.Name).Select(x => x.First()).ToArray();
        }
        public static FileModel[] FindDiffNameDuplicateFiles(IEnumerable<FileModel> data) //BUG: нужно доработать
        {
            var modelArr = data.ToArray();
            var result = new List<FileModel>();
            for (var i = 0; i < modelArr.Length; i++)
            {
                for (var j = i + 1; j < modelArr.Length; j++)
                {
                    if (modelArr[i].Size > 65000 && modelArr[i].Date == modelArr[j].Date
                        && modelArr[i].Size == modelArr[j].Size 
                        && modelArr[i].Extension == modelArr[j].Extension)
                        result.Add(modelArr[i]);
                }
            }
            return result.GroupBy(x => x.Extension).Select(x => x.First()).ToArray();
        }
        public static FileModel[] ReScanDirectory(List<FileModel> data, string root) //BUG: нужно доработать
        {
            var newData = FileSystemProvider.Get(root);
            foreach(var model in newData)
            {
                if (data.Contains(model))
                {
                    data.Remove(model);
                    data.Add(model);
                }
            } 
            return data.ToArray();
            
        }
    }
}
