using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Text;
using MediatR;
using Backend.Queries.Actividades;
using Backend.Models.DTOs;
using Backend.Commands.Proyectos;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> UploadDocument(IFormFile file, [FromForm] int? projectId = null, [FromForm] string documentType = "project_document")
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se proporcionó ningún archivo");
            }

            // Asignar ProjectId automáticamente si no se proporciona
            if (!projectId.HasValue)
            {
                // Crear un nuevo proyecto en la base de datos
                // Usamos UsuarioId = 1 por defecto si no hay usuario autenticado (para pruebas)
                int userId = 1;
                // TODO: Obtener userId del contexto de seguridad si está disponible
                
                var createProjectCommand = new CreateProyectoCommand { UsuarioId = userId };
                var projectDto = await _mediator.Send(createProjectCommand);
                projectId = projectDto.ProyectoId;
                
                _logger.LogInformation("Se creó automáticamente el Proyecto: {ProjectId}", projectId);
            }

            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(10); // Aumentar timeout para archivos grandes
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
    public async Task<IActionResult> QueryDocuments([FromBody] RAGQueryRequestDto request)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5); // Aumentar timeout para consultas complejas
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
    public async Task<IActionResult> GenerateBudget([FromBody] RAGBudgetGenerationRequestDto request)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(10); // Aumentar timeout para generación de presupuesto

            var json = JsonSerializer.Serialize(request);
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

    [HttpPost("resources/plan")]
    public async Task<IActionResult> PlanResources([FromBody] RAGResourcePlanningRequestDto request)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            try
            {
                var response = await client.PostAsync($"{ragServiceUrl}/resources/plan", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(JsonSerializer.Deserialize<RAGResourcePlanningResponseDto>(content));
                }
                else
                {
                    _logger.LogWarning("RAG service returned error: {StatusCode}. Falling back to heuristic.", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error calling RAG service. Falling back to heuristic.");
            }

            // Heuristic Logic
            var assignments = new List<RAGResourceAssignmentDto>();
            decimal currentCost = 0;
            var availableResources = request.Recursos.Where(r => r.Disponible).ToList();

            if (!availableResources.Any())
            {
                return Ok(new RAGResourcePlanningResponseDto
                {
                    Resumen = "No hay recursos disponibles para asignar.",
                    Confianza = 0.0f,
                    CriteriosUtilizados = new List<string> { "Heurística básica: Sin recursos" }
                });
            }

            foreach (var activity in request.Actividades)
            {
                // Simple heuristic: Assign the first available resource
                var resource = availableResources.First(); 
                
                // Check budget
                if (currentCost + resource.Costo <= request.PresupuestoMaximo)
                {
                    assignments.Add(new RAGResourceAssignmentDto
                    {
                        ActividadId = activity.Id,
                        RecursoId = resource.Id,
                        NombreRecurso = resource.Nombre,
                        CostoAsignado = resource.Costo
                    });
                    currentCost += resource.Costo;
                }
            }

            var result = new RAGResourcePlanningResponseDto
            {
                Asignaciones = assignments,
                Resumen = $"Se generaron asignaciones para {assignments.Count} actividades usando heurística básica.",
                CriteriosUtilizados = new List<string> { "Heurística básica: Primer recurso disponible", "Restricción de presupuesto" },
                Confianza = 0.5f
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error planning resources");
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
                return Ok(JsonSerializer.Deserialize<RAGBudgetSuggestionsResponseDto>(content));
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

<<<<<<< HEAD
    [HttpPost("budget/save-extracted")]
    public async Task<IActionResult> SaveExtractedBudget([FromBody] SaveExtractedBudgetRequestDto request)
    {
        try
        {
            var response = new SaveExtractedBudgetResponseDto
            {
                Success = true,
                Message = "Presupuesto guardado exitosamente",
                ItemsPerRubro = new Dictionary<string, int>()
            };

            var errors = new List<string>();
            int totalCreated = 0;

            // Agrupar items por rubro
            var itemsByRubro = request.Items.GroupBy(i => i.Rubro).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var (rubro, items) in itemsByRubro)
            {
                int createdInRubro = 0;

                try
                {
                    switch (rubro)
                    {
                        case "TalentoHumano":
                            createdInRubro = await SaveTalentoHumanoItems(items, request.ActividadId);
                            break;
                        case "EquiposSoftware":
                            createdInRubro = await SaveEquiposSoftwareItems(items, request.ActividadId);
                            break;
                        case "ServiciosTecnologicos":
                            createdInRubro = await SaveServiciosTecnologicosItems(items, request.ActividadId);
                            break;
                        case "MaterialesInsumos":
                            createdInRubro = await SaveMaterialesInsumosItems(items, request.ActividadId);
                            break;
                        case "CapacitacionEventos":
                            createdInRubro = await SaveCapacitacionEventosItems(items, request.ActividadId);
                            break;
                        case "GastosViaje":
                            createdInRubro = await SaveGastosViajeItems(items, request.ActividadId);
                            break;
                        default:
                            errors.Add($"Rubro desconocido: {rubro}");
                            break;
                    }

                    response.ItemsPerRubro[rubro] = createdInRubro;
                    totalCreated += createdInRubro;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error guardando items del rubro {Rubro}", rubro);
                    errors.Add($"Error en rubro {rubro}: {ex.Message}");
                }
            }

            response.ItemsCreated = totalCreated;
            response.Errors = errors;

            if (totalCreated == 0)
            {
                response.Success = false;
                response.Message = "No se pudo guardar ningún item del presupuesto";
            }
            else if (errors.Count > 0)
            {
                response.Message = $"Presupuesto guardado parcialmente: {totalCreated} items creados con {errors.Count} errores";
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando presupuesto extraído");
            return StatusCode(500, new SaveExtractedBudgetResponseDto
            {
                Success = false,
                Message = "Error interno del servidor",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    private async Task<int> SaveTalentoHumanoItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.TalentoHumano.CreateTalentoHumanoCommand
            {
                RubroId = 1, // Asumir ID de rubro - idealmente obtenerlo dinámicamente
                CargoEspecifico = item.Nombre,
                Semanas = item.Cantidad,
                Total = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año",
                ActividadId = actividadId
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }

    private async Task<int> SaveEquiposSoftwareItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.EquiposSoftware.CreateEquiposSoftwareCommand
            {
                RubroId = 2,
                EspecificacionesTecnicas = item.EspecificacionesTecnicas ?? item.Nombre,
                Cantidad = item.Cantidad,
                Total = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año"
                // Nota: EquiposSoftware no tiene ActividadId en el modelo de dominio
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }

    private async Task<int> SaveServiciosTecnologicosItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.ServiciosTecnologicos.CreateServiciosTecnologicosCommand
            {
                RubroId = 3,
                Descripcion = item.Nombre,
                Total = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año"
                // Nota: ServiciosTecnologicos no tiene ActividadId en el modelo de dominio
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }

    private async Task<int> SaveMaterialesInsumosItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.MaterialesInsumos.CreateMaterialesInsumosCommand
            {
                RubroId = 4,
                Materiales = item.Nombre,
                Total = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año",
                ActividadId = actividadId
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }

    private async Task<int> SaveCapacitacionEventosItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.CapacitacionEventos.CreateCapacitacionEventosCommand
            {
                RubroId = 5,
                Tema = item.Nombre,
                Cantidad = item.Cantidad,
                Total = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año",
                ActividadId = actividadId
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }

    private async Task<int> SaveGastosViajeItems(List<ExtractedBudgetItemDto> items, int actividadId)
    {
        int created = 0;
        foreach (var item in items)
        {
            var command = new Backend.Commands.GastosViaje.CreateGastosViajeCommand
            {
                RubroId = 6,
                Costo = item.Total ?? 0,
                RagEstado = item.HasBudgetValues ? "extracted" : "generated",
                PeriodoNum = item.Periodo,
                PeriodoTipo = "año",
                ActividadId = actividadId
            };
            
            await _mediator.Send(command);
            created++;
        }
        return created;
    }
}

// DTOs para las peticiones
public class QueryRequest
{
    public string Question { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
    public int? TopK { get; set; } = 10; // Aumentado de 5 a 10 para más contexto
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
=======
    [HttpGet("projects/{projectId}/activities/extract")]
    public async Task<IActionResult> ExtractActivitiesFromDocuments(int projectId)
    {
        try
        {
            var ragServiceUrl = GetRAGServiceUrl();
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(10);

            var response = await client.GetAsync($"{ragServiceUrl}/projects/{projectId}/activities/extract");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(JsonSerializer.Deserialize<RAGExtractedActivitiesDto>(content));
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Error extracting activities: {Error}", errorContent);
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting activities from documents");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /* TODO: Este endpoint requiere adaptación completa a la nueva estructura de devDB
     * En devDB, los rubros se manejan mediante RecursoEspecifico con polimorfismo, no con RubroId directo.
     * Los Commands también cambiaron significativamente.
     * Se necesita reimplementar completamente para trabajar con la nueva arquitectura.
     
    [HttpPost("budget/save-extracted")]
    public async Task<IActionResult> SaveExtractedBudget([FromBody] SaveExtractedBudgetRequestDto request)
    {
        return StatusCode(501, new SaveExtractedBudgetResponseDto
        {
            Success = false,
            Message = "Este endpoint requiere adaptación a la nueva estructura de devDB",
            Errors = new List<string> { "Pendiente de implementación para RecursoEspecifico" }
        });
    }
    */
}

// DTOs para las peticiones
// QueryRequest eliminado, se usa RAGQueryRequestDto
// BudgetGenerationRequest eliminado, se usa BudgetGenerationRequestDto
>>>>>>> 31707b8c3dce0f6b25630ee459060eecc25b5e19
