using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace System_analyzer;

public class FileSystemProvider
{
    public static List<FileModel> Get(string root)
    {
        var result = new List<FileModel>();
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
                result.Add(new FileModel(di.Name, di.CreationTime, di.Extension,
                    di.GetFiles().Sum(x => x.Length)));
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
                result.Add(new FileModel(fi.Name, fi.CreationTime, fi.Extension, fi.Length));
            }
            foreach (var dir in subDirs)
                dirs.Push(dir);
        }
        return result;
    }
}

