using System;
using System.Linq;
using System.Threading.Tasks;
using InfTechWeb.DBSetting;
using InfTechWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace InfTechWeb.Controllers
{
    /// <summary>
    /// Контроллер для работы с файлами
    /// </summary>
    public class FileController : Controller
    {
        public JsonResult FileContent(int fileId)
        {
            using ApplicationContext db = new ApplicationContext();

            var allFiles = db.Files.ToList();
            foreach (var af in allFiles)
            {
                if (af.FileId==fileId) return Json(af);
            }
            return new JsonResult(BadRequest(new { message = "bad request"}));
        }
        
        [HttpPost]
        public async Task<string> FileSave([FromBody] FileModel file)
        {
            Console.WriteLine("Saved" + file.FileId + file.FileContent);
            await using ApplicationContext db = new ApplicationContext();
            var changeFile = db.Files
                .FirstOrDefault(c => c.FileId == file.FileId);
            if (changeFile != null)
            {
                changeFile.FileContent = file.FileContent;
                await db.SaveChangesAsync();
                return ("Файл успешно сохранён");
            }
            return ("Файл не найден или занят");
        }
        
        public async Task<string> FileDelete(int fileId)
        {
            Console.WriteLine(fileId);
            await using ApplicationContext db = new ApplicationContext();
            FilesModel file = new FilesModel {FileId = fileId};
            try
            {
                db.Files.Attach(file);
                db.Files.Remove(file);
                await db.SaveChangesAsync();
                return ($"Файл #{fileId} удален");
            }
            catch
            {
                return ($"Ошбика! Не удалось удалить файл #{fileId!}");
            }
        }
        
        [HttpPost]
        public async Task<string> FileUpload([FromBody] FileModel fileUp)
        {
            if (fileUp.FileDescription == "")  fileUp.FileDescription = fileUp.FileContent.Substring(0, 20);
            Console.WriteLine(fileUp.FileName + fileUp.FileDescription + fileUp.FolderId + fileUp.FileContent);
            string extension = fileUp.FileName.Substring(fileUp.FileName.LastIndexOf(".", StringComparison.Ordinal)+1);
            await using ApplicationContext db = new ApplicationContext();
            ExtensionsModel ext = new ExtensionsModel();
            var findExt = db.Extensions
                .FirstOrDefault(c => c.ExtensionName == extension);
            if (findExt != null)
            {
                FilesModel file = new FilesModel
                {
                    FileContent = fileUp.FileContent, 
                    FileDescription = fileUp.FileDescription,
                    FileName = fileUp.FileName,
                    FolderId = fileUp.FolderId,
                    ExtensionId = findExt.ExtensionId
                };
                try
                {
                    await db.Files.AddAsync(file);
                    await db.SaveChangesAsync();
                    return ("Файл успешно сохранён");
                }
                catch
                {
                    return ("Невозможно сохранить файл!");
                }
            }
            return ("Неподдерживаемый формат файла!");
        }
        
        public async Task<string> FileRename(int fileId, string newName)
        {
            await using ApplicationContext db = new ApplicationContext();
            FilesModel file = new FilesModel();
            var findFile = db.Files
                .FirstOrDefault(c => c.FileId== fileId);
            if (findFile != null)
            {
                findFile.FileName = newName + findFile.FileName.Substring(findFile.FileName.LastIndexOf(".", StringComparison.Ordinal));
                await db.SaveChangesAsync();
                return ("Файл успешно сохранён");
            }
            return ("Файл не найден");
        }
        
        public async Task<Object> FileDownload(int fileId)
        {
            await using ApplicationContext db = new ApplicationContext();
            FilesModel file = new FilesModel();
            var findFile = db.Files
                .FirstOrDefault(c => c.FileId== fileId);
            if (findFile != null)
            {
                string filePath = "wwwroot/resources/boofer.txt";
                await System.IO.File.WriteAllTextAsync(filePath, findFile.FileContent);
                string fileType = "application/txt";
                return File("~/resources/boofer.txt", fileType, findFile.FileName);
            }
            return ("Файл не найден");
        }
    }
}