using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfTechWeb.DBSetting;
using Microsoft.AspNetCore.Mvc;

namespace InfTechWeb.Controllers
{
    public class FolderController : Controller
    {
        public JsonResult FolderContent(int folderId)
        {
            using ApplicationContext db = new ApplicationContext();
            List<FoldersModel> folders = new List<FoldersModel>();
            List<FilesModel> files = new List<FilesModel>();
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
        
        public async Task<string> FolderAdd(string folderName, int parfolderId)
        {
            await using ApplicationContext db = new ApplicationContext();
            FoldersModel fld = new FoldersModel {ParFolderId = parfolderId, FolderName = folderName};
            try
            {
                await db.Folders.AddAsync(fld);
                await db.SaveChangesAsync();
                return ($"Папка {folderName} создана");
            }
            catch
            {
                return ($"Ошбика! Не удалось создать папку!");
            }
        }
        
        public async Task<string> FolderDelete(int folderId)
        {
            await using ApplicationContext db = new ApplicationContext();
            
            FoldersModel fld = new FoldersModel {FolderId = folderId};
            List<FoldersModel> fldParents = new List<FoldersModel> {fld};
            List<FoldersModel> fldAllChildren = new List<FoldersModel>();
            //List<FilesModel> files= new List<FilesModel>();
            
            while (fldParents.Count > 0)
            {
                foreach (FoldersModel par in fldParents) 
                {
                    List<FoldersModel> fldChild  = db.Folders
                        .Where(c => c.ParFolderId == par.FolderId)
                        .ToList();
                    fldAllChildren.AddRange(fldChild);
                    
                    List<FilesModel> files= db.Files
                        .Where(c => c.FolderId == par.FolderId)
                        .ToList();
                    foreach (var file in files)
                    {
                        db.Files.Attach(file);
                        db.Files.Remove(file);
                    }
                    
                    db.Folders.Attach(par);
                    db.Folders.Remove(par);
                }
                fldParents = fldAllChildren;
                fldAllChildren = new List<FoldersModel>();
            }
            await db.SaveChangesAsync();
            return ($"Директория #{folderId} удалена");
        }
        
        public async Task<string> FolderRename(int folderId, string newName)
        {
            await using ApplicationContext db = new ApplicationContext();
            FoldersModel file = new FoldersModel();
            var findFld = db.Folders
                .FirstOrDefault(c => c.FolderId== folderId);
            if (findFld != null)
            {
                findFld.FolderName = newName;
                await db.SaveChangesAsync();
                return ("Папка успешно переименована");
            }
            return ("Папка не найдена");
        }
    }
}