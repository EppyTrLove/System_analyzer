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
using System_analyzer;
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

        Console.WriteLine("Please enter the path: ");
        var inputValue1 = Console.ReadLine();
        var dataBasePath = FileModelDaо.CreateDataBase(inputValue1);
        while (true)
        {
            Console.WriteLine("Please enter one of the options from 1 to 3:\n" +
                "1. Get top 10 by extension popularity\n2. Get top 10 by file size\n" +
                "3. Get top 10 by extension size\n4. Get info obout duplicate files\n" +
                "5. To exit");
            int tableNumber;
            if (int.TryParse(Console.ReadLine(), out tableNumber))
                if (tableNumber == 1)
                    PrintTable(Top10Service.GetTop10ByExtensionPopularity(dataBasePath), "File name", "File extension");
                else if (tableNumber == 2)
                    PrintTable(Top10Service.GetTop10ByFileSize(dataBasePath), "File name", "File size");
                else if (tableNumber == 3)
                    PrintTable(Top10Service.GetTop10ByExtensionSize(dataBasePath), "Extension name", "Extension size");
                else if (tableNumber == 4)
                    FileModelDaо.FindDuplicateFiles(dataBasePath);
                else if (tableNumber == 4)
                    return;
                else
                    Console.WriteLine("Input error!");
            Console.WriteLine();
        }
        
    }
}