﻿﻿ using System.ComponentModel.DataAnnotations;

   namespace InfTechWeb.DBSetting
{
    public class FoldersModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int ParFolderId { get; set; }
    }
    
    public class FilesModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public int ExtensionId { get; set; }
        public int FolderId { get; set; }
        public string FileContent { get; set; }
    }
    
    public class ExtensionsModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int ExtensionId { get; set; }
        public string ExtensionName { get; set; }
        public byte[] ExtensionIco { get; set; }
    }
}