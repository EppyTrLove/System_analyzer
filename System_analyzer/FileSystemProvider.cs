using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Net.WebRequestMethods;


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
                result.Add(new FileModel(di.FullName, di.CreationTime, di.Extension,
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
                result.Add(new FileModel(fi.FullName, fi.CreationTime, fi.Extension, fi.Length));
            }
            foreach (var dir in subDirs)
                dirs.Push(dir);
        }
        return result;
    }
    public static List<FileModel> DirectoryAttachmentInfo(string path) 
    {
        var result = new List<FileModel>();
        if (!Directory.Exists(path))
            throw new ArgumentException();
        FileInfo[] files;
        DirectoryInfo[] subDirs;
        files = new DirectoryInfo(path).GetFiles();
        foreach (var fi in files)
            result.Add(new FileModel(fi.FullName, fi.CreationTime, fi.Extension, fi.Length));
        subDirs = new DirectoryInfo(path).GetDirectories();
        foreach (var dirInfo in subDirs) 
            result.Add(new FileModel(dirInfo.FullName, dirInfo.CreationTime, dirInfo.Extension,
               new DirectoryInfo(dirInfo.FullName)
                .GetFiles("*.*", SearchOption.AllDirectories)
                .Select(x => x.Length)
                .Sum()));
        return result;

    }
}  


