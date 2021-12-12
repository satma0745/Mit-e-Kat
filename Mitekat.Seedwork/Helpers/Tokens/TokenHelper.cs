namespace Mitekat.Seedwork.Helpers.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JWT.Algorithms;
using JWT.Builder;

public class TokenHelper : ITokenHelper
{
    protected virtual JwtBuilder TokenBuilder => JwtBuilder
        .Create()
        .WithAlgorithm(new HMACSHA512Algorithm())
        .WithSecret(Configuration.Secret);
    
    protected virtual IAuthConfiguration Configuration { get; }

    public TokenHelper(IAuthConfiguration configuration) =>
        Configuration = configuration;
    
    public virtual AccessTokenPayload ParseAccessToken(string accessToken)
    {
        var claims = ParseToken(accessToken);
        if (claims is null)
        {
            return null;
        }
        
        var userId = Guid.Parse(claims["sub"]);
        return new AccessTokenPayload(userId);
    }

    protected IDictionary<string, string> ParseToken(string token)
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
