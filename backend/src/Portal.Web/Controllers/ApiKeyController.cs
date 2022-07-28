﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Core.ApiKeys;
using Portal.Core.ApiKeys.Models;

namespace Portal.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Authorize(Policy = Constants.Policies.PortalIdentity)]
  [Route("api-keys")]
  public class ApiKeyController : Controller
  {
    private readonly IApiKeyService _apiKeyService;

    public ApiKeyController(IApiKeyService apiKeyService)
    {
      _apiKeyService = apiKeyService;
    }

    [HttpGet("/create-api-key")]
    public ActionResult CreateApiKey()
    {
      return View(nameof(ApiKeyEdit));
    }

    [HttpGet]
    public ActionResult ApiKeyList()
    {
      return View();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> ApiKeyEdit(Guid id, CancellationToken cancellationToken = default)
    {
      ApiKeyModel? apiKey = await _apiKeyService.GetAsync(id, cancellationToken);
      if (apiKey == null)
      {
        return NotFound();
      }

      return View(apiKey);
    }
  }
}
