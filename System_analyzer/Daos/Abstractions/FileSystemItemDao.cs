using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace System_analyzer.Daos.Abstractions
{

    public interface IFileSystemItemDao : IDisposable
    { 
    public void Add(IFileSystemItem[] models);
    public IEnumerable<IFileSystemItem> GetAll();
    public void RemoveAll();
    }

}
