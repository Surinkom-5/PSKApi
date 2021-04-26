using FlowerShop.Infrastructure.Extensions;
using System.Linq;

namespace FlowerShop.Infrastructure.Helpers
{
    public static class SearchQueryHelper
    {
        public static string FormatSearchQuery(string searchquery)
        {
            var trimmedSearchText = searchquery.ToLower().Replace("*", string.Empty).Trim();
            var words = trimmedSearchText.Split(" ");
            var lastWord = words.Last();

            searchquery = trimmedSearchText.Replace(" ", " AND ")
                .ReplaceLastOccurrence(lastWord, $"\"{lastWord}*\"");
            return searchquery;
        }
    }
}
