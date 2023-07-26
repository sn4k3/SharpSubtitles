using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SharpSubtitlesApi.Clients;

public class BaseClient : BindableBase
{
    #region Constants

    private static readonly Lazy<HttpClient> HttpClientShared = new(() =>
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", About.SoftwareWithVersion);
        return httpClient;
    });

    #endregion

    #region Members
    protected readonly HttpClient _httpClient;
    private string _apiAddress = string.Empty;
    private string _apiKey = string.Empty;
    private string _authToken = string.Empty;
    #endregion

    #region Properties

    /// <summary>
    /// Gets the version of the used Api
    /// </summary>
    public virtual int Version => 0;

    /// <summary>
    /// Gets or sets the Api address for the calls.
    /// </summary>
    public string ApiAddress
    {
        get => _apiAddress;
        set => SetAndRaisePropertyIfChanged(ref _apiAddress, value.Trim(' ', '/'));
    }

    /// <summary>
    /// Gets if this client requires an api key for the calls.
    /// </summary>
    public virtual bool RequireApiKey => false;

    /// <summary>
    /// Gets or sets the Api key for the calls
    /// </summary>
    public string ApiKey
    {
        get => _apiKey;
        set => SetAndRaisePropertyIfChanged(ref _apiKey, value);
    }

    /// <summary>
    /// Gets if this client requires an auth token for most of the calls.<br/>
    /// This is often retrieved from a user login.
    /// </summary>
    public virtual bool RequireAuthToken => false;

    /// <summary>
    /// Gets or sets the auth token required to make requests
    /// </summary>
    public string AuthToken
    {
        get => _authToken;
        set => SetAndRaisePropertyIfChanged(ref _authToken, value);
    }

    /// <summary>
    /// Gets this client name / provider
    /// </summary>
    public string ClientName => GetType().Name[..^6];

    /// <summary>
    /// Gets the main website url for this client
    /// </summary>
    public virtual string WebsiteUrl
    {
        get
        {
            var uriAddress = new Uri(_apiAddress);
            var domain = uriAddress.GetLeftPart(UriPartial.Authority);
            return Regex.Replace(domain, @"\/\/(.*api|www)[.]", "//");
        }
    }

    #endregion

    #region Constructors
    public BaseClient() : this(null) { }
    public BaseClient(HttpClient? httpClient) : this(string.Empty, string.Empty, httpClient) {}
    public BaseClient(string apiKey, HttpClient? httpClient = null) : this(string.Empty, apiKey, httpClient) {}
    public BaseClient(string apiAddress, string apiKey, HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? HttpClientShared.Value;
        ApiAddress = apiAddress;
        ApiKey = apiKey;
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        return $"{GetType().Name} {nameof(Version)}: {Version}, {nameof(ApiAddress)}: {ApiAddress}, {nameof(RequireApiKey)}: {RequireApiKey}, {nameof(ApiKey)}: {ApiKey}, {nameof(RequireAuthToken)}: {RequireAuthToken}, {nameof(AuthToken)}: {AuthToken}";
    }
    #endregion

    #region Methods
    /// <summary>
    /// Gets the absolute url for an request to the Api
    /// </summary>
    /// <param name="path">Partial path without Api address</param>
    /// <param name="parameters">Parameters to send with the request</param>
    /// <returns>Absolute path for the request</returns>
    public string GetRequestUrl(string path, Dictionary<string, object>? parameters = null)
    {
        return $"{_apiAddress}/{path.Trim(' ', '/')}{GetUrlParametersString(parameters)}";
    }

    /// <summary>
    /// Gets the parameters string from a dictionary of key and values parameters
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns>The formatted string</returns>
    public static string GetUrlParametersString(Dictionary<string, object>? parameters)
    {
        if (parameters is null || parameters.Count == 0) return string.Empty;

        var builder = new StringBuilder();

        // Sort the parameters alphabetically to avoid http redirection.
        foreach (var item in parameters.OrderBy(x => x.Key))
        {
            builder.Append(builder.Length == 0 ? "?" : "&");
            builder.Append($"{item.Key.ToLowerInvariant()}={item.Value.ToString()?.ToLowerInvariant()}");
        }

        return builder.ToString();
    }

    public Task<T?> PostJsonAsync<T>(string requestUrl, object obj, Dictionary<string, object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var url = GetRequestUrl(requestUrl, urlParameters);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestJson = JsonSerializer.Serialize(obj);
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        return SendRequestAsync<T>(request, cancellationToken);
    }

    public Task<T?> PostJsonAsync<T>(string requestUrl, object obj, CancellationToken cancellationToken = default) => PostJsonAsync<T>(requestUrl, obj, null, cancellationToken);

    public Task<T?> GetJsonAsync<T>(string requestUrl, Dictionary<string, object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var url = GetRequestUrl(requestUrl, urlParameters);
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return SendRequestAsync<T>(request, cancellationToken);
    }

    public Task<T?> GetJsonAsync<T>(string requestUrl, CancellationToken cancellationToken = default) => GetJsonAsync<T>(requestUrl, null, cancellationToken);

    public Task<T?> DeleteJsonAsync<T>(string requestUrl, Dictionary<string, object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var url = GetRequestUrl(requestUrl, urlParameters);
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return SendRequestAsync<T>(request, cancellationToken);
    }

    public Task<T?> DeleteJsonAsync<T>(string requestUrl, CancellationToken cancellationToken = default) => DeleteJsonAsync<T>(requestUrl, null, cancellationToken);

    /// <summary>
    /// Trigger before SendRequestAsync is executed.
    /// </summary>
    /// <remarks>Use this method to prepare the message request with any key/token</remarks>
    /// <param name="request">The formatted request</param>
    /// <param name="cancellationToken"></param>
    protected virtual Task OnBeforeSendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends a request to the Api
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        await OnBeforeSendRequestAsync(request, cancellationToken).ConfigureAwait(false);

        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        
        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    #endregion
}