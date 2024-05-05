using Microsoft.AspNetCore.Mvc;
using ShortenerLink.API.Core;
using ShortenerLink.API.UriShortener;

namespace ShortenerLink.Tests;

public class UriShortenerControllerTests
{
    [Theory]
    [InlineData("https://github.com/nikravesh/Make-Shortner-Link")]
    public async Task CreateShortenerUrlWhenCalled(string longUrl)
    {
        UriShortenerController controller = UriShortenerControllerTestsMotherObject.GetUriShortenerControllerObject;

        var response = await controller.CreateShortUrl(longUrl);

        Assert.IsType<OkObjectResult>(response);
        Assert.True(response != null);
    }

    [Theory]
    [InlineData(null)]
    public async Task CreateShortenerUrlWithNullUrlWhenCalledReturnBadRequest(string url)
    {
        UriShortenerController controller = UriShortenerControllerTestsMotherObject.GetUriShortenerControllerObject;

        var response = await controller.CreateShortUrl(url);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("url cannot be null!", APIErrorMessageConstants.BadRequestMessage);
    }

    [Fact]
    public async Task CreateShortenerUrlWithEmptyUrlWhenCalledReturnBadRequest()
    {
        UriShortenerController controller = UriShortenerControllerTestsMotherObject.GetUriShortenerControllerObject;

        var response = await controller.CreateShortUrl(string.Empty);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("url cannot be null!", APIErrorMessageConstants.BadRequestMessage);
    }

    [Theory]
    [InlineData("https://github.com/nikravesh/Make-Shortner Link.git")]
    public async Task CreateShortenerUrlWithSpaceContainUrlWhenCalledReturnBadRequest(string url)
    {
        UriShortenerController controller = UriShortenerControllerTestsMotherObject.GetUriShortenerControllerObject;

        var response = await controller.CreateShortUrl(url);

        Assert.IsType<BadRequestObjectResult>(response);
        Assert.Equal("url cannot be null!", APIErrorMessageConstants.BadRequestMessage);
    }
}
