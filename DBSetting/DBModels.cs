﻿﻿ using System.ComponentModel.DataAnnotations;

   namespace InfTechWeb.DBSetting
{
    public class FolderModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int FolderId { get; set; }
        public string FolderName { get; set; }
        public int ParFolderId { get; set; }
    }
    
    public class FileModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public int TypeId { get; set; }
        public int FolderId { get; set; }
        public string FileContent { get; set; }
    }
    
    public class TypeModel //представляет набор объектов, которые хранятся в базе данных
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string TypeIco { get; set; }
    }
}