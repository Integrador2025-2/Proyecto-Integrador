using Microsoft.AspNetCore.Mvc;
using Backend.Models.DTOs;
using System.Text.Json;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RAGController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RAGController> _logger;
    private readonly string _ragServiceBaseUrl;

    public RAGController(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        ILogger<RAGController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _ragServiceBaseUrl = _configuration["RAGService:BaseUrl"] ?? "http://localhost:8001";
    }

    /// <summary>
    /// Verifica el estado del servicio RAG
    /// </summary>
    [HttpGet("health")]
    public async Task<ActionResult<RAGHealthResponseDto>> GetHealth()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_ragServiceBaseUrl}/health");

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, new { error = "RAG service is not available" });
            }

            var content = await response.Content.ReadAsStringAsync();
            var healthData = JsonSerializer.Deserialize<RAGHealthResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(healthData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking RAG service health");
            return StatusCode(500, new { error = "Error connecting to RAG service", details = ex.Message });
        }
    }

    /// <summary>
    /// Sube un documento al servicio RAG
    /// </summary>
    [HttpPost("documents/upload")]
    public async Task<ActionResult<RAGDocumentUploadResponseDto>> UploadDocument(
        IFormFile file,
        [FromForm] int? projectId = null,
        [FromForm] string documentType = "project_document")
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file provided" });
            }

            var client = _httpClientFactory.CreateClient();
            
            using var formData = new MultipartFormDataContent();
            using var fileStream = file.OpenReadStream();
            using var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            formData.Add(fileContent, "file", file.FileName);
            
            if (projectId.HasValue)
            {
                formData.Add(new StringContent(projectId.Value.ToString()), "project_id");
            }
            formData.Add(new StringContent(documentType), "document_type");

            var response = await client.PostAsync($"{_ragServiceBaseUrl}/documents/upload", formData);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error uploading document", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RAGDocumentUploadResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading document to RAG service");
            return StatusCode(500, new { error = "Error uploading document", details = ex.Message });
        }
    }

    /// <summary>
    /// Realiza una consulta semántica sobre los documentos
    /// </summary>
    [HttpPost("query")]
    public async Task<ActionResult<RAGQueryResponseDto>> Query([FromBody] RAGQueryRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { error = "Question is required" });
            }

            var client = _httpClientFactory.CreateClient();
            var jsonContent = JsonContent.Create(request);
            
            var response = await client.PostAsync($"{_ragServiceBaseUrl}/query", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error querying documents", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RAGQueryResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying RAG service");
            return StatusCode(500, new { error = "Error querying documents", details = ex.Message });
        }
    }

    /// <summary>
    /// Genera un presupuesto automáticamente basado en documentos del proyecto
    /// </summary>
    [HttpPost("budget/generate")]
    public async Task<ActionResult<RAGBudgetGenerationResponseDto>> GenerateBudget([FromBody] RAGBudgetGenerationRequestDto request)
    {
        try
        {
            if (request.ProjectId <= 0)
            {
                return BadRequest(new { error = "Valid ProjectId is required" });
            }

            if (string.IsNullOrWhiteSpace(request.ProjectDescription))
            {
                return BadRequest(new { error = "ProjectDescription is required" });
            }

            var client = _httpClientFactory.CreateClient();
            var jsonContent = JsonContent.Create(request);
            
            var response = await client.PostAsync($"{_ragServiceBaseUrl}/budget/generate", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error generating budget", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RAGBudgetGenerationResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating budget from RAG service");
            return StatusCode(500, new { error = "Error generating budget", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene todos los documentos asociados a un proyecto
    /// </summary>
    [HttpGet("projects/{projectId}/documents")]
    public async Task<ActionResult<RAGProjectDocumentsResponseDto>> GetProjectDocuments(int projectId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_ragServiceBaseUrl}/projects/{projectId}/documents");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error getting project documents", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RAGProjectDocumentsResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project documents from RAG service");
            return StatusCode(500, new { error = "Error getting project documents", details = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un documento del servicio RAG
    /// </summary>
    [HttpDelete("documents/{documentId}")]
    public async Task<IActionResult> DeleteDocument(string documentId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                return BadRequest(new { error = "DocumentId is required" });
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{_ragServiceBaseUrl}/documents/{documentId}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error deleting document", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(JsonSerializer.Deserialize<object>(content));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting document from RAG service");
            return StatusCode(500, new { error = "Error deleting document", details = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene sugerencias de presupuesto para un proyecto específico
    /// </summary>
    [HttpGet("projects/{projectId}/budget/suggestions")]
    public async Task<ActionResult<RAGBudgetSuggestionsResponseDto>> GetBudgetSuggestions(
        int projectId, 
        [FromQuery] string? category = null)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var url = $"{_ragServiceBaseUrl}/projects/{projectId}/budget/suggestions";
            if (!string.IsNullOrWhiteSpace(category))
            {
                url += $"?category={Uri.EscapeDataString(category)}";
            }

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("RAG service returned error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                return StatusCode((int)response.StatusCode, new { error = "Error getting budget suggestions", details = errorContent });
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RAGBudgetSuggestionsResponseDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting budget suggestions from RAG service");
            return StatusCode(500, new { error = "Error getting budget suggestions", details = ex.Message });
        }
    }
}
