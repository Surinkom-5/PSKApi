namespace FlowerShop.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLastOccurrence(this string Source, string find, string replace)
        {
            int place = Source.LastIndexOf(find);

            return place == -1 ? Source : Source.Remove(place, find.Length).Insert(place, replace);
        }
    }
}
