using Interview.BLL.Core;
using Interview.BLL.UriShortener.Interfaces;
using Interview.DAL.DataContext;
using Interview.Model.UriShortener.DTOs;
using Interview.Model.UriShortener.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Interview.BLL.UriShortener.Classes;

public class UriShortenerService : IUriShortenerService
{
    private readonly UriShortenerDBContext _dbContext;

    public UriShortenerService(UriShortenerDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UriShortenerDto> CreateShortUriAsync(string originalUrl)
    {
        try
        {
            var uriLeftPart = UrlHelper.GetUriLeftPart(originalUrl);
            var shortUri = UrlHelper.GetShortUri(uriLeftPart);

            if (await CheckIsExistUriAsync(originalUrl))
            {
                return await GetShortUriAsync(originalUrl);
            }

            UriShortenerEntity shortenerEntity = new()
            {
                OrginalUri = originalUrl,
                ShortenerUri = shortUri,
                UsedUriCount = 0
            };

            await _dbContext.UriShorteners.AddAsync(shortenerEntity);
            await _dbContext.SaveChangesAsync();

            return new UriShortenerDto { ShortenerUri = shortenerEntity.ShortenerUri, OrginalUri = shortenerEntity.OrginalUri };
        }
        catch (SqlException e) 
        {
            //todo : take log
            throw;
        }
    }

    public async Task<UriShortenerDto> GetShortUriAsync(string shortUrl)
    {
        try
        {
            var selectedUri = await _dbContext.UriShorteners.SingleOrDefaultAsync(u => u.ShortenerUri == shortUrl);
            if (selectedUri == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

            return new UriShortenerDto { ShortenerUri = selectedUri.ShortenerUri, OrginalUri = selectedUri.OrginalUri };
        }
        catch (SqlException e)
        {

            throw;
        }
    }

    public async Task<bool> CheckIsExistUriAsync(string orginalUrl)
    {
        try
        {
            var uriShortenerEntity = await _dbContext.UriShorteners.FirstOrDefaultAsync(u => u.OrginalUri == orginalUrl);

            return uriShortenerEntity != null;
        }
        catch (SqlException e)
        {

            throw;
        }
    }

    public async Task IncrementUsedUriAsync(string shortUrl)
    {
        try
        {
            var uriShortenerEntity = await _dbContext.UriShorteners.SingleOrDefaultAsync(u => u.ShortenerUri == shortUrl);

            if (uriShortenerEntity == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

            uriShortenerEntity.UsedUriCount += 1;
            _dbContext.UriShorteners.Update(uriShortenerEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (SqlException e)
        {

            throw;
        }
    }

    public async Task<int> GetUrlUsedCount(string shortUrl)
    {
        try
        {
            var uriShortenerEntity = await _dbContext.UriShorteners.FirstOrDefaultAsync(u => u.ShortenerUri == shortUrl);

            if (uriShortenerEntity == null) throw new Exception(ExceptionMessageConstants.UrlNotFound);

            return uriShortenerEntity.UsedUriCount;
        }
        catch (SqlException e)
        {
            throw;
        }
    }
}
