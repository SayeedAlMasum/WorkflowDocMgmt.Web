//DocumentType.cs
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Models
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
