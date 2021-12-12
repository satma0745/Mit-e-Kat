namespace Mitekat.Seedwork.Persistence.Configuration;

using Microsoft.Extensions.Configuration;
using Mitekat.Seedwork.Configuration;

public class PersistenceConfiguration : IPersistenceConfiguration
{
    public virtual string ConnectionString =>
        $"Server={Server};Port={Port};Database={Database};User Id={Username};Password={Password};";

    protected virtual string Server =>
        Configuration
            .GetParameter(ServerParameterPath, "database server")
            .Required();

    protected virtual string ServerParameterPath => "Persistence:Server";

    protected virtual string Port =>
        Configuration
            .GetParameter(PortParameterPath, "database port")
            .Required();

    protected virtual string PortParameterPath => "Persistence:Port";

    protected virtual string Database =>
        Configuration
            .GetParameter(DatabaseParameterPath, "database name")
            .Required();

    protected virtual string DatabaseParameterPath => "Persistence:Database";

    protected virtual string Username =>
        Configuration
            .GetParameter(UsernameParameterPath, "database user's username")
            .Required();

    protected virtual string UsernameParameterPath => "Persistence:Username";

    protected virtual string Password =>
        Configuration
            .GetParameter(PasswordParameterPath, "database user's password")
            .Required();

    protected virtual string PasswordParameterPath => "Persistence:Password";
    
    protected IConfiguration Configuration { get; }

    public PersistenceConfiguration(IConfiguration configuration) =>
        Configuration = configuration;
}
