namespace Mitekat.Auth.Application.Helpers.Tokens;

using System;
using ISeedworkAuthConfiguration = Mitekat.Seedwork.Helpers.Tokens.IAuthConfiguration;

internal interface IAuthConfiguration : ISeedworkAuthConfiguration
{
    TimeSpan AccessTokenLifetime { get; }
    
    TimeSpan RefreshTokenLifetime { get; }
}
