using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Todo.Core;

namespace Todo.API.Middlewares
{
    public class GraphQLJWTMiddleware : IMiddleware
    {
        private readonly string issuer;
        private readonly string key;

        public GraphQLJWTMiddleware(IConfiguration configuration)
        {
            issuer = configuration.GetValue<string>(Constants.AppSettingsJwtIssuer);
            key = configuration.GetValue<string>(Constants.AppSettingsJwtKey);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                token = token.Replace("Bearer ", string.Empty);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

                //attach user to context on successful jwt validation
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == Constants.UserIdClaim).Value);
                context.Items[Constants.UserIdClaim] = userId;
            }
            await next(context);          
        }
    }
}
