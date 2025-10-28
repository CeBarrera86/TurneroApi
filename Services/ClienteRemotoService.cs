using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Interfaces;

namespace TurneroApi.Services
{
    public class ClienteRemotoService : IClienteRemotoService
    {
        private readonly GeaCorpicoDbContext _geaCorpicoContext;
        private readonly ILogger<ClienteRemotoService> _logger;

        public ClienteRemotoService(GeaCorpicoDbContext geaCorpicoContext, ILogger<ClienteRemotoService> logger)
        {
            _geaCorpicoContext = geaCorpicoContext;
            _logger = logger;
        }

        public async Task<IClienteRemotoService.ClienteRemotoDto?> ObtenerClienteGeaPico(string dni)
        {
            // Tipos de documento válidos para la búsqueda DNI (1), CI (3), LE (4), LC (5)    
            try
            {
                var clienteDocumentoGea = await _geaCorpicoContext.GeaCorpicoClienteDocumentos.Include(cd => cd.ClienteGea)
                    .FirstOrDefaultAsync(cd =>
                        cd.CLD_NUMERO_DOCUMENTO == dni &&
                        cd.CLD_EMPRESA == 1 &&
                        (cd.CLD_TIPO_DOCUMENTO == 1 ||
                            cd.CLD_TIPO_DOCUMENTO == 3 ||
                            cd.CLD_TIPO_DOCUMENTO == 4 ||
                            cd.CLD_TIPO_DOCUMENTO == 5)
                    );

                if (clienteDocumentoGea != null && clienteDocumentoGea.ClienteGea != null)
                {
                    return new IClienteRemotoService.ClienteRemotoDto
                    {
                        Dni = clienteDocumentoGea.CLD_NUMERO_DOCUMENTO,
                        Titular = clienteDocumentoGea.ClienteGea.CLI_TITULAR
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR al buscar cliente {Dni} en GeaCorpico. Detalles: {Message}", dni, ex.Message);
            }

            return null;
        }
    }
}