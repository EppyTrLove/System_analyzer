using System;
using ConsoleTables;
using System.Collections.Generic;
using System_analyzer;
using System.Transactions;

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
        Console.WriteLine("Please enter the path: ");
        var inputValue1 = Console.ReadLine();
        var data = FileSystemProvider.Get(inputValue1);
        fileModelDao.Add(data.ToArray()); 
        while (true)
        {
            Console.WriteLine("Please enter one of the options from 1 to 7:\n" +
                "1. Get indo obout all files in directory\n" +
                "2. Get top 10 by extension popularity\n3. Get top 10 by file size\n" +
                "4. Get top 10 by extension size\n5. Get info obout big duplicate files\n" +
                "6. Get info about about duplicate renamed files\n" +
                "7. To exit");
            int serviceNumber;
            if (int.TryParse(Console.ReadLine(), out serviceNumber))
                if(serviceNumber == 1)
                    PrintTable(DataServices.GetInfoAboutAllFiles(data), "File name", "File size");
                else if (serviceNumber == 2)
                    PrintTable(DataServices.GetTop10ByExtensionPopularity(data), "File name", "File extension");
                else if (serviceNumber == 3)
                    PrintTable(DataServices.GetTop10ByFileSize(data), "File name", "File size");
                else if (serviceNumber == 4)
                    PrintTable(DataServices.GetTop10ByExtensionSize(data), "Extension name", "Extension size");
                else if (serviceNumber == 5)
                    foreach (var model in DataServices.FindDuplicateFiles(data))
                    {
                        Console.WriteLine($"В выбранном каталоге содержатся дубликаты фйла {model}");
                    }
                else if (serviceNumber == 6)
                    foreach (var model in DataServices.FindDiffNameDuplicateFiles(data))
                    {
                        Console.WriteLine($"В выбранном каталоге содержатся переименованные дубликаты файла {model}");
                    }
                else if (serviceNumber == 7)
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