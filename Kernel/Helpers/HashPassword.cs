using System.Text;
using XSystem.Security.Cryptography;

namespace Kernel.Helpers
{
    public class HashPassword
    {
        public static string Hash(string value)
        {
            var algorithm = new SHA256Managed();
            var plainBytes = Encoding.UTF8.GetBytes(value);
            var cypherBytes = algorithm.ComputeHash(plainBytes);
            return Convert.ToBase64String(cypherBytes);
        }
    }
}
