using Interview.API.Core;
using Interview.BLL.UriShortener.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Interview.API.UriShortener;
[Route("api/[controller]")]
[ApiController]
public class UriShortenerController : ControllerBase
{

    private readonly IUriShortenerService _uriShortnereService;

    public UriShortenerController(IUriShortenerService uriShortnereService)
    {
        _uriShortnereService = uriShortnereService;
    }

    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] string orginalUrl)
    {
        if (string.IsNullOrWhiteSpace(orginalUrl))
            return BadRequest(APIConstants.BadRequestMessage);

        try
        {
            var uriShortenerDto = await _uriShortnereService.CreateShortUriAsync(orginalUrl);
            return Ok(uriShortenerDto);
        }
        catch (Exception e)
        {
            //TODO : Take log

            return BadRequest(e.Message);
        }
    }

    [Route("browse")]
    [HttpGet]
    public async Task<IActionResult> Go([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest(APIConstants.BadRequestMessage);

        if (await _uriShortnereService.CheckIsExistUriAsync(url))
            return BadRequest(APIConstants.NotFoundUrl);

        var uriShortenerDto = await _uriShortnereService.GetShortUriAsync(url);

        await _uriShortnereService.IncrementUsedUriAsync(uriShortenerDto.ShortenerUri);

        return Redirect(uriShortenerDto.OrginalUri);
    }

    [Route("navigateCount")]
    [HttpGet]
    public async Task<IActionResult> GetUrlUsed([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest(APIConstants.BadRequestMessage);

        return Ok(await _uriShortnereService.GetUrlUsedCount(url));
    }
}
