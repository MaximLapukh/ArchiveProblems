﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Models
{
    public class News
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
    }
}
