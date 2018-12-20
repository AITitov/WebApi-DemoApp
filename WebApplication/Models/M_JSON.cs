using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class M_JSON
    {
        public bool success { get; set; }
        public int total { get; set; }
        public object data { get; set; }
        public string message { get; set; }
    }
}