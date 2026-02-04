using Microsoft.Extensions.DependencyInjection;
using TurneroApi.Mappings;
using TurneroApi.Interfaces;
using TurneroApi.Services;

namespace TurneroApi.Config
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddTurneroServices(this IServiceCollection services)
    {
      services.AddScoped<ContenidoMappingAction>();
      services.AddScoped<IArchivoService, ArchivoService>();
      services.AddScoped<IClienteService, ClienteService>();
      services.AddScoped<IClienteRemotoService, ClienteRemotoService>();
      services.AddScoped<IContenidoService, ContenidoService>();
      services.AddScoped<IEstadoService, EstadoService>();
      services.AddScoped<IHistorialService, HistorialService>();
      services.AddScoped<IMiniaturaService, MiniaturaService>();
      services.AddScoped<IMostradorSectorService, MostradorSectorService>();
      services.AddScoped<IMostradorService, MostradorService>();
      services.AddScoped<IPuestoService, PuestoService>();
      services.AddScoped<IRolService, RolService>();
      services.AddScoped<IRolPermisoService, RolPermisoService>();
      services.AddScoped<IPermisoService, PermisoService>();
      services.AddScoped<ISectorService, SectorService>();
      services.AddScoped<ITicketService, TicketService>();
      services.AddScoped<ITurnoService, TurnoService>();
      services.AddScoped<IUrlBuilderService, UrlBuilderService>();
      services.AddScoped<IUsuarioService, UsuarioService>();

      return services;
    }
  }
}
