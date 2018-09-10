using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CCT
{
    public class User
    {
        
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string userName { get; set; }
        [MaxLength(200)]
        public string email { get; set; }
    } 
}