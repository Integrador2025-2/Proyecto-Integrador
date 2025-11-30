namespace Backend.Models.DTOs;

public class RAGQueryRequestDto
{
    public string Question { get; set; } = string.Empty;
    public int? ProjectId { get; set; }
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
    public int? ActividadId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Justificacion { get; set; }
    public string? EspecificacionesTecnicas { get; set; }
    public int? CantidadAnios { get; set; } = 1;
    public decimal? ValorUnitario { get; set; }
    public int? DuracionDias { get; set; }
}

public class RAGBudgetGenerationRequestDto
{
    public int ProjectId { get; set; }
    public string ProjectDescription { get; set; } = string.Empty;
    public List<string> BudgetCategories { get; set; } = new()
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
    public string Category { get; set; } = string.Empty;
    public List<Dictionary<string, object>> SuggestedItems { get; set; } = new();
    public string Reasoning { get; set; } = string.Empty;
    public float Confidence { get; set; }
}

public class RAGBudgetSuggestionsResponseDto
{
    public int ProjectId { get; set; }
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
    public int? Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public double? CostoUnitario { get; set; }
    public string? Unidad { get; set; }
    public double? Disponibilidad { get; set; }
}

public class RAGResourceAssignmentDto
{
    public int? ActividadId { get; set; }
    public string ActividadNombre { get; set; } = string.Empty;
    public int? RecursoId { get; set; }
    public string RecursoNombre { get; set; } = string.Empty;
    public string RecursoTipo { get; set; } = string.Empty;
    public double Cantidad { get; set; }
    public double? CostoEstimado { get; set; }
    public string? Justificacion { get; set; }
}

public class RAGResourcePlanRequestDto
{
    public int? ProjectId { get; set; }
    public List<RAGActivityDto> Actividades { get; set; } = new();
    public List<RAGResourceDto> Recursos { get; set; } = new();
    public string? Objetivo { get; set; }
    public double? PresupuestoMaximo { get; set; }
}

public class RAGResourcePlanResponseDto
{
    public int? ProjectId { get; set; }
    public List<RAGResourceAssignmentDto> Asignaciones { get; set; } = new();
    public string Resumen { get; set; } = string.Empty;
    public List<string> CriteriosUtilizados { get; set; } = new();
    public float Confianza { get; set; }
}
