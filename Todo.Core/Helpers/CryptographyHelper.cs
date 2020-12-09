using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Todo.Core.Helpers
{
    public static class CryptographyHelper
    {
        /// <summary>
        /// Returns salt and hash for the input string using SHA512
        /// </summary>
        /// <param name="input">string to be hashed</param>
        /// <returns></returns>
        public static (byte[], byte[]) GetSaltAndHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Cannot create hash for null or empty string.");
            
            byte[] salt;
            byte[] hash;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            return (salt, hash);
        }

        /// <summary>
        /// SHA512 hash comparison
        /// </summary>
        /// <param name="toBeHashed">String to be hashed and compared with the existing hash</param>
        /// <param name="existingHash">Existing hash to be used for comparison</param>
        /// <param name="existingSalt">Existing salt to be used for comparison</param>
        /// <returns>Boolean for match result</returns>
        public static bool CompareHashes(string toBeHashed, byte[] existingHash, byte[] existingSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(existingSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(toBeHashed));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != existingHash[i])
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Generates a JWT with userid and username in the claims
        /// </summary>
        /// <param name="jwtKey">Key used for encryption</param>
        /// <param name="jwtIssuer">Issuer of the JWT</param>
        /// <param name="userId">User id to be included in claims</param>
        /// <param name="username">Username to be included in claims</param>
        /// <returns>JWT string</returns>
        public static string GenerateJSONWebToken(string jwtKey, string jwtIssuer, string userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(Constants.UserIdClaim, userId),
                new Claim(Constants.UsernameClaim, username),
                new Claim(Constants.IssuerClaim, jwtIssuer)
            };

            var token = new JwtSecurityToken(jwtIssuer, jwtIssuer, claims, expires: DateTime.Now.AddMinutes(120), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
