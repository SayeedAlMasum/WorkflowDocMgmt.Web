//MasterDataController.cs
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkflowDocMgmt.Web.Data;
using WorkflowDocMgmt.Web.Models;

namespace WorkflowDocMgmt.Web.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly MasterDataRepository _repo;

        public MasterDataController(MasterDataRepository repo)
        {
            _repo = repo;
        }

        // List document types
        public IActionResult DocumentTypes()
        {
            var types = _repo.GetDocumentTypes();
            return View(types); // MasterData/DocumentTypes.cshtml
        }

        // GET: Create
        public IActionResult CreateDocumentType() => View();

        // POST: Create
        [HttpPost]
        public IActionResult CreateDocumentType(DocumentType docType)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateDocumentType(docType);
                return RedirectToAction("DocumentTypes");
            }
            return View(docType);
        }
    }
}
