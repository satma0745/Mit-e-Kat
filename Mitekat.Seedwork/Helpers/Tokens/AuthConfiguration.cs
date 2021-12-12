namespace Mitekat.Seedwork.Helpers.Tokens;

using Microsoft.Extensions.Configuration;
using Mitekat.Seedwork.Configuration;

public class AuthConfiguration : IAuthConfiguration
{
    public virtual string Secret =>
        Configuration
            .GetParameter(SecretParameterPath, "auth secret")
            .Required();

    protected virtual string SecretParameterPath => "Auth:Secret";

    protected virtual IConfiguration Configuration { get; }

    public AuthConfiguration(IConfiguration configuration) =>
        Configuration = configuration;
}
