using LiteDB;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System_analyzer.Daos.Abstractions;


namespace System_analyzer.Daos.NoSql
{
    public class FileSystemItemDao : IFileSystemItemDao
    {
        private const string colectionType = "Files";
        private readonly LiteDatabase _db;

        public FileSystemItemDao(string dataBasePath)
        {
            _db = new LiteDatabase(dataBasePath);
        }
        public void Add(IFileSystemItem[] models)
        {
            var col = _db.GetCollection<IFileSystemItem>(colectionType);
            foreach (var model in models)
                col.Insert(model);
        }
        public IEnumerable<IFileSystemItem> GetAll()
        {
            var col = _db.GetCollection<IFileSystemItem>(colectionType);
            return col.FindAll();
        }
        public void RemoveAll()
        {
            var col = _db.GetCollection<IFileSystemItem>(colectionType);
            col.DeleteAll();
        }
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}
