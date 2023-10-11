using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System_analyzer.Daos.Abstractions;
using System.Runtime.Serialization;

namespace System_analyzer.Daos.FileSystem
{
    public class FileSystemItemDao : IFileSystemItemDao
    {
        private readonly string _dataBasePath;
        public FileSystemItemDao(string dataBasePath)
        {
            _dataBasePath = dataBasePath;
            if (!File.Exists(_dataBasePath))
                File.Create(_dataBasePath);
        }
        public IEnumerable<IFileSystemItem> GetAll()
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
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}












