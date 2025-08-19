//Admin.cs

using System;

namespace WorkflowDocMgmt.Web.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? AccessLevel { get; set; } // ReadWrite / ReadOnly
        public DateTime CreatedAt { get; set; }
    }
}
