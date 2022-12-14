using Logitar.Portal.Application.Emails.Templates;
using Logitar.Portal.Core.Emails.Templates.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Portal.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.PortalIdentity)]
  [Route("templates")]
  public class TemplateController : Controller
  {
    private readonly ITemplateService _templateService;

    public TemplateController(ITemplateService templateService)
    {
      _templateService = templateService;
    }

    [HttpGet("/create-template")]
    public ActionResult CreateTemplate()
    {
      return View(nameof(TemplateEdit));
    }

    [HttpGet]
    public ActionResult TemplateList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> TemplateEdit(Guid id, CancellationToken cancellationToken = default)
    {
      TemplateModel? template = await _templateService.GetAsync(id, cancellationToken);
      if (template == null)
      {
        return NotFound();
      }

      return View(template);
    }
  }
}
