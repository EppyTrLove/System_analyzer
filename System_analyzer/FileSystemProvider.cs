using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace System_analyzer
{

    public class FileSystemProvider
    {
        public static List<IFileSystemItem> Get(string root)
        {
            var result = new List<IFileSystemItem>();
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
                    result.Add(new DirectoryModel(di.FullName, di.Extension, new DirectoryInfo(di.FullName)
                        .GetFiles("*.*", SearchOption.AllDirectories)
                        .Select(x => x.Length)
                        .Sum()));
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
                    result.Add(new FileModel(fi.FullName, fi.CreationTime, fi.Extension, fi.Length));
                }
                foreach (var dir in subDirs)
                    dirs.Push(dir);
            }
            return result;
        }
        public static List<IFileSystemItem> DirectoryAttachmentInfo(string newPath, IEnumerable<IFileSystemItem> data)
        {
            if (!Directory.Exists(newPath))
                throw new ArgumentException();
            else if (!data.Any(x => x.Path.Contains(newPath!)))
            {
                var result = new List<IFileSystemItem>();
                FileInfo[] files;
                DirectoryInfo[] subDirs;
                files = new DirectoryInfo(newPath).GetFiles();
                foreach (var fi in files)
                    result.Add(new FileModel(fi.FullName, fi.CreationTime, fi.Extension, fi.Length));
                subDirs = new DirectoryInfo(newPath).GetDirectories();
                foreach (var dirInfo in subDirs)
                    result.Add(new DirectoryModel(dirInfo.FullName, dirInfo.Extension, new DirectoryInfo(dirInfo.FullName)
                        .GetFiles("*.*", SearchOption.AllDirectories)
                        .Select(x => x.Length)
                        .Sum()));
                return result;
            }
            // Не доделано
        }

    }
}
  


