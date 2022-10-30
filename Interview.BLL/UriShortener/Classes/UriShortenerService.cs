using Interview.BLL.Core;
using Interview.BLL.UriShortener.Interfaces;
using Interview.DAL.DataContext;
using Interview.Model.UriShortener.DTOs;
using Interview.Model.UriShortener.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interview.BLL.UriShortener.Classes;

public class UriShortenerService : IUriShortenerService
{
    private readonly UriShortenerDBContext _dbContext;

    public UriShortenerService(UriShortenerDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UriShortenerDto> CreateShortUriAsync(string originalUri)
    {
        var uriLeftPart = UrlHelper.GetUriLeftPart(originalUri);
        var shortUri = UrlHelper.GetShortUri(uriLeftPart);

        if (await CheckIsExistUriAsync(originalUri))
        {
            return await GetShortUriAsync(originalUri);
        }

        UriShortenerEntity shortenerEntity = new()
        {
            OrginalUri = originalUri,
            ShortenerUri = shortUri,
            UsedUriCount = 0
        };

        await _dbContext.UriShorteners.AddAsync(shortenerEntity);
        await _dbContext.SaveChangesAsync();

        return new UriShortenerDto { ShortenerUri = shortenerEntity.ShortenerUri, OrginalUri = shortenerEntity.OrginalUri };
    }

    public async Task<UriShortenerDto> GetShortUriAsync(string originalUri)
    {
        var selectedUri = await _dbContext.UriShorteners.SingleOrDefaultAsync(u => u.OrginalUri == originalUri);
        if (selectedUri == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

        return new UriShortenerDto { ShortenerUri = selectedUri.ShortenerUri, OrginalUri = selectedUri.OrginalUri };
    }

    public async Task<bool> CheckIsExistUriAsync(string orginalUrl)
    {
        var uriShortenerEntity = await _dbContext.UriShorteners.FirstOrDefaultAsync(u => u.OrginalUri == orginalUrl);

        return uriShortenerEntity != null;
    }

    public async Task IncrementUsedUriAsync(string shortUri)
    {
        var uriShortenerEntity = await _dbContext.UriShorteners.SingleOrDefaultAsync(u => u.ShortenerUri == shortUri);

        if (uriShortenerEntity == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

        uriShortenerEntity.UsedUriCount += 1;
        _dbContext.UriShorteners.Update(uriShortenerEntity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> GetUrlUsedCount(string shortUrl)
    {
        var uriShortenerEntity = await _dbContext.UriShorteners.FirstOrDefaultAsync(u => u.ShortenerUri == shortUrl);

        if (uriShortenerEntity == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

        return uriShortenerEntity.UsedUriCount;
    }
}
