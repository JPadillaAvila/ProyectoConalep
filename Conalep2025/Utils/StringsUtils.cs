namespace Conalep2025.Utils
{
    public class StringsUtils
    {
        

       public static string? ExtractStringBeetween(string text, string startText, string endText)
        {

            var start = text.LastIndexOf(startText) + startText.Length;
            var end = text.IndexOf(endText);
            var length = end - start;
            if (length < 0) { return null; }
            return text.Substring(start, length);
        }

        public static string? ExtractStringBeetween(string text, string startText)
        {

            var start = text.LastIndexOf(startText) + startText.Length;
            var end = text.Length;
            var length = end - start;
            return text.Substring(start, length);
        }
    }
}
