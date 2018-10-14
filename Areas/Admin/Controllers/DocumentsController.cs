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

        public DocumentsController(AppDBContext _context,
                                UserManager<User> _userManager) 
        {
            mContext = _context;
        }

        #endregion

        public IActionResult Index()
        {

            mContext.Database.EnsureCreated();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Document doc)
        {

            PdfFile File = new PdfFile
            {
            Category = doc.Category,
            OriginalName = doc.file.FileName,
            Name = doc.file.Name
            };

            if (ModelState.IsValid)
            {

                using (var mem = new MemoryStream())
                {
                    await doc.file.CopyToAsync(mem);
                    File.PDF = mem.ToArray();
                }

                await UploadDocument(File);


                return LoadPDF(File.Id);
            }

            return View();
        }


        #region Functions
        public async Task UploadDocument(PdfFile pdf)
        {
            await mContext.AddAsync<PdfFile>(pdf);
            await mContext.SaveChangesAsync();

        }

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

