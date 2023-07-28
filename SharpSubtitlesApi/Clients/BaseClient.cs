using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace SharpSubtitlesApi.Clients;

public class BaseClient : BindableBase, IDisposable
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
	private bool _disposed;
	public readonly object Mutex = new();
    private readonly System.Timers.Timer _resetRequestsTimer = new(1000)
    {
        AutoReset = false,
    };

	protected readonly HttpClient _httpClient;
	protected string _apiAddress = string.Empty;
	protected string? _apiKey = string.Empty;
	protected string? _authToken;
	private ulong _totalRequests;
	protected int _maximumRequestsPerSecond = -1;
	private int _requestsInCurrentSecond;
	private bool _autoWaitForRequestLimit;

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
    public string? ApiKey
    {
        get => _apiKey;
        set => SetAndRaisePropertyIfChanged(ref _apiKey, value);
    }

    /// <summary>
    /// Gets if this client requires an auth token for most of the calls.<br/>
    /// This is often retrieved from a user login.<br/>
    /// Set to true if any of the requests require it
    /// </summary>
    public virtual bool RequireAuthToken => false;

    /// <summary>
    /// Gets or sets the auth token required to make requests
    /// </summary>
    public string? AuthToken
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

    /// <summary>
    /// Gets the total number of requests made with this client
    /// </summary>
    public ulong TotalRequests => _totalRequests;

	/// <summary>
	/// Gets or sets the maximum number of requests per second the client can handle before error.<br/>
	///  &lt;= 0 = Unlimited
	/// </summary>
	public int MaximumRequestsPerSecond
    {
	    get => _maximumRequestsPerSecond;
	    set => SetAndRaisePropertyIfChanged(ref _maximumRequestsPerSecond, value);
    }

	/// <summary>
	/// Gets the requests made in the current second.<br/>
	/// It resets every second
	/// </summary>
	public int RequestsInCurrentSecond => _requestsInCurrentSecond;

    /// <summary>
    /// True if the requests hit the limit in the actual second
    /// </summary>
    public bool RequestsHitLimit => _maximumRequestsPerSecond > 0 && _requestsInCurrentSecond >= _maximumRequestsPerSecond;

    /// <summary>
    /// Gets or sets if the request should be delayed if limits are hit.
    /// <remarks>There's no guarantee that the request will be made inside limits.</remarks>
    /// </summary>
    public bool AutoWaitForRequestLimit
    {
	    get => _autoWaitForRequestLimit;
	    set
	    {
		    _autoWaitForRequestLimit = value;
		    _requestsInCurrentSecond = 0;
	    }
    }

    #endregion

    #region Constructors
    public BaseClient() : this(null) { }
    public BaseClient(HttpClient? httpClient) : this(string.Empty, string.Empty, httpClient) {}
    public BaseClient(string apiAddress, HttpClient? httpClient = null) : this(apiAddress, null, httpClient) { }

	public BaseClient(string apiAddress, string apiKey, HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? HttpClientShared.Value;
        ApiAddress = apiAddress;
        ApiKey = apiKey;

        _resetRequestsTimer.Elapsed += ResetRequestsTimerOnElapsed;
	}

    private void ResetRequestsTimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
	    Interlocked.Exchange(ref _requestsInCurrentSecond, 0);
    }

    #endregion

    #region Overrides
    public override string ToString()
    {
        return $"{GetType().Name} {nameof(Version)}: {Version}, {nameof(ApiAddress)}: {ApiAddress}, {nameof(RequireApiKey)}: {RequireApiKey}, {nameof(ApiKey)}: {ApiKey}, {nameof(RequireAuthToken)}: {RequireAuthToken}, {nameof(AuthToken)}: {AuthToken}";
    }

    public void Dispose()
    {
	    if (_disposed) return;
	    _disposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods
    /// <summary>
    /// Gets the absolute url for an request to the Api
    /// </summary>
    /// <param name="path">Partial path without Api address</param>
    /// <param name="parameters">Parameters to send with the request</param>
    /// <returns>Absolute path for the request</returns>
    public string GetRequestUrl(string path, IReadOnlyDictionary<string, object>? parameters = null)
    {
        return $"{_apiAddress}/{path.Trim(' ', '/')}{GetUrlParametersString(parameters)}";
    }

    /// <summary>
    /// Gets the absolute url for an request to the Api
    /// </summary>
    /// <param name="path">Partial path without Api address</param>
    /// <param name="classObj">Class to parse the parameters from</param>
    /// <returns>Absolute path for the request</returns>
    public string GetRequestUrl(string path, object? classObj)
    {
	    return $"{_apiAddress}/{path.Trim(' ', '/')}{GetUrlParametersString(classObj)}";
    }

	/// <summary>
	/// Gets the parameters string from a dictionary of key and values parameters
	/// </summary>
	/// <param name="parameters"></param>
	/// <returns>The formatted string</returns>
	public static string GetUrlParametersString(IReadOnlyDictionary<string, object>? parameters)
    {
        if (parameters is null || parameters.Count == 0) return string.Empty;

        var builder = new StringBuilder();

        // Sort the parameters alphabetically to avoid http redirection.
        foreach (var item in parameters.OrderBy(x => x.Key))
        {
            builder.Append(builder.Length == 0 ? "?" : "&");
            builder.Append($"{HttpUtility.UrlEncode(item.Key.ToLowerInvariant())}={HttpUtility.UrlEncode(item.Value.ToString()?.ToLowerInvariant() ?? string.Empty)}");
        }

        return builder.ToString();
    }

	/// <summary>
	/// Gets the parameters string from a class (Reflection)
	/// </summary>
	/// <param name="obj"></param>
	/// <returns>The formatted string</returns>
	public static string GetUrlParametersString(object? obj)
    {
	    if (obj is null) return string.Empty;

	    var dict = new Dictionary<string, object>();

	    var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

	    foreach (var propertyInfo in properties)
	    {
		    var method = propertyInfo.GetMethod;
            if (method is null) continue;
            var value = propertyInfo.GetValue(obj);

            switch (value)
            {
	            case null:
		            continue;
	            case IList list:
		            if (list.Count == 0) continue;

		            var items = new List<string>(list.Count);
		            foreach (var item in list)
		            {
                        if (item is null) continue;
						items.Add(item.ToString()!);
					}
		            value = string.Join(',', items);
		            break;
			}

            var attr = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            dict.Add(attr?.Name ?? method.Name, value);
	    }

	    return GetUrlParametersString(dict);

    }

	public Task<T?> PostJsonAsync<T>(string requestUrl, object postData, Dictionary<string, object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var url = GetRequestUrl(requestUrl, urlParameters);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var requestJson = JsonSerializer.Serialize(postData);
        request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        return SendRequestAsync<T>(request, cancellationToken);
    }

	public Task<T?> PostJsonAsync<T>(string requestUrl, object postData, object? classUrlParameters, CancellationToken cancellationToken = default)
	{
		var url = GetRequestUrl(requestUrl, classUrlParameters);
		using var request = new HttpRequestMessage(HttpMethod.Post, url);
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		var requestJson = JsonSerializer.Serialize(postData);
		request.Content = new StringContent(requestJson, Encoding.UTF8, "application/json");

		return SendRequestAsync<T>(request, cancellationToken);
	}

	public Task<T?> PostJsonAsync<T>(string requestUrl, object postData, CancellationToken cancellationToken = default) => PostJsonAsync<T>(requestUrl, postData, null, cancellationToken);

	public Task<T?> GetJsonAsync<T>(string requestUrl, Dictionary<string, object>? urlParameters, CancellationToken cancellationToken = default)
    {
        var url = GetRequestUrl(requestUrl, urlParameters);
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return SendRequestAsync<T>(request, cancellationToken);
    }

	public Task<T?> GetJsonAsync<T>(string requestUrl, object? classUrlParameters, CancellationToken cancellationToken = default)
	{
		var url = GetRequestUrl(requestUrl, classUrlParameters);
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

    public Task<T?> DeleteJsonAsync<T>(string requestUrl, object? classUrlParameters, CancellationToken cancellationToken = default)
    {
	    var url = GetRequestUrl(requestUrl, classUrlParameters);
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

        if (_autoWaitForRequestLimit && RequestsHitLimit)
        {
	        do
	        {
				await Task.Delay(RandomNumberGenerator.GetInt32(500, 2000), cancellationToken).ConfigureAwait(false);
			} while (RequestsHitLimit);
        }
        
        using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        Interlocked.Increment(ref _totalRequests);

        if (_autoWaitForRequestLimit && _maximumRequestsPerSecond > 0)
        {
	        Interlocked.Increment(ref _requestsInCurrentSecond);
			_resetRequestsTimer.Start();
        }

        var stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        return await JsonSerializer.DeserializeAsync<T>(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    #endregion
}