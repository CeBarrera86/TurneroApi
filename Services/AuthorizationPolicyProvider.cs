using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using TurneroApi.Data;

namespace TurneroApi.Services;

public class DynamicAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
  private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
  private readonly IServiceScopeFactory _scopeFactory;

  public DynamicAuthorizationPolicyProvider(
      IOptions<AuthorizationOptions> options,
      IServiceScopeFactory scopeFactory)
  {
    _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    _scopeFactory = scopeFactory;
  }

  // La interfaz exige Task<AuthorizationPolicy> (no nullable)
  public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
      => _fallbackPolicyProvider.GetDefaultPolicyAsync();

  // Este sí puede ser nullable según la interfaz
  public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
      => _fallbackPolicyProvider.GetFallbackPolicyAsync();

  // También puede ser nullable
  public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
  {
    using var scope = _scopeFactory.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TurneroDbContext>();

    // Verificar si existe el permiso en la tabla
    var permisoExiste = dbContext.Permisos.Any(p => p.Nombre == policyName);
    if (!permisoExiste)
    {
      // Si no existe, usar fallback
      return await _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }

    // Crear política dinámica
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("permiso", policyName)
        .Build();

    return policy;
  }
}