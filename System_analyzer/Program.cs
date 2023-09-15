using System;
using ConsoleTables;
using System.Collections.Generic;
using System_analyzer;
using System.Linq;
using LiteDB;

class Programm
{

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
        var fileModelDao = new FileModelDaо($@"{Environment.CurrentDirectory}\Database.txt");
        var localData = new List<IFileSystemItem>();
        var data = fileModelDao.GetAll().ToList();
        Console.WriteLine("Please enter the path: ");
        var inputValue = Console.ReadLine();
        if (!data.Any(x => x.Path.Contains(inputValue!)))
        {
            localData = FileSystemProvider.Get(inputValue!);
            fileModelDao.Add(localData.ToArray());
            data.AddRange(localData);
        }
        while (true)
        {
            Console.WriteLine("Please enter one of the options from 1 to 7:\n" +
                "1. Get top 10 by extension popularity\n" + 
                "2. Get top 10 by file size\n" +
                "3. Get top 10 by extension size\n" + 
                "4. Get info obout big duplicate files\n" +
                "5. Get info about about duplicate renamed files\n" +
                "6. Rescan the directory\n" + 
                "7. Get information about directory attachment\n" + 
                "8. To exit");
            int serviceNumber;
            if (int.TryParse(Console.ReadLine(), out serviceNumber))
                if (serviceNumber == 1)
                    PrintTable(DataServices.GetTop10ByExtensionPopularity(data), "File name", "File extension");
                else if (serviceNumber == 2)
                    PrintTable(DataServices.GetTop10ByFileSize(data), "File name", "File size");
                else if (serviceNumber == 3)
                    PrintTable(DataServices.GetTop10ByExtensionSize(data), "Extension name", "Extension size");
                else if (serviceNumber == 4)
                    foreach (var model in DataServices.FindDuplicateFiles(data))
                    {
                        Console.WriteLine($"В выбранном каталоге содержатся дубликаты фйла {model}");
                    }
                else if (serviceNumber == 5)
                    foreach (var model in DataServices.FindDiffNameDuplicateFiles(data))
                    {
                        Console.WriteLine($"В выбранном каталоге содержатся переименованные дубликаты файла {model}");
                    }
                else if (serviceNumber == 6)
                {
                    Console.WriteLine("Please enter the path you want to rescan:");
                    var path = Console.ReadLine();
                    data.RemoveAll(x => x.Path.Contains(path!));
                    fileModelDao.ReScanDirecory(path!);
                }
                else if (serviceNumber == 7)
                {
                    Console.WriteLine("Please enter the path you want to get info:");
                    inputValue = Console.ReadLine();
                    localData = FileSystemProvider.DirectoryAttachmentInfo(inputValue!);
                    PrintTable(DataServices.GetDirectoryAttachmentInfo(localData), "Name of Item", "File size");

                }
                else if (serviceNumber == 8)
                {
                    fileModelDao.RemoveAll();
                    return;
                }
                else
                    Console.WriteLine("Input error!"); 
            Console.WriteLine();
        }
        
    }
}