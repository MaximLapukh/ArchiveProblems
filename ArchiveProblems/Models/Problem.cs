using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string answer { get; set; }
        public int countSolved { get; set; }
    }
}
