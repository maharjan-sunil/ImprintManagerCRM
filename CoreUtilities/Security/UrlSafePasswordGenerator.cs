using System.Security.Cryptography;
using System.Text;

namespace CoreUtilities.Security
{
    public static class UrlSafePasswordGenerator
    {
        private const string Uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijkmnopqrstuvwxyz";
        private const string Digits = "23456789"; // Excludes 0, 1
        private const string Symbols = "!@$?_-";

        private static readonly string AllChars = Uppercase + Lowercase + Digits + Symbols;

        public static string Generate(int length = 12)
        {
            if (length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            var password = new StringBuilder(length);

            // Ensure at least one from each required category
            password.Append(GetRandomChar(Uppercase));
            password.Append(GetRandomChar(Lowercase));
            password.Append(GetRandomChar(Digits));
            password.Append(GetRandomChar(Symbols));

            // Fill remaining with random mix of all
            for (int i = password.Length; i < length; i++)
            {
                password.Append(GetRandomChar(AllChars));
            }

            // Shuffle the result to avoid predictable order
            return Shuffle(password.ToString());
        }

        private static char GetRandomChar(string source)
        {
            byte[] random = new byte[1];
            using var rng = RandomNumberGenerator.Create();
            do
            {
                rng.GetBytes(random);
            } while (random[0] >= byte.MaxValue - (byte.MaxValue % source.Length));

            return source[random[0] % source.Length];
        }

        private static string Shuffle(string input)
        {
            var array = input.ToCharArray();
            using var rng = RandomNumberGenerator.Create();
            for (int i = array.Length - 1; i > 0; i--)
            {
                byte[] box = new byte[1];
                rng.GetBytes(box);
                int j = box[0] % (i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
            return new string(array);
        }
    }
}
