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

namespace CCT
{
    public class DocumentsController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        #endregion

        #region Default Constructor

        public DocumentsController(AppDBContext _context) 
        {
            mContext = _context;
        }

        #endregion

        public IActionResult Index()
        {

            mContext.Database.EnsureCreated();

            return View();
        }

        #region Functions
        public IActionResult LoadPDF(int Id)
        {
            var theFile = mContext.Files.FindAsync(Id);
            var stream = new MemoryStream(theFile.Result.PDF);
            return new FileStreamResult(stream, "application/pdf");
        }

        public FileResult Download(int Id)
        {
            var theFile = mContext.Files.FindAsync(Id);
            var stream = new MemoryStream(theFile.Result.PDF);
            return File(stream, "application/pdf", theFile.Result.OriginalName);
        }

        #endregion
    }
}

