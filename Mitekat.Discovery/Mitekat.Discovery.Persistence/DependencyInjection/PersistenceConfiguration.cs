namespace Mitekat.Discovery.Persistence.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Mitekat.Discovery.Core.Seedwork.Configuration;

internal class PersistenceConfiguration
{
    public string ConnectionString =>
        $"Server={Server};Port={Port};Database={Database};User Id={Username};Password={Password};";

    private string Server =>
        configuration
            .GetParameter("Persistence:Server", "database server")
            .Required();

    private string Port =>
        configuration
            .GetParameter("Persistence:Port", "database port")
            .Required();

    private string Database =>
        configuration
            .GetParameter("Persistence:Database", "database name")
            .Required();

    private string Username =>
        configuration
            .GetParameter("Persistence:Username", "database user's username")
            .Required();

    private string Password =>
        configuration
            .GetParameter("Persistence:Password", "database user's password")
            .Required();
    
    private readonly IConfiguration configuration;

    public PersistenceConfiguration(IConfiguration configuration) =>
        this.configuration = configuration;
}
