using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;


namespace System_analyzer
{
    public class FileModelDaо
    {
        private readonly string _dataBasePath;
        public FileModelDaо(string dataBasePath)
        {
            _dataBasePath = dataBasePath;
            if (!File.Exists(_dataBasePath))
                File.Create(_dataBasePath);
        }
        public IFileSystemItem[] GetAll()
        {
            return File.ReadAllLines(_dataBasePath)
                .Select(x => System.Text.Json.JsonSerializer.Deserialize<IFileSystemItem>(x))
                .ToArray()!;
        }
        public void Add(IFileSystemItem[] models)
        {
            foreach (var model in models)
                File.AppendAllText(_dataBasePath, $"{System.Text.Json.JsonSerializer.Serialize(model)}\n");
        }
        public void RemoveAll()
        {
            File.Delete(_dataBasePath);
            File.Create(_dataBasePath).Dispose();
        }
        public void ReScanDirecory(string path)
        {
            long startIndex = -1;
            var tail = new List<IFileSystemItem>();
            foreach (var line in File.ReadAllLines(_dataBasePath))
            {
                if (!tail.Any() && !line.Contains(path))
                    startIndex += line.Length + Environment.NewLine.Length;
                if (line.Contains(path))
                    tail.Add(System.Text.Json.JsonSerializer.Deserialize<IFileSystemItem>(line));
            }
            var dataString = tail.Select(x => $"{System.Text.Json.JsonSerializer.Serialize(x)}\n");
            using var fs = File.OpenWrite(_dataBasePath);
            fs.Position = startIndex;
            foreach (var str in dataString)
                fs.Write(Encoding.Default.GetBytes(str));
        }
    }
  
}

  



                
        
    




