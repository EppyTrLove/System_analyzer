using System.ComponentModel;

namespace System_analyzer
{
    public class FileModelDaо
    {
        public static string CreateDataBase(string root)
        {
            var dataBasePath = $@"{Environment.CurrentDirectory}\Database.txt";
            File.Create(dataBasePath).Dispose();
            var dirs = new Stack<string>();
            if (!Directory.Exists(root))
                throw new ArgumentException();
            dirs.Push(root);
            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                string[] subDirs = null;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);

                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                foreach (var dir in subDirs)
                {
                    var di = new DirectoryInfo(dir);
                    File.AppendAllText(dataBasePath, new FileModel(di.Name, di.CreationTime, di.Extension,
                        di.GetFiles().Sum(x => x.Length)).ToString());
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                foreach (var file in files)
                {
                    var fi = new FileInfo(file);
                    File.AppendAllText(dataBasePath, new FileModel(fi.Name, fi.CreationTime,
                        fi.Extension, fi.Length)
                        .ToString());
                }
                foreach (var dir in subDirs)
                    dirs.Push(dir);
            }
            return dataBasePath;
        }
        public static void FindDuplicateFiles(string path)
        {
            List<string> lines = File.ReadAllLines(path).ToList();
            var str = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                int count = lines.Count(str => str == lines[i]);
                if (count > 1)
                    str.Add($"Файл '{lines[i]}' дублируется в каталоге {count} раз(-а).");
            }
            foreach (var i in str.Distinct())
                Console.WriteLine(i);
        }
    }
}

                
        
    




