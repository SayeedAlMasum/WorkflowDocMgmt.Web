//DocumentWorkflow.cs
using System;

namespace WorkflowDocMgmt.Web.Models
{
    public class DocumentWorkflow
    {
        public int DocumentWorkflowId { get; set; }   
        public int DocumentId { get; set; }          
        public int StageId { get; set; }              
        public string StageName { get; set; }        
        public string Status { get; set; }          
        public DateTime ChangedAt { get; set; }      
        public string ChangedBy { get; set; }         
    }
}
