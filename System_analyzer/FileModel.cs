using System;
using System.Collections;
using System.Text.Json;

namespace System_analyzer
{
    public class FileModel : IEquatable<FileModel>
    {
            private const string Separator = ";";
            public FileModel(string name, DateTime date, string extension, long size)
            {
                Name = name;
                Date = date;
                Extension = extension;
                Size = size;
        }
            public string Name { get; set; }
            public DateTime Date { get; set; }
            public string Extension { get; set; }
            public long Size { get; set; }

            public static string ToString(FileModel model)
            {
                return JsonSerializer.Serialize(model);
            }
            public static FileModel FromString(string source)
            {
                var model = JsonSerializer.Deserialize<FileModel>(source);
                return new FileModel(model.Name, model.Date, model.Extension, model.Size);
            }

        public bool Equals(FileModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Extension== other.Extension && Size == other.Size;
        }
    }
    }
