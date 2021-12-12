namespace Mitekat.Discovery.Helpers.Tokens;

using Microsoft.Extensions.Configuration;
using Mitekat.Discovery.Core.Seedwork.Configuration;

internal class AuthConfiguration
{
    public string Secret =>
        configuration
            .GetParameter("Auth:Secret", "auth secret")
            .Required();

    private readonly IConfiguration configuration;

    public AuthConfiguration(IConfiguration configuration) =>
        this.configuration = configuration;
}
