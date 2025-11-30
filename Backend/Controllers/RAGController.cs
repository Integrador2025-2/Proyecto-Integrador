using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using MediatR;
using Backend.Queries.Actividades;
using Backend.Models.DTOs;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RAGController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RAGController> _logger;
    private readonly IMediator _mediator;

    public RAGController(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration, 
        ILogger<RAGController> logger,
        IMediator mediator)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _mediator = mediator;
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
            var client = _httpClientFactory.CreateClient();
            using var formContent = new MultipartFormDataContent();
            
            formContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
            formContent.Add(new StringContent(projectId?.ToString() ?? ""), "project_id");
            formContent.Add(new StringContent(documentType), "document_type");

            var response = await client.PostAsync($"{ragServiceUrl}/documents/upload", formContent);
            
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
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{ragServiceUrl}/query", content);
            
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
            // Si no se proporcionan actividades pero hay un projectId, intentar obtenerlas de la BD
            if (request.Activities == null || request.Activities.Count == 0)
            {
                try
                {
                    var actividades = await _mediator.Send(new GetActividadesByProyectoQuery(request.ProjectId));
                    if (actividades != null && actividades.Count > 0)
                    {
                        // Convertir ActividadDto a RAGActivityForBudgetDto
                        request.Activities = actividades.Select(a => new RAGActivityForBudgetDto
                        {
                            ActividadId = a.ActividadId,
                            Nombre = a.Nombre,
                            Descripcion = a.Descripcion,
                            Justificacion = a.Justificacion,
                            EspecificacionesTecnicas = a.EspecificacionesTecnicas,
                            CantidadAnios = a.CantidadAnios,
                            ValorUnitario = a.ValorUnitario,
                            DuracionDias = null // ActividadDto no tiene duracion_dias directamente
                        }).ToList();
                        
                        _logger.LogInformation("Se obtuvieron {Count} actividades del proyecto {ProjectId} para generación de presupuesto", 
                            request.Activities.Count, request.ProjectId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se pudieron obtener actividades del proyecto {ProjectId}, continuando sin actividades", 
                        request.ProjectId);
                }
            }

            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            
            // Crear request DTO para el servicio RAG
            var ragRequest = new RAGBudgetGenerationRequestDto
            {
                ProjectId = request.ProjectId,
                ProjectDescription = request.ProjectDescription,
                BudgetCategories = request.BudgetCategories,
                DurationYears = request.DurationYears,
                Activities = request.Activities
            };
            
            var json = JsonSerializer.Serialize(ragRequest, new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{ragServiceUrl}/budget/generate", content);
            
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
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{ragServiceUrl}/projects/{projectId}/documents");
            
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
            var client = _httpClientFactory.CreateClient();
            var url = $"{ragServiceUrl}/projects/{projectId}/budget/suggestions";
            if (!string.IsNullOrEmpty(category))
            {
                url += $"?category={category}";
            }

            var response = await client.GetAsync(url);
            
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
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"{ragServiceUrl}/documents/{documentId}");
            
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
    public List<RAGActivityForBudgetDto>? Activities { get; set; } // Lista opcional de actividades
}
