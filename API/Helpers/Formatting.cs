using System.Collections.Generic;

namespace API.Helpers
{
    public static class Formatting
    {
        public static string ListToString(List<string> input)
        {
            string x = "";

            foreach (var item in input)
            {
                x += input.IndexOf(item) == 0? item :", " + item;
            }

            return x;
        }
    }
}
