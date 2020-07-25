using System.Collections.Generic;
using System.Linq;
using InfTechWeb.DBSetting;
using Microsoft.AspNetCore.Mvc;

namespace InfTechWeb.Controllers
{
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
    }
}