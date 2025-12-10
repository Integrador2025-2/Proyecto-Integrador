using System.Text.Json.Serialization;

namespace Backend.Models.DTOs;

public class RAGQueryRequestDto
{
    [JsonPropertyName("question")]
    public string Question { get; set; } = string.Empty;
    
    [JsonPropertyName("project_id")]
    public int? ProjectId { get; set; }
    
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; } = 5;
}

public class RAGQueryResponseDto
{
    public string Answer { get; set; } = string.Empty;
    public List<RAGSourceDto> Sources { get; set; } = new();
    public float Confidence { get; set; }
}

public class RAGSourceDto
{
    public string Content { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
    public float Similarity { get; set; }
}

public class RAGDocumentUploadRequestDto
{
    public int? ProjectId { get; set; }
    public string DocumentType { get; set; } = "project_document";
}

public class RAGDocumentUploadResponseDto
{
    public string Message { get; set; } = string.Empty;
    public string DocumentId { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
}

public class RAGActivityForBudgetDto
{
    [JsonPropertyName("actividad_id")]
    public int? ActividadId { get; set; }
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
    
    [JsonPropertyName("justificacion")]
    public string? Justificacion { get; set; }
    
    [JsonPropertyName("especificaciones_tecnicas")]
    public string? EspecificacionesTecnicas { get; set; }
    
    [JsonPropertyName("cantidad_anios")]
    public int? CantidadAnios { get; set; } = 1;
    
    [JsonPropertyName("valor_unitario")]
    public decimal? ValorUnitario { get; set; }
    
    [JsonPropertyName("duracion_dias")]
    public int? DuracionDias { get; set; }
}

public class RAGBudgetGenerationRequestDto
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }
    
    [JsonPropertyName("project_description")]
    public string ProjectDescription { get; set; } = string.Empty;
    
    [JsonPropertyName("budget_categories")]
    public List<string> BudgetCategories { get; set; } = new()
    {
        "TalentoHumano",
        "ServiciosTecnologicos",
        "EquiposSoftware",
        "MaterialesInsumos",
        "CapacitacionEventos",
        "GastosViaje"
    };
    
    [JsonPropertyName("duration_years")]
    public int DurationYears { get; set; } = 1;
    
    [JsonPropertyName("activities")]
    public List<RAGActivityForBudgetDto>? Activities { get; set; } // Lista opcional de actividades
}

public class RAGBudgetGenerationResponseDto
{
    public int ProjectId { get; set; }
    public Dictionary<string, object> BudgetData { get; set; } = new();
    public string? GeneratedAt { get; set; }
    public float ConfidenceScore { get; set; }
    public string? ExcelPath { get; set; }
    public int? SourceDocuments { get; set; }
    public int? SourceActivities { get; set; }
    public string? Method { get; set; } // "llm_based" o "document_based"
}

public class RAGProjectDocumentDto
{
    public string DocumentId { get; set; } = string.Empty;
    public string Filename { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public int ProjectId { get; set; }
    public string UploadDate { get; set; } = string.Empty;
    public string ContentPreview { get; set; } = string.Empty;
}

public class RAGProjectDocumentsResponseDto
{
    public int ProjectId { get; set; }
    public List<RAGProjectDocumentDto> Documents { get; set; } = new();
}

public class RAGBudgetSuggestionDto
{
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
    
    [JsonPropertyName("suggested_items")]
    public List<Dictionary<string, object>> SuggestedItems { get; set; } = new();
    
    [JsonPropertyName("reasoning")]
    public string Reasoning { get; set; } = string.Empty;
    
    [JsonPropertyName("confidence")]
    public float Confidence { get; set; }
}

public class RAGBudgetSuggestionsResponseDto
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }
    
    [JsonPropertyName("suggestions")]
    public List<RAGBudgetSuggestionDto> Suggestions { get; set; } = new();
}

public class RAGHealthResponseDto
{
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class RAGActivityDto
{
    public int? Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? DuracionDias { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public List<int> DependenciaIds { get; set; } = new();
}

public class RAGResourceDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [JsonPropertyName("costo")]
    public decimal Costo { get; set; }
    
    [JsonPropertyName("tipo")]
    public string? Tipo { get; set; }
    
    [JsonPropertyName("disponible")]
    public bool Disponible { get; set; } = true;
}

public class RAGResourceAssignmentDto
{
    [JsonPropertyName("actividad_id")]
    public int ActividadId { get; set; }

    [JsonPropertyName("recurso_id")]
    public int RecursoId { get; set; }
    
    [JsonPropertyName("nombre_recurso")]
    public string NombreRecurso { get; set; } = string.Empty;

    [JsonPropertyName("costo_asignado")]
    public decimal CostoAsignado { get; set; }
}

public class RAGActivityForResourceDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
}

public class RAGResourcePlanningRequestDto
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("actividades")]
    public List<RAGActivityForResourceDto> Actividades { get; set; } = new();

    [JsonPropertyName("recursos")]
    public List<RAGResourceDto> Recursos { get; set; } = new();

    [JsonPropertyName("presupuesto_maximo")]
    public decimal PresupuestoMaximo { get; set; }
}

public class RAGResourcePlanningResponseDto
{
    [JsonPropertyName("asignaciones")]
    public List<RAGResourceAssignmentDto> Asignaciones { get; set; } = new();

    [JsonPropertyName("resumen")]
    public string Resumen { get; set; } = string.Empty;

    [JsonPropertyName("criterios_utilizados")]
    public List<string> CriteriosUtilizados { get; set; } = new();

    [JsonPropertyName("confianza")]
    public float Confianza { get; set; }
}

// DTOs para extracción y guardado de presupuesto
public class ExtractedBudgetItemDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Rubro { get; set; } = string.Empty; // TalentoHumano, EquiposSoftware, etc.
    public int Cantidad { get; set; } = 1;
    public decimal? ValorUnitario { get; set; }
    public decimal? Total { get; set; }
    public string? Justificacion { get; set; }
    public string? EspecificacionesTecnicas { get; set; }
    public int Periodo { get; set; } = 1;
    public string? SourceSheet { get; set; }
    public bool HasBudgetValues { get; set; }
}

public class SaveExtractedBudgetRequestDto
{
    public int ProjectId { get; set; }
    public int ActividadId { get; set; }  // ID de la actividad a la que pertenece este presupuesto
    public List<ExtractedBudgetItemDto> Items { get; set; } = new();
    public string ExtractionMethod { get; set; } = "intelligent_extraction";
}

public class SaveExtractedBudgetResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ItemsCreated { get; set; }
    public Dictionary<string, int> ItemsPerRubro { get; set; } = new();
    public List<string> Errors { get; set; } = new();
}

public class RAGExtractedActivitiesDto
{
    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; }

    [JsonPropertyName("activities")]
    public List<RAGExtractedActivityDto> Activities { get; set; } = new();

    [JsonPropertyName("total_activities")]
    public int TotalActivities { get; set; }
}

public class RAGExtractedActivityDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("justification")]
    public string? Justification { get; set; }

    [JsonPropertyName("technical_specifications")]
    public string? TechnicalSpecifications { get; set; }

    [JsonPropertyName("duration_years")]
    public int? DurationYears { get; set; }

    [JsonPropertyName("unit_value")]
    public decimal? UnitValue { get; set; }
}
