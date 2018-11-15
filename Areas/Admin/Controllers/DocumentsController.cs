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

namespace CCT.Admin
{
    // Makes entire area private to only Admins

    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class DocumentsController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        public class Document : PdfFile
        {
            public IFormFile file {get; set;}
        }

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


        #region Upload Documents
        public IActionResult Upload()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(Document doc)
        {

            PdfFile File = new PdfFile
            {
            Category = doc.Category,
            OriginalName = doc.file.FileName,
            Name = doc.Name,
            AccessLevel = doc.AccessLevel
            };

            if (ModelState.IsValid)
            {

                using (var mem = new MemoryStream())
                {
                    await doc.file.CopyToAsync(mem);
                    File.PDF = mem.ToArray();
                }

                await UploadDocument(File);

                ViewBag.result = "Upload successful!";

            }

            return View();
        }

        #endregion

        #region Functions
        public async Task UploadDocument(PdfFile pdf)
        {
            await mContext.AddAsync<PdfFile>(pdf);
            await mContext.SaveChangesAsync();
        }

        public IActionResult DeleteDocument(int Id)
        {
            var theFile = mContext.Files.Find(Id);
            var result = mContext.Files.Remove(theFile);
            return RedirectToAction("Index");
        }

        #endregion
    }
}

