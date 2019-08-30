using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebAppFixed.Models;

namespace WebAppFixed.Controllers
{
    public class HomeController : Controller
    {
        public ValuesContext dbV = new ValuesContext();
        public ActionResult Index()
        {
            string path = $@"\\vmware-host\Shared Folders\Projects\WebAppFixed\FormForCreatingValues\Values from the lab\{DateTime.Now.Year}\{DateTime.Now.Month}";
            
            if (System.IO.File.Exists(path + $@"\{DateTime.Now.Day}.txt"))
                if (FillDb.WritingCheck(path))
                    FillDb.FillLabVals(path);

            IEnumerable<Value> labValues = dbV.Values;                            // Получение из бд всех объекты Value
            ViewBag.Values = labValues;
           
            return View();
        }
        
        public string Set(string start, string end) => Dot.Creator(start, end);   // Получает в качестве параметра JSON строки, возвращает строку от нужного отрезка времени  
        
        public FileResult GetFile(string start, string end)
        {
            string filePath = Server.MapPath("~/Files/YourData.txt"); 
            string fileType="application/txt";
            string fileName = "YourData.txt";
            DownloadFile.Create(start, end, filePath);
            
            return File(filePath,fileType,fileName);
        }
    }
}