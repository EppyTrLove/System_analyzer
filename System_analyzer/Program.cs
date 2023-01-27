using System;
using ConsoleTables;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Diagnostics;

class FileModel
{
    private const string Separator = ";";
    public FileModel(string name, DateTime date, string extension, long size)
    {
        Name = name;
        Size = size;
        Extension = extension;
        Date = date;
    }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Extension { get; set; }
    public long Size { get; set; }

    public override string ToString()
    {
        return $"{Name}{Separator}{Date}{Separator}{Extension}{Separator}{Size}{Environment.NewLine}";
    }
    public static FileModel FromString(string source)
    {
        var arr = source.Split(Separator);
        return new FileModel(arr[0], DateTime.Parse(arr[1]), arr[2], long.Parse(arr[3]));
    }
}
class Programm
{
    public static void Walk(string root)
    {
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
            foreach (string file in files)
            {
                var fi = new FileInfo(file);
                if (!File.ReadAllText(@"C:\Solvery1\File_DataBase.txt").Contains(fi.Name))
                    File.AppendAllText(@"C:\Solvery1\File_DataBase.txt",
                        new FileModel(fi.Name, fi.CreationTime, fi.Extension, fi.Length).ToString());
            }
            foreach (var dir in subDirs)
                dirs.Push(dir);
        }
    }
    private static IEnumerable<(string Name, string Value)> GetTop10ByExtensionPopularity(string path
        = @"C:\Solvery1\File_DataBase.txt")
    {
        var data = File.ReadLines(path)
            .Select(x => FileModel.FromString(x));
        var result = data
            .GroupBy(x => x.Extension)
            .OrderByDescending(x => x.Count())
            .Take(10)
            .SelectMany(x => x);
        return result;
    }
    private static IEnumerable<(string Name, string Value)> GetTop10ByFileSize(string path
        = @"C:\Solvery1\File_DataBase.txt")
    {
        var data = File.ReadLines(path)
            .Select(x => FileModel.FromString(x));
        var result = data
         .OrderByDescending(x => x.Size)
         .Take(10);
        return result;
    }
    private static IEnumerable<(string Name, string Value)> GetTop10ByExtensionSize(string path
        = @"C:\Solvery1\File_DataBase.txt")
    {
        var data = File.ReadLines(path)
            .Select(x => FileModel.FromString(x));
        var result = data
         .OrderByDescending(x => x.Size)
         .Take(10);
        return result;
    }


    private static void PrintTable(IEnumerable<(string Name, string Value)> dataList,
          string name, string valueName)
    {
        var tableSize = new ConsoleTable(name, valueName);
        foreach (var item in dataList)
            tableSize.AddRow(item.Name, item.Value);
        tableSize.Write();
    }

    public static void Main(string[] args)
    {
        // Data acsess Доступ к данным(Работа с файловой системой)
        // Buiseness logic Бизнес-логика(Сортировка)
        // Presentation Представлеие(Вывод в консоль)
        Console.WriteLine("Please enter the path: ");
        var inputValue1 = Console.ReadLine();
        Walk(@inputValue1);
        Console.WriteLine("Please enter one of the options from 1 to 3:\n" +
            "1. Get top 10 by extension popularity\n2. Get top 10 by file size\n" +
            "3. Get top 10 by extension size");
        int tableNumber;
        bool sucsess = int.TryParse(Console.ReadLine(), out tableNumber);
        if (sucsess)
            if (tableNumber == 1)
                PrintTable(GetTop10ByExtensionPopularity(), "File name", "File extension");
            else if (tableNumber == 2)
                PrintTable(GetTop10ByFileSize, "File name", "File size");
            else if (tableNumber == 3)
                PrintTable(GetTop10ByExtensionSize, "Extension name", "Extension size");
            else
                Console.WriteLine("Input error!");
    }
}