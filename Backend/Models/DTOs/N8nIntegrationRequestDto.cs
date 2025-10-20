using System.Collections.Generic;

namespace Backend.Models.DTOs;

public class N8nIntegrationRequestDto
{
    public List<CadenaDeValorDto> CadenaDeValor { get; set; } = new();
    public List<ActividadDto> Actividades { get; set; } = new();
}
