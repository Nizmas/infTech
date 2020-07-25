using System;
using System.Collections.Generic;
using System.Linq;
using InfTechWeb.DBSetting;
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
        
        public string FileSave(int fileId, string fileContent)
        {
            Console.WriteLine("Saved" + fileId + fileContent);
            using ApplicationContext db = new ApplicationContext();
            var changeFile = db.Files
                .FirstOrDefault(c => c.FileId == fileId);
            if (changeFile != null)
            {
                changeFile.FileContent = fileContent;
                db.SaveChanges();
                return ("Файл успешно сохранён");
            }
            return ("Файл не найден или занят");
        }
    }
}