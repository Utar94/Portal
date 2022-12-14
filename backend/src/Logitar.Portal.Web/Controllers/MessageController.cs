using Logitar.Portal.Application.Emails.Messages;
using Logitar.Portal.Core.Emails.Messages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logitar.Portal.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.PortalIdentity)]
  [Route("messages")]
  public class MessageController : Controller
  {
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
      _messageService = messageService;
    }

    [HttpGet]
    public ActionResult MessageList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> MessageView(Guid id, CancellationToken cancellationToken = default)
    {
      MessageModel? message = await _messageService.GetAsync(id, cancellationToken);
      if (message == null)
      {
        return NotFound();
      }

      return View(message);
    }
  }
}
