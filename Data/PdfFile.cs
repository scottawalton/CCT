using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CCT
{
    public class PdfFile
    {

       public int Id {get; set;}
       public byte[] PDF {get; set;}
       public string Category {get; set;}
    }
}