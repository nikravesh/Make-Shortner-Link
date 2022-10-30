using Interview.API.Core;
using Interview.BLL.UriShortener.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            var uriShortenerDto = await _uriShortnereService.CreateShortUriAsync(orginalUrl);
            return Ok(uriShortenerDto);
        }
        catch (SqlException e)
        {
            return Content(APIErrorMessageConstants.TryAgain);
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
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            if (await _uriShortnereService.CheckIsExistUriAsync(url))
                return BadRequest(APIErrorMessageConstants.NotFoundUrl);

            var uriShortenerDto = await _uriShortnereService.GetShortUriAsync(url);

            await _uriShortnereService.IncrementUsedUriAsync(uriShortenerDto.ShortenerUri);

            return Redirect(uriShortenerDto.OrginalUri);
        }
        catch (SqlException e)
        {
            return Content(APIErrorMessageConstants.TryAgain);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Route("navigateCount")]
    [HttpGet]
    public async Task<IActionResult> GetUrlUsed([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            return Ok(await _uriShortnereService.GetUrlUsedCount(url));
        }
        catch (SqlException e)
        {
            return Content(APIErrorMessageConstants.TryAgain);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
