//Document.cs
using System;

namespace WorkflowDocMgmt.Web.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public string Title { get; set; }               
        public int DocumentTypeId { get; set; }      
        public string FilePath { get; set; }
        public int WorkflowId { get; set; }            
        public string CreatedBy { get; set; }        
        public string Status { get; set; }
        public int CurrentStageId { get; set; }
        public string CurrentStageName { get; set; }
        public DateTime UploadedOn { get; set; }
    }


}
