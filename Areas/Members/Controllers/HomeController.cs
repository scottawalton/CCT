using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CCT.Members
{
    // Makes entire area private
    [Area("Members")]
    [Authorize]
    public class HomeController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        public class Document : PdfFile
        {
            public IFormFile file {get; set;}
        }

        #endregion

        #region Default Constructor

        public HomeController(AppDBContext context) 
        {
            mContext = context;
        }

        #endregion

        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }

        public IActionResult LoadPDF(int fileID)
        {
            var theFile = mContext.Files.FindAsync(fileID);
            var stream = new MemoryStream(theFile.Result.PDF);
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}