﻿namespace Mitekat.Discovery.Helpers.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;
using Mitekat.Discovery.Core.Abstraction.Helpers;

internal class TokenHelper : ITokenHelper
{
    private JwtBuilder TokenBuilder => JwtBuilder
        .Create()
        .WithAlgorithm(new HMACSHA512Algorithm())
        .WithSecret(configuration.Secret);

    private readonly AuthConfiguration configuration;

    public TokenHelper(AuthConfiguration configuration) =>
        this.configuration = configuration;

    public AccessTokenPayload ParseAccessToken(string accessToken)
    {
        var claims = ParseToken(accessToken);
        if (claims is null)
        {
            return null;
        }
        
        var userId = Guid.Parse(claims["sub"]);
        return new AccessTokenPayload(userId);
    }

    private IDictionary<string, string> ParseToken(string token)
    {
        try
        {
            var json = TokenBuilder.MustVerifySignature().Decode(token);
            var claims = JsonSerializer.Deserialize<IDictionary<string, object>>(json)!;
            return claims.ToDictionary(claim => claim.Key, claim => claim.Value.ToString());
        }
        catch (Exception)
        {
            return null;
        }
    }
}
