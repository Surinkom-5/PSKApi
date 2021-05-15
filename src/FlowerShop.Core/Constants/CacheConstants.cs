using System;

namespace FlowerShop.Core.Constants
{
    public static class CacheConstants
    {
        public static readonly TimeSpan CachedTime = TimeSpan.FromHours(6);

        public const string CategoryCacheKey = "category-cache";
    }
}