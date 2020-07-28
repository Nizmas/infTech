using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InfTechWeb.DBSetting;
using InfTechWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfTechWeb.Controllers
{
    /// <summary>
    /// Контроллер для работы с расширениями
    /// </summary>
    public class ExtensionController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        public ExtensionController( IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }
        
        public JsonResult ExtensionContent()
        {
            using ApplicationContext db = new ApplicationContext();
            var allExtensions = db.Extensions.ToList();
            return new JsonResult(allExtensions);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, string uploadedName)
        {
            if (uploadedFile != null)
            {
                byte[] imageData = null;
                using var binaryReader = new BinaryReader(uploadedFile.OpenReadStream());
                imageData = binaryReader.ReadBytes((int)uploadedFile.Length);

                await using ApplicationContext db = new ApplicationContext();
                ExtensionsModel ext = new ExtensionsModel {ExtensionName = uploadedName, ExtensionIco = imageData};
                await db.Extensions.AddAsync(ext);
                await db.SaveChangesAsync();
            }
            return Redirect("/Home");
        }
        
        public JsonResult ExtensionsList()
        {
            using ApplicationContext db = new ApplicationContext();
            var allExtensions = db.Extensions.ToList();
            return Json(allExtensions);
        }
        
        public string ExtensionDelete(int extensionId)
        {
            using ApplicationContext db = new ApplicationContext();
            ExtensionsModel ext = new ExtensionsModel {ExtensionId = extensionId};
            try
            {
                db.Extensions.Attach(ext);
                db.Extensions.Remove(ext);
                db.SaveChanges();
                return ($"Расширение #{extensionId} удалено");
            }
            catch
            {
                return ($"Ошбика! Расширение #{extensionId} не найдено!");
            }
        }
    }
}