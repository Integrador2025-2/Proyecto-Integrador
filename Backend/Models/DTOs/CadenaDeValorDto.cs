namespace Backend.Models.DTOs;

public class CadenaDeValorDto
{
    public int CadenaDeValorId { get; set; }
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string? ObjetivoNombre { get; set; }
}

public class CreateCadenaDeValorDto
{
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
}

public class UpdateCadenaDeValorDto
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
}
