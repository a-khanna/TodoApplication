using System;
using System.Text;

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
    }
}
