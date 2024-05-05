using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace ShortenerLink.BLL.Core;

public class UrlHelper
{
    public static string GetUriLeftPart(string uri)
    {
        return new Uri(uri).GetLeftPart(UriPartial.Authority);
    }
    public static string GetShortUri(string host, int tokenLength = 16)
    {
        var rnd = new RNGCryptoServiceProvider();
        var tokenBytes = new byte[tokenLength];
        rnd.GetBytes(tokenBytes);
        var token =
            tokenBytes
                .Select(b => UriConstants.TOKENALPHABET[b % UriConstants.TOKENALPHABET.Length])
                .ToArray();

        return host + "/" + new string(token);
    }

    public static bool IsUrlValid(string url)
    {
        string pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
        Regex Rgx = new(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        return Rgx.IsMatch(url);
    }
}
