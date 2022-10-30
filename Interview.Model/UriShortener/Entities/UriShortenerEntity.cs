namespace Interview.Model.UriShortener.Entities;

public class UriShortenerEntity
{
    public int Id { get; set; }

    public string OrginalUri { get; set; }

    public string ShortenerUri { get; set; }

    public int UsedUriCount { get; set; }
}
