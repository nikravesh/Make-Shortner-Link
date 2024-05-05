using System.Text.RegularExpressions;
using System;

using Moq;

using ShortenerLink.API.UriShortener;
using ShortenerLink.BLL.UriShortener.Interfaces;

namespace ShortenerLink.Tests;

public class UriShortenerControllerTestsMotherObject
{
    private static Mock<IUriShortenerService> UrlShortenerService => new();
    public static UriShortenerController GetUriShortenerControllerObject => new(UrlShortenerService.Object);
}