using Interview.Model.UriShortener.DTOs;

namespace Interview.BLL.UriShortener.Interfaces;

public interface IUriShortenerService
{
    Task<UriShortenerDto> CreateShortUriAsync(string originalUri);

    Task<UriShortenerDto> GetShortUriAsync(string originalUri);

    Task<bool> CheckIsExistUriAsync(string orginalUrl);

    Task IncrementUsedUriAsync(string shortUri);

    Task<int> GetUrlUsedCount(string shortUrl);
}
