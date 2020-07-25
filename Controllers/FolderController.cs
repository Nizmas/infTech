using System.Collections.Generic;
using System.Linq;
using InfTechWeb.DBSetting;
using Microsoft.AspNetCore.Mvc;

namespace InfTechWeb.Controllers
{
    public class FolderController : Controller
    {
        public JsonResult FolderContent(int folderId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<FolderModel> folders = new List<FolderModel>();
            List<FileModel> files = new List<FileModel>();
            var allFolders = db.Folders.ToList();
            foreach (var af in allFolders)
            {
                if (af.ParFolderId==folderId) folders.Add(af);
            }
            var allFiles = db.Files.ToList();
            foreach (var af in allFiles)
            {
                if (af.FolderId==folderId) files.Add(af);
            }
            return Json(new {folders, files});
        }
    }
}