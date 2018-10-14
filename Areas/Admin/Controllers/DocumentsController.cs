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

        [HttpPost]
        public async Task<IActionResult> Index(Document doc)
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

                // TODO: Display success message.

            }

            return View();
        }

        #region Functions
        public async Task UploadDocument(PdfFile pdf)
        {
            await mContext.AddAsync<PdfFile>(pdf);
            await mContext.SaveChangesAsync();
        }

        // TODO: Implement access levels and ability to edit them

        #endregion
    }
}

