using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCT.Models;
using Microsoft.AspNetCore.Mvc;

namespace CCT.ViewComponents
{   
    [ViewComponent]
    public class DocListViewComponent : ViewComponent
    {
        protected static AppDBContext mContext;

        public DocListViewComponent(AppDBContext _context)
        {
            mContext = _context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category, string accessLevel)
        {

            if(category == null)
            {
                if(accessLevel == null)
                {
                    // No arguments were passed -- default functionality: return all files

                    List<PdfFile> alldocs = await Task.Run(() => mContext.Files.ToList());

                    return View(alldocs);
                }
                else
                {
                    // Access Level was passed-- displays all with access level equal to given or public

                    List<PdfFile> accessableDocs = await Task.Run(() => mContext.Files.Where(
                        x => x.AccessLevel == accessLevel || x.AccessLevel == "public"
                    ).ToList());

                    return View(accessableDocs);
                }
            }
            else
            {
                if(accessLevel == null)
                {
                    // Access Level was not passed-- displays all documents with category

                    List<PdfFile> catdocs = await Task.Run(() => mContext.Files.Where(
                        x => x.Category == category).ToList());

                    return View(catdocs);
                }
                else
                {
                    // All arguments passed -- displays all documents that have the appropriate category and access level

                    List<PdfFile> docs = await Task.Run(() => mContext.Files.Where(
                        x => x.Category == category && (x.AccessLevel == accessLevel) || (x.AccessLevel == "public")
                        ).ToList());

                    return View(docs);
                }
            }
        }

    }
}