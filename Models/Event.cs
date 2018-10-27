using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CCT
{
    public class Event
    {
        public int Id {get; set;}
        public DateTime start {get; set;}
        public DateTime end {get; set;}
        public string title {get; set;}
        public string description { get; set;}
    } 
}