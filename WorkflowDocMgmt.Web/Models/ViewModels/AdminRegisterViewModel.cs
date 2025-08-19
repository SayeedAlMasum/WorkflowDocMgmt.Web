//AdminRegisterViewModel
namespace WorkflowDocMgmt.Web.Models.ViewModels
{
    public class AdminRegisterViewModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AccessLevel { get; set; } = "ReadOnly"; // Default
    }
}
