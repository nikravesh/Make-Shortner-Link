using Moq;
using ShortenerLink.API.UriShortener;
using ShortenerLink.BLL.UriShortener.Interfaces;

namespace ShortenerLink.Tests;

public class UriShortenerControllerTests
{
    [Theory]
    [InlineData("https://github.com/nikravesh/Make-Shortner-Link")]
    public async Task CreateShortenerUrlWhenCalled(string longUrl)
    {
        Mock<IUriShortenerService> urlShortenerService = new();

        var controller = new UriShortenerController(urlShortenerService.Object);

        var response = await controller.CreateShortUrl(longUrl);

        Assert.True(response!=null);
    }
}