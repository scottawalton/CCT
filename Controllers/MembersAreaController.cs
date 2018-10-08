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
    // Makes entire area private
    [Authorize]
    public class MembersAreaController : Controller
    {
        #region Protected Members
        protected AppDBContext mContext;

        public class Document : PdfFile
        {
            public IFormFile file {get; set;}
        }

        #endregion

        #region Default Constructor

        public MembersAreaController(AppDBContext context) 
        {
            mContext = context;
        }

        #endregion

        public IActionResult Index()
        {
            mContext.Database.EnsureCreated();
            return View();
        }

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


                return YourPDF(File);
            }



            return View();

        }

        public async Task UploadDocument(PdfFile pdf)
        {
            await mContext.AddAsync<PdfFile>(pdf);
            await mContext.SaveChangesAsync();
        }

        public IActionResult YourPDF(PdfFile file)
        {
            var theFile = mContext.Files.FindAsync(file.Id);
            var stream = new MemoryStream(theFile.Result.PDF);
            return new FileStreamResult(stream, "application/pdf");
        }
    }
}
