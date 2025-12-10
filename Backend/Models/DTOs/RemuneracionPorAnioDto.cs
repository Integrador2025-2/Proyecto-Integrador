namespace Backend.Models.DTOs;

public class RemuneracionPorAnioDto
{
    public int RemuneracionPorAnioId { get; set; }
    public int TalentoHumanoId { get; set; }
    public int Anio { get; set; }
    public decimal Honorarios { get; set; }
    public decimal ValorHora { get; set; }
    public int SemanasAnio { get; set; }
    public decimal TotalAnio { get; set; }
}

public class CreateRemuneracionPorAnioDto
{
    public int TalentoHumanoId { get; set; }
    public int Anio { get; set; }
    public decimal Honorarios { get; set; }
    public decimal ValorHora { get; set; }
    public int SemanasAnio { get; set; }
    public decimal TotalAnio { get; set; }
}

public class UpdateRemuneracionPorAnioDto
{
    public int RemuneracionPorAnioId { get; set; }
    public int TalentoHumanoId { get; set; }
    public int Anio { get; set; }
    public decimal Honorarios { get; set; }
    public decimal ValorHora { get; set; }
    public int SemanasAnio { get; set; }
    public decimal TotalAnio { get; set; }
}

