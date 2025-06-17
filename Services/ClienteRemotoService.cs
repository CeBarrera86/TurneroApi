// TurneroApi/Services/ClienteRemotoService.cs
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Interfaces;
using TurneroApi.Models.GeaPico; // For GeaCorpicoClienteDocumento and GeaCorpicoCliente
using Microsoft.Extensions.Logging;

namespace TurneroApi.Services
{
    public class ClienteRemotoService : IClienteRemotoService
    {
        private readonly GeaCorpicoDbContext _geaCorpicoContext; // Inject the NEW DbContext
        private readonly ILogger<ClienteRemotoService> _logger;

        public ClienteRemotoService(GeaCorpicoDbContext geaCorpicoContext, ILogger<ClienteRemotoService> logger)
        {
            _geaCorpicoContext = geaCorpicoContext;
            _logger = logger;
        }

        public async Task<IClienteRemotoService.ClienteRemotoDto?> ObtenerClienteGeaPico(string dni)
        {
            _logger.LogInformation("Buscando cliente {Dni} en la base de datos remota (GeaCorpico.dbo.CLIENTE_DOCUMENTO).", dni);
            try
            {
                // Reproduciendo la lógica de Laravel:
                // 1. Buscar en CLIENTE_DOCUMENTO por DNI
                // 2. Incluir los datos relacionados de CLIENTES (CLI_ID, CLI_TITULAR)
                var clienteDocumentoGea = await _geaCorpicoContext.GeaCorpicoClienteDocumentos
                                            .Include(cd => cd.ClienteGea) // Eager load the related client info
                                            .FirstOrDefaultAsync(cd => cd.CLD_NUMERO_DOCUMENTO == dni);

                if (clienteDocumentoGea != null && clienteDocumentoGea.ClienteGea != null)
                {
                    _logger.LogInformation("Cliente {Dni} encontrado en GeaCorpico. ID Remoto: {ClientId}", dni, clienteDocumentoGea.CLD_CLIENTE);
                    return new IClienteRemotoService.ClienteRemotoDto
                    {
                        Dni = clienteDocumentoGea.CLD_NUMERO_DOCUMENTO,
                        Titular = clienteDocumentoGea.ClienteGea.CLI_TITULAR // Get the titular from the related client
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar cliente {Dni} en GeaCorpico. Revise conexión y mapeo.", dni);
            }
            _logger.LogInformation("Cliente {Dni} no encontrado en GeaCorpico.", dni);
            return null;
        }
    }
}