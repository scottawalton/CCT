using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CCT.Models
{
    public class PdfFile
    {
       public int Id {get; set;}
       public byte[] PDF {get; set;}
       public string Category {get; set;}
       public string OriginalName {get; set;}
       public string Name {get; set;}

    }
}