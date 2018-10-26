using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCT
{
    public class Event
    {
        public string Id {get; set;}
        public string start {get; set;}
        public string end {get; set;}
        public string title {get; set;}
        public string description { get; set;}
    } 
}