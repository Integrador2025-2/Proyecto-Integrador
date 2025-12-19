namespace Backend.Models.DTOs;

public class RubroDto
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class CreateRubroDto
{
    public string Descripcion { get; set; } = string.Empty;
}

public class UpdateRubroDto
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

