using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InfTechWeb.DBSetting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InfTechWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace InfTechWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var folders = db.Folders.ToList();
                Console.WriteLine("Список объектов:");
                foreach (FoldersModel fu in folders)
                {
                    Console.WriteLine($"{fu.FolderId}.{fu.FolderName} - {fu.ParFolderId}");
                    if (fu.ParFolderId == 0)
                    {
                        ViewBag.Project = fu;
                    }
                }
            }
            return View();
        }
    }
}