using System.IO;
using System.Linq;

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
        public FileModel[] GetAll()
        {
            return File.ReadAllLines(_dataBasePath)
                .Select(x => FileModel.FromString(x))
                .ToArray();
        }
        public void Add(FileModel[] models)
        {
            foreach (var model in models)
            File.AppendAllText(_dataBasePath, $"{FileModel.ToString(model)}\n");
        }
        public void RemoveAll()
        {
            File.Delete(_dataBasePath);
            File.Create(_dataBasePath).Dispose();
        }
    }
}

                
        
    




