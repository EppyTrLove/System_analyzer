using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace System_analyzer
{
    public class FileModel : IFileSystemItem
    {
        public FileModel(string fullName, DateTime date, string extension, long size)
        {
            Path = fullName.Substring(0, fullName.LastIndexOf("\\"));
            Name = fullName.Split("\\").Last();
            Date = date;
            Extension = extension;
            Size = size;
            Id = Path.GetDeterministicHashCode();
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public long Id { get; set; }

        public bool Equals(FileModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Extension == other.Extension && Size == other.Size;
        }

    }
    public class DirectoryModel : IFileSystemItem
    {
        public DirectoryModel(string fullName, string extension, long size)
        {
            Path = fullName.Substring(0, fullName.LastIndexOf("\\"));
            Name = fullName.Split("\\").Last();
            Extension = extension;
            Size = size;
            Id = Path.GetHashCode();
        }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public long Id { get; set; }


        public bool Equals(DirectoryModel? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Size == other.Size;
        }
    }
    public interface IFileSystemItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public long Id { get; set; }


        public bool Equals(IFileSystemItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Size == other.Size;
        }
    }
    public class AttachmentInfoModel
    {
        public AttachmentInfoModel(string fullName, string extension, long size)
        {
            FullName = fullName;
            Extension = extension;
            Size = size;
        }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
    }
}
