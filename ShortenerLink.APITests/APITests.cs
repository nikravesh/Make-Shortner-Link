using Microsoft.AspNetCore.Mvc;

using Moq;
using ShortenerLink.API.UriShortener;
using ShortenerLink.BLL.UriShortener.Interfaces;

namespace ShortenerLink.APITests;

[TestFixture]
public class APITests
{
    private Mock<IUriShortenerService> _urlShortenerService;

    [SetUp]
    public void Setup()
    {
        _urlShortenerService = new Mock<IUriShortenerService>();
    }

    [Test]
    [TestCase("https://www.varzesh3.com/news/1850854/%D8%AA%DB%8C%D9%85-%D9%85%D9%84%DB%8C-%D9%88-%D9%85%D8%B5%D8%AF%D9%88%D9%85%DB%8C%D8%AA-%D8%A7%DB%8C%D9%86-%D8%AF%D9%88-%D8%B3%D8%AA%D8%A7%D8%B1%D9%87-%D8%B3%D9%BE%D8%A7%D9%87%D8%A7%D9%86-%D9%88-%D9%81%D9%88%D9%84%D8%A7%D8%AF")]
    public async Task CreateShortUrl_WhenCall_GetShortLink(string url)
    {
        var controller = new UriShortenerController(_urlShortenerService.Object);

        var response = await controller.CreateShortUrl(url);

        Assert.Equals(typeof(OkObjectResult), response);

    }
}