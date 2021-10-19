using ArchiveProblems.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveProblems
{
    public static class helper
    {
        public static readonly string USERID_KEY = "userid";
        public static readonly string ADMIN_KEY = "admin";
        public static bool isCorrectUser(User user) => user != null && (user.name != null && user.password != null);
    }
}
