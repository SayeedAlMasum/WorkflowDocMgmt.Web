//MasterDataController.cs
using Microsoft.AspNetCore.Mvc;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly MasterDataRepository _repo;

        public MasterDataController(MasterDataRepository repo) => _repo = repo;

        public IActionResult DocumentTypes()
        {
            var types = _repo.GetDocumentTypes();
            return View(types);
        }

        public IActionResult CreateDocumentType() => View();

        [HttpPost]
        public IActionResult CreateDocumentType(DocumentType docType)
        {
            _repo.CreateDocumentType(docType);
            return RedirectToAction("DocumentTypes");
        }
    }
}
