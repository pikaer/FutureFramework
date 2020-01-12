namespace Infrastructure
{
    public static class StringHelper
    {
        public static string TextCut(this string text, int pos)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return text;
            }
            else if (text.Length <= pos)
            {
                return text;
            }
            else
            {
                string result = text.Substring(0, pos);
                return result + "...";
            }
        }
    }
}
