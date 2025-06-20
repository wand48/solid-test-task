namespace Solid.TestTask.Helpers
{
    public static class UriHelper
    {
        public static Uri GetUri(string uriString, string messagePart)
        {
            Uri uri = new Uri(uriString);

            uri.Validate(messagePart);

            return uri;
        }

        public static void Validate(this Uri uri, string messagePart)
        {
            if (!uri.IsAbsoluteUri)
            {
                throw new UriFormatException($"URI {messagePart} не является абсолютным");
            }

            if (string.IsNullOrWhiteSpace(uri.Scheme) ||
                !(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                throw new UriFormatException($"URI {messagePart} должен использовать HTTP или HTTPS схему");
            }
        }
    }
}
