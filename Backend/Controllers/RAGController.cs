using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RAGController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RAGController> _logger;

    public RAGController(HttpClient httpClient, IConfiguration configuration, ILogger<RAGController> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    private string GetRAGServiceUrl()
    {
        return _configuration["RAGService:BaseUrl"] ?? "http://localhost:8001";
    }

    [HttpPost("documents/upload")]
    public async Task<IActionResult> UploadDocument(IFormFile file, int? projectId = null, string documentType = "project_document")
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se proporcionó ningún archivo");
            }

            var ragServiceUrl = GetRAGServiceUrl();
            using var formContent = new MultipartFormDataContent();
            
            formContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            formContent.Add(new StringContent(projectId?.ToString() ?? ""), "project_id");
            formContent.Add(new StringContent(documentType), "document_type");

            var response = await _httpClient.PostAsync($"{ragServiceUrl}/documents/upload", formContent);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(content));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error uploading document to RAG service: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("query")]
    public async Task<IActionResult> QueryDocuments([FromBody] QueryRequest request)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{ragServiceUrl}/query", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(responseContent));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error querying RAG service: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying documents");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpPost("budget/generate")]
    public async Task<IActionResult> GenerateBudget([FromBody] BudgetGenerationRequest request)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{ragServiceUrl}/budget/generate", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(responseContent));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error generating budget: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating budget");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("projects/{projectId}/documents")]
    public async Task<IActionResult> GetProjectDocuments(int projectId)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var response = await _httpClient.GetAsync($"{ragServiceUrl}/projects/{projectId}/documents");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(content));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error getting project documents: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project documents");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpGet("projects/{projectId}/budget/suggestions")]
    public async Task<IActionResult> GetBudgetSuggestions(int projectId, string? category = null)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var url = $"{ragServiceUrl}/projects/{projectId}/budget/suggestions";
            if (!string.IsNullOrEmpty(category))
            {
                url += $"?category={category}";
            }

            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(content));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error getting budget suggestions: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting budget suggestions");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    [HttpDelete("documents/{documentId}")]
    public async Task<IActionResult> DeleteDocument(string documentId)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var response = await _httpClient.DeleteAsync($"{ragServiceUrl}/documents/{documentId}");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<object>(content));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error deleting document: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}

// DTOs para las peticiones
public class QueryRequest
{
    public string Question { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
    public int? TopK { get; set; } = 5;
}

public class BudgetGenerationRequest
{
    public int ProjectId { get; set; }
    public string ProjectDescription { get; set; } = string.Empty;
    public List<string> BudgetCategories { get; set; } = new List<string>
    {
        "TalentoHumano",
        "ServiciosTecnologicos",
        "EquiposSoftware",
        "MaterialesInsumos",
        "CapacitacionEventos",
        "GastosViaje"
    };
    public int DurationYears { get; set; } = 1;
}
