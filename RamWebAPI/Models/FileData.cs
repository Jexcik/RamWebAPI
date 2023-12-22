using System;

namespace RamWebAPI.Models
{
    public class FileData
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string LocalFileFolder { get; set; }
        public DateTime Date { get; set; }
    }
}
