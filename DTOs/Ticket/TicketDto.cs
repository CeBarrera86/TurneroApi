namespace TurneroApi.DTOs.Ticket;

using System;
using TurneroApi.DTOs.Cliente;
using TurneroApi.DTOs.Estado;
using TurneroApi.DTOs.Sector;

public class TicketDto
{
  public ulong Id { get; set; }
  public string Letra { get; set; } = null!;
  public int Numero { get; set; }
  public ulong ClienteId { get; set; }
  public DateTime Fecha { get; set; }
  public int SectorIdOrigen { get; set; }
  public int? SectorIdActual { get; set; }
  public int EstadoId { get; set; }
  public DateTime? Actualizado { get; set; }

  public ClienteDto ClienteNavigation { get; set; } = null!;
  public SectorDto SectorIdOrigenNavigation { get; set; } = null!;
  public SectorDto? SectorIdActualNavigation { get; set; }
  public EstadoDto EstadoNavigation { get; set; } = null!;
}