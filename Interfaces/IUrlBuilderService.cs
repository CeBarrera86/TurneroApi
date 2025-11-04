using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IUrlBuilderService
{
  string ConstruirUrlMiniatura(string nombre, string tipo);
  string ConstruirUrlArchivo(string nombre);
}
