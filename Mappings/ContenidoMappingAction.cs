using AutoMapper;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Mappings;

public class ContenidoMappingAction : IMappingAction<Contenido, ContenidoDto>
{
  private readonly IUrlBuilderService _urlBuilder;

  public ContenidoMappingAction(IUrlBuilderService urlBuilder)
  {
    _urlBuilder = urlBuilder;
  }

  public void Process(Contenido source, ContenidoDto destination, ResolutionContext context)
  {
    destination.UrlMiniatura = _urlBuilder.ConstruirUrlMiniatura(source.Nombre, source.Tipo);
    destination.UrlArchivo = _urlBuilder.ConstruirUrlArchivo(source.Nombre);
  }
}