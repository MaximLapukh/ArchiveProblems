using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems.Models
{
    public class User
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public List<Problem> solvedProblems { get; set; }

    }
}
