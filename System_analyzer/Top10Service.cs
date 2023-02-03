namespace System_analyzer
{
    public class Top10Service
    {
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionPopularity(string path)
        {
            var data = File.ReadLines(path)
                .Select(x => FileModel.FromString(x));
            var result = data
                .GroupBy(x => x.Extension)
                .OrderByDescending(x => x.Count())                
                .Take(10)
                .SelectMany(x => x)
                .DistinctBy(x => x.Extension)
                .Select(x => (x.Name, x.Extension));
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByFileSize(string path)
        {
            var data = File.ReadLines(path)
                .Select(x => FileModel.FromString(x));
            var result = data
             .OrderByDescending(x => x.Size)
             .Select(x => (x.Name, x.Size.ToString()))
             .Take(10);
            return result;
        }
        public static IEnumerable<(string Name, string Value)> GetTop10ByExtensionSize(string path)
        {
            var data = File.ReadLines(path)
                .Select(x => FileModel.FromString(x));
            var result = data
             .OrderByDescending(x => x.Size)
             .DistinctBy(x=> x.Extension)
             .Take(10)
             .Select(x => (x.Extension, x.Size.ToString()));
            return result;
        }
    }
}
