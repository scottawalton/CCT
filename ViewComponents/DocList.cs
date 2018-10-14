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

        public async Task<IViewComponentResult> InvokeAsync(string category)
        {

            if(category == null)
            {
                List<PdfFile> alldocs = await Task.Run(() => mContext.Files.ToList());

                return View(alldocs);
            }

            List<PdfFile> docs = await Task.Run(() => mContext.Files.Where(x => x.Category == category).ToList());

            return View(docs);
        }

    }
}
