namespace Kernel.Helpers
{
    public static class CacheKeyGenerator
    {
        public static string CreateCacheKey(params string[] factors)
        {
            if (factors == null || factors.Length == 0)
            {
                return string.Empty;
            }
            return string.Join(":", factors);
        }

    }
}
