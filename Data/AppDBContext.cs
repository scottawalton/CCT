using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CCT.Models;

namespace CCT
{
    public class AppDBContext : IdentityDbContext<User>
    {
        // default Database constructor - expecting options
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<PdfFile> Files { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}