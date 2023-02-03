namespace System_analyzer
{
    public class FileModel
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
    }
