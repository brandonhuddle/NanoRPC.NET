namespace NanoRpc.Extensions
{
    public static class StringArrayExtensions
    {
        public static string ToJsonString(this string[] array, bool includeSquareBrackets = false)
        {
            string result = includeSquareBrackets ? "[" : "";

            for (int i = 0; i < array.Length; ++i)
            {
                if (i != 0)
                {
                    result += ", ";
                }

                result += "\"" + array[i] + "\"";
            }

            return array + (includeSquareBrackets ? "]" : "");
        }
    }
}
