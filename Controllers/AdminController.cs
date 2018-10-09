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
    // Makes entire area private to only Admins
        [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;
        protected UserManager<User> userManager;

        public class Document : PdfFile
        {
            public IFormFile file {get; set;}
        }

        #endregion

        #region Default Constructor

        public AdminController(AppDBContext _context,
                                UserManager<User> _userManager) 
        {
            userManager = _userManager;
            mContext = _context;
        }

        #endregion

        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }

        #region Manage Documents
        public IActionResult ManageDocuments()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ManageDocuments(Document doc)
        {

            PdfFile File = new PdfFile
            {
            Category = doc.Category
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

        public async Task UploadDocument(PdfFile pdf)
        {
            await mContext.AddAsync<PdfFile>(pdf);
            await mContext.SaveChangesAsync();
        }

        public IActionResult LoadPDF(int fileID)
        {
            var theFile = mContext.Files.FindAsync(fileID);
            var stream = new MemoryStream(theFile.Result.PDF);
            return new FileStreamResult(stream, "application/pdf");
        }
        #endregion

        public IActionResult ManageUsers()
        {
            return View();
        }
    }
}
