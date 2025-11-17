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
}

public class RAGBudgetGenerationResponseDto
{
    public int ProjectId { get; set; }
    public Dictionary<string, object> BudgetData { get; set; } = new();
    public string? GeneratedAt { get; set; }
    public float ConfidenceScore { get; set; }
    public string? ExcelPath { get; set; }
    public int? SourceDocuments { get; set; }
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
