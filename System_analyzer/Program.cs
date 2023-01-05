using System;
using ConsoleTables;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
class FileModel
{
    public const string Separator = ";";
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
        return $"{Name}{Separator}{Date}{Separator}{Extension}{Separator}{Size}";
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
        const string Separator = ";";
        FileInfo[] files = null;
        DirectoryInfo[] subDirs = null;
        // Получаем все файлы в текущем каталоге
        files = root.GetFiles();
        if (files != null)
        {
            //выводим имена файлов в консоль
            foreach (FileInfo fi in files)
                File.AppendAllText(@"C:\Solvery1\File_DataBase.txt", fi.FullName + Separator + fi.CreationTime +
                    Separator + fi.Extension + Separator + fi.Length + Environment.NewLine);
            //получаем все подкаталоги
            subDirs = root.GetDirectories();
            //проходим по каждому подкаталогу
            foreach (DirectoryInfo dirInfo in subDirs)
            {
                //рекурсия
                Walk(dirInfo);
            }
        }
    }
    public static void Main(string[] args)
    {
        Walk(new DirectoryInfo(@"C:\Users\eppy_\OneDrive\Рабочий стол\С#"));
        var sizeTable = new ConsoleTable("File name", "File size");
        var extensionTable = new ConsoleTable("File name", "File extension");
        var dateTimeTable = new ConsoleTable("File name", "File date");
        foreach (string line in File.ReadLines(@"C:\Solvery1\File_DataBase.txt"))
        {
            string[] ns = line.Split(';');
            sizeTable.AddRow(ns[0], ns[3]);
            extensionTable.AddRow(ns[0], ns[2]);
            dateTimeTable.AddRow(ns[0], ns[1]);
        }
        //sizeTable.Rows.OrderByDescending(y => y.Length);
        sizeTable.Write();
        extensionTable.Write();
        dateTimeTable.Write();
    }
}



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

