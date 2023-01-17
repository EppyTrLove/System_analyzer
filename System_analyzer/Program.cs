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
    public static void Walk(DirectoryInfo root)
    {
        FileInfo[] files;
        DirectoryInfo[] subDirs;
        try
        {
            files = root.GetFiles();
            foreach (var fi in files)
                //условие ниже работает некорректно
                if (!File.ReadAllText(@"C:\Solvery1\File_DataBase.txt").Contains(fi.FullName))
                File.AppendAllText(@"C:\Solvery1\File_DataBase.txt",
                        new FileModel(fi.FullName, fi.CreationTime, fi.Extension, fi.Length).ToString());
            subDirs = root.GetDirectories();
            foreach (var dirInfo in subDirs)
                {
                File.AppendAllText(@"C:\Solvery1\File_DataBase.txt",
                        new FileModel(dirInfo.FullName, dirInfo.CreationTime, dirInfo.Extension,
                        dirInfo.GetFiles().Sum(x => x.Length)).ToString());
                Walk(dirInfo);
                 }
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("DirectoryNotFoundException: The selected directory was not found");
        }
        catch (UnauthorizedAccessException)
        {
      
            Console.WriteLine($"UnAuthorizedAccessException: Unable to access file");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    private static void SortData(string path, out IEnumerable<FileModel> extensionList, 
        out IEnumerable<FileModel> sizeList, out IEnumerable<FileModel> creationList)
    {
        var data = new List<FileModel>();
        foreach (var line in File.ReadLines(path))
        {
            var ns = FileModel.FromString(line);
            data.Add(ns);
        }

        extensionList = data
            .OrderByDescending(x => x.Size)
            .DistinctBy(x => x.Extension)
            .Take(10);
        sizeList = data
            .OrderByDescending(x => x.Size)
            .Take(10);
         creationList = data
            .OrderByDescending(x => x.Date)
            .Take(10);
        //countList = data
        //    .OrderByDescending(x => x.Extension.Count())
        //    .Take(10);
    }
    private static void PrintTable(IEnumerable<FileModel> dataList,
          string name, string valueName)
    {
        switch (valueName)
        {
            case "File size":
                {
                    var tableSize = new ConsoleTable(name, valueName);
                    foreach (var item in dataList)
                        tableSize.AddRow(item.Name, item.Size);
                    tableSize.Write();
                    break;
                }
            case "File date":
                {
                    var tableDate = new ConsoleTable(name, valueName);
                    foreach (var item in dataList)
                        tableDate.AddRow(item.Name, item.Date);
                    tableDate.Write();
                    break;
                }
            case "File extension":
                {
                    var tableExtension = new ConsoleTable(name, valueName);
                    foreach (var item in dataList)
                        tableExtension.AddRow(item.Name, item.Extension);
                    tableExtension.Write();
                    break;
                }
            case "Directory size":
                {
                    var tableDirSize = new ConsoleTable(name, valueName);
                    foreach (var item in dataList)
                        if(item.Name.Contains(""))
                        tableDirSize.AddRow(item.Name, item.Size);
                    tableDirSize.Write();
                    break;
                }
            case "Extension size":
                {
                    var tableDirSize = new ConsoleTable(name, valueName);
                    foreach (var item in dataList)
                            tableDirSize.AddRow(item.Extension, item.Size);
                    tableDirSize.Write();
                    break;
                }
        }  
    }
    public static void Main(string[] args)
    {
        // Data acsess Доступ к данным(Работа с файловой системой)
        // Buiseness logic Бизнес-логика(Сортировка)
        // Presentation Представлеие(Вывод в консоль)

        IEnumerable<FileModel> extensionList;
        IEnumerable<FileModel> sizeList;
        IEnumerable<FileModel> creationList;
        IEnumerable<FileModel> countList;
        Walk(new DirectoryInfo(@"C:\"));
        SortData(@"C:\Solvery1\File_DataBase.txt", out extensionList, out sizeList, out creationList);
        PrintTable(sizeList, "File name", "File size");
        PrintTable(creationList, "File name", "File date");
        PrintTable(extensionList, "File name", "File extension");
        PrintTable(sizeList, "Directory name", "Directory size");
        PrintTable(extensionList, "Extension name", "Extension size");
    }
}








//class FileModel
//{
//    public const string Separator = ";";
//    public FileModel(string name, DateTime date, string extension, long size)
//    {
//        Name = name;
//        Size = size;
//        Extension = extension;
//        Date = date;
//    }
//    public string Name { get; set; }
//    public DateTime Date { get; set; }
//    public string Extension { get; set; }
//    public long Size { get; set; }

//    public override string ToString()
//    {
//        return $"{Name}{Separator}{Date}{Separator}{Extension}{Separator}{Size}";
//    }
//    public static FileModel FromString(string source)
//    {
//        var arr = source.Split(Separator);
//        return new FileModel(arr[0], DateTime.Parse(arr[1]), arr[2], long.Parse(arr[3]));
//    }
//}
//class Program
//{
//    public static void Walk(DirectoryInfo root)
//    {
//        const string Separator = ";";
//        FileInfo[] files = null;
//        DirectoryInfo[] subDirs = null;
//        // Получаем все файлы в текущем каталоге
//        files = root.GetFiles();
//        if (files != null)
//        {
//            //выводим имена файлов в консоль
//            foreach (var fi in files)
//                File.AppendAllText(@"C:\Solvery1\File_DataBase.txt", fi.FullName + Separator + fi.CreationTime +
//                    Separator + fi.Extension + Separator + fi.Length + Environment.NewLine);
//            //получаем все подкаталоги
//            subDirs = root.GetDirectories();
//            //проходим по каждому подкаталогу
//            foreach (var dirInfo in subDirs)
//            {
//                //рекурсия
//                Walk(dirInfo);
//            }
//        }
//    }
//    public static void Main(string[] args)
//    {
//        Walk(new DirectoryInfo(@"C:\Users\eppy_\OneDrive\Рабочий стол\С#"));
//        var sizeTable = new ConsoleTable("File name", "File size");
//        var extensionTable = new ConsoleTable("File name", "File extension");
//        var dateTimeTable = new ConsoleTable("File name", "File date");
//        foreach (var line in File.ReadLines(@"C:\Solvery1\File_DataBase.txt"))
//        {
//            string[] ns = line.Split(';');
//            sizeTable.AddRow(ns[0], ns[3]);
//            extensionTable.AddRow(ns[0], ns[2]);
//            dateTimeTable.AddRow(ns[0], ns[1]);
//        }
//        //sizeTable.Rows.OrderByDescending(y => y.Length);
//        sizeTable.Write();
//        extensionTable.Write();
//        dateTimeTable.Write();
//    }
//}



//foreach (string text in newText)
//{
//    Console.WriteLine(text);
//}
//sizeTable.Write();


//    var c = FileModel.FromString(sr.ReadLine());
//var sizeTable = new ConsoleTable("File name", "File size");
//var extensionTable = new ConsoleTable("File name", "File extension");
//var dateTimeTable = new ConsoleTable("File name", "File date");
//sizeTable.AddRow(c.Name, c.Size);
//sizeTable.Write();
//extensionTable.AddRow(c.Name, c.Extension);
//extensionTable.Write();
//dateTimeTable.AddRow(c.Name, c.Date);
//dateTimeTable.Write();




//ileModel.FromString(@"C:\Solvery1\File_DataBase.txt");
//if (fileInfo.Exists)
//{
//File.Create(@"C:\Solvery1\File_DataBase.txt");
// using (var sw = fileInfo.CreateText())
//  {
//     sw.WriteLine(fileInfo.Name + ";" + fileInfo.CreationTime + ";" + fileInfo.Length);
// }
//if (fileInfo.Exists)
//{
//    foreach (var file in Directory.GetFiles(@"C:\Solvery", "*", SearchOption.AllDirectories))
//{
//    var fileInfo = new FileInfo(@"C:\Solvery");
//    File.AppendAllText(@"C:\Solvery1\File_DataBase.txt", fileInfo.FullName + ";" + fileInfo.CreationTime +
//    ";" + fileInfo.Extension + fileInfo.Length+ ";" + Environment.NewLine);
//}
//}
//var dir = new DirectoryInfo(@"C:\Solvery");
//foreach (var file in Directory.EnumerateFiles(@"C:\Solvery", "*", SearchOption.AllDirectories))
//File.AppendAllText(@"C:\Solvery\File_DataBase.txt", file);
// ProcessDirectory(@"C:\Solvery");

