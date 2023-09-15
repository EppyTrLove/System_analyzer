using LiteDB;
using System.Collections.Generic;


namespace System_analyzer
{
        public class LiteDbDaо
        {
            private readonly string _dataBasePath;
            public LiteDbDaо(string dataBasePath)
            {
                _dataBasePath = dataBasePath;
                _ = new LiteDatabase(_dataBasePath);
            }
            public void Add(IFileSystemItem[] models)
            {
                using (var db = new LiteDatabase(_dataBasePath))
                {
                    var col = db.GetCollection<IFileSystemItem>("Files");
                    foreach (var model in models)
                        col.Insert(model);
                }
            }
            public IEnumerable<IFileSystemItem> GetAll()
            {
                using (var db = new LiteDatabase(_dataBasePath))
                {
                    var col = db.GetCollection<IFileSystemItem>("Files");
                    return col.FindAll();
                }
            }
            public void RemoveAll()
            {
                using (var db = new LiteDatabase(_dataBasePath))
                {
                    var col = db.GetCollection<IFileSystemItem>("Files");
                    col.DeleteAll();
                }
            }
        }
    }
