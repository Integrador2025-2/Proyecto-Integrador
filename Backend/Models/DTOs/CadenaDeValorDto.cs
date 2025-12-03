namespace Backend.Models.DTOs;

public class CadenaDeValorDto
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
}

public class CreateCadenaDeValorDto
{
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
}

public class UpdateCadenaDeValorDto
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
}
