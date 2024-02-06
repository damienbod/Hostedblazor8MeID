namespace BlazorWebMeID.Identity.Services;

public class BaseUrlProvider
{
    public string BaseUrl { get; private set; } = string.Empty;

    public void SetBaseUrl(string baseUrl)
    {
        BaseUrl = baseUrl;
    }
}
