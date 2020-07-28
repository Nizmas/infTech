using System.ComponentModel.DataAnnotations;

namespace InfTechWeb.Models
{
    public class FileModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public int ExtensionId { get; set; }
        public int FolderId { get; set; }
        public string FileContent { get; set; }
    }
}