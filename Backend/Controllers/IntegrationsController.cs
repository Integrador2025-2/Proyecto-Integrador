using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IntegrationsController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public IntegrationsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost("n8n")]
    public async Task<IActionResult> SendToN8n([FromBody] N8nIntegrationRequestDto payload)
    {
        var webhookUrl = _configuration["N8N:WebhookUrl"];
        if (string.IsNullOrWhiteSpace(webhookUrl))
            return BadRequest(new { error = "N8N webhook URL not configured (N8N:WebhookUrl)" });

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync(webhookUrl, payload);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, new { error = "n8n returned error", details = content });
        }

        var result = await response.Content.ReadAsStringAsync();
        return Ok(new { status = "sent", response = result });
    }
}
