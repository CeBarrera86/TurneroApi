namespace TurneroApi.Interfaces
{
    public interface IClienteRemotoService
    {
        public class ClienteRemotoDto
        {
            public string Dni { get; set; } = null!;
            public string? Titular { get; set; }
        }

        Task<ClienteRemotoDto?> ObtenerClienteGeaPico(string dni);
    }
}