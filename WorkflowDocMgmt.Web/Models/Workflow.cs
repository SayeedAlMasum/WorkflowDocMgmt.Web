//Workflow.cs
using System;
using System.Collections.Generic;

namespace WorkflowDocMgmt.Web.Models
{
    public class Workflow
    {
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public WorkflowType Type { get; set; }
        public List<int> AssignedAdminIds { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
    }
}
