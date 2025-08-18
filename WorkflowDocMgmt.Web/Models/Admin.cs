//Admin.cs
using System;

namespace WorkflowDocMgmt.Web.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // store hashed password in production
        public AccessLevel AccessLevel { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; } // AdminId of creator
    }
}
