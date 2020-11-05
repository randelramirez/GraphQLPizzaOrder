using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphQLPizzaOrder.Core.Helpers
{
    public static class CursorHelper
    {
        // We create a cursor using the ID and convert it to Base64
        public static string ToCursor(int id) => Convert.ToBase64String(BitConverter.GetBytes(id));

        public static int FromCursor(string base64) => BitConverter.ToInt32(Convert.FromBase64String(base64), 0);

        public static (string firstCursor, string lastCursor) GetFirstAndLastCursor(IEnumerable<int> enumerable)
        {
            if (enumerable?.Any() != true)
            {
                return (null, null);
            }

            var firstCursor = ToCursor(enumerable.First());
            var lastCursor = ToCursor(enumerable.Last());

            return (firstCursor, lastCursor);
        }
    }
}
