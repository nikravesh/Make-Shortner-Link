using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using ShortenerLink.API.Core;
using ShortenerLink.BLL.UriShortener.Interfaces;

namespace ShortenerLink.API.UriShortener;

[Route("api/[controller]")]
[ApiController]
public class UriShortenerController : ControllerBase
{
    #region Fields
    private static SemaphoreSlim _semaphore;
    private readonly IUriShortenerService _uriShortnereService;
    #endregion

    #region Constructor
    public UriShortenerController(IUriShortenerService uriShortnereService)
    {
        _uriShortnereService = uriShortnereService;
        _semaphore = new SemaphoreSlim(1, 1);
    }
    #endregion

    #region APIs
    /// <summary>
    /// This API for create short link 
    /// </summary>
    /// <param name="orginalUrl">the original link</param>
    /// <returns>original link and also short link</returns>
    [Route("create")]
    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] string orginalUrl)
    {
        if (string.IsNullOrWhiteSpace(orginalUrl))
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            await _semaphore.WaitAsync();
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
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// This API navigate to link with short link
    /// </summary>
    /// <param name="shortUrl">the short url for redirection</param>
    /// <returns></returns>
    [Route("browse")]
    [HttpGet]
    public async Task<IActionResult> Go([FromQuery] string shortUrl)
    {
        if (string.IsNullOrWhiteSpace(shortUrl))
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            if (!await _uriShortnereService.CheckIsExistUriAsync(shortUrl))
                return BadRequest(APIErrorMessageConstants.NotFoundUrl);

            var uriShortenerDto = await _uriShortnereService.GetShortUriAsync(shortUrl);

            await _uriShortnereService.IncrementUsedUriAsync(uriShortenerDto.ShortenerUri);

            //redirect to (uriShortenerDto.OrginalUri);
            //var redirectPermanent = RedirectPermanent(uriShortenerDto.OrginalUri);
            return Ok(uriShortenerDto.OrginalUri);
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

    /// <summary>
    /// this method get url browsed count
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    [Route("navigateCount")]
    [HttpGet]
    public async Task<IActionResult> GetUrlUsed([FromQuery] string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return BadRequest(APIErrorMessageConstants.BadRequestMessage);

        try
        {
            await _semaphore.WaitAsync();
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
        finally
        {
            _semaphore.Release();
        }
    }
    #endregion
}
