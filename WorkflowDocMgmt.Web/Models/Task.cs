//Task.cs
namespace WorkflowDocMgmt.Web.Models
{
    public class Task
    {
        public int TaskId { get; set; }
        public int DocumentId { get; set; }
        public int WorkflowId { get; set; }
        public int AssignedAdminId { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public int StepNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
