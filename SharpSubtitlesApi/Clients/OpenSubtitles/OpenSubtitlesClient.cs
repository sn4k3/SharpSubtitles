﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using SharpSubtitlesApi.Clients.OpenSubtitles.Models;

namespace SharpSubtitlesApi.Clients.OpenSubtitles;

/// <summary>
/// API for https://opensubtitles.stoplight.io
/// </summary>
public class OpenSubtitlesClient : BaseClient
{
    #region Constants
    public const string ApiDefaultAddress = "https://api.opensubtitles.com/api/v1";
    public const string ApiVipAddress = "https://vip-api.opensubtitles.com/api/v1";
    public const string ApiMockAddress = "https://stoplight.io/mocks/opensubtitles/opensubtitles-api/2781383";

    /// <summary>
    /// Gets the list subtitle formats recognized by the API
    /// </summary>
    public static readonly string[] SubtitleFormats = {
        "srt",
        "sub",
        "mpl",
        "webvtt",
        "dfxp",
        "txt"
    };

    /// <summary>
    /// Gets the languages information
    /// </summary>
    public static readonly IReadOnlyDictionary<string, string> Languages = new Dictionary<string, string>
    {
        {"af", "Afrikaans"},
        {"sq", "Albanian"},
        {"ar", "Arabic"},
        {"an", "Aragonese"},
        {"hy", "Armenian"},
        {"at", "Asturian"},
        {"eu", "Basque"},
        {"be", "Belarusian"},
        {"bn", "Bengali"},
        {"bs", "Bosnian"},
        {"br", "Breton"},
        {"bg", "Bulgarian"},
        {"my", "Burmese"},
        {"ca", "Catalan"},
        {"zh-cn", "Chinese (simplified)"},
        {"zh-tw", "Chinese (traditional)"},
        {"ze", "Chinese bilingual"},
        {"hr", "Croatian"},
        {"cs", "Czech"},
        {"da", "Danish"},
        {"nl", "Dutch"},
        {"en", "English"},
        {"eo", "Esperanto"},
        {"et", "Estonian"},
        {"fi", "Finnish"},
        {"fr", "French"},
        {"gl", "Galician"},
        {"ka", "Georgian"},
        {"de", "German"},
        {"el", "Greek"},
        {"he", "Hebrew"},
        {"hi", "Hindi"},
        {"hu", "Hungarian"},
        {"is", "Icelandic"},
        {"id", "Indonesian"},
        {"it", "Italian"},
        {"ja", "Japanese"},
        {"kk", "Kazakh"},
        {"km", "Khmer"},
        {"ko", "Korean"},
        {"lv", "Latvian"},
        {"lt", "Lithuanian"},
        {"lb", "Luxembourgish"},
        {"mk", "Macedonian"},
        {"ms", "Malay"},
        {"ml", "Malayalam"},
        {"ma", "Manipuri"},
        {"mn", "Mongolian"},
        {"me", "Montenegrin"},
        {"se", "Northern Sami"},
        {"no", "Norwegian"},
        {"nb", "Norwegian Bokmal"},
        {"oc", "Occitan"},
        {"fa", "Persian"},
        {"pl", "Polish"},
        {"pt-pt", "Portuguese"},
        {"pt-br", "Portuguese (Brazilian)"},
        {"ro", "Romanian"},
        {"ru", "Russian"},
        {"sr", "Serbian"},
        {"si", "Sinhalese"},
        {"sk", "Slovak"},
        {"sl", "Slovenian"},
        {"es", "Spanish"},
        {"sw", "Swahili"},
        {"sv", "Swedish"},
        {"sy", "Syriac"},
        {"tl", "Tagalog"},
        {"ta", "Tamil"},
        {"te", "Telugu"},
        {"th", "Thai"},
        {"tr", "Turkish"},
        {"uk", "Ukrainian"},
        {"ur", "Urdu"},
        {"uz", "Uzbek"},
        { "vi", "Vietnamese" }
    };

    #endregion

    #region Properties
    /// <inheritdoc />
    public override int Version => 1;

    /// <inheritdoc />
    public override bool RequireApiKey => true;

    /// <inheritdoc />
    public override bool RequireAuthToken => true;
    #endregion

    #region Constructors

    public OpenSubtitlesClient() : base(ApiDefaultAddress, string.Empty)
    {
	    _maximumRequestsPerSecond = 5;
    }

    public OpenSubtitlesClient(HttpClient? httpClient) : base(ApiDefaultAddress, string.Empty, httpClient)
    {
	    _maximumRequestsPerSecond = 5;
	}

    public OpenSubtitlesClient(string apiKey, HttpClient? httpClient = null) : base(ApiDefaultAddress, apiKey, httpClient)
    {
	    _maximumRequestsPerSecond = 5;
	}
    #endregion

    #region Methods
    /// <inheritdoc />
    protected override Task OnBeforeSendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
	    if (RequireApiKey && ApiKey is not null)
	    {
		    request.Headers.Add("Api-Key", ApiKey);
	    }

	    if (RequireAuthToken && !string.IsNullOrEmpty(AuthToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthToken);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Gets the subtitle formats<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/69b286fc7506e-subtitle-formats
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesResultData<OpenSubtitlesSubtitleFormats>?> GetSubtitleFormatsAsync(CancellationToken token = default)
    {
        return GetJsonAsync<OpenSubtitlesResultData<OpenSubtitlesSubtitleFormats>>("infos/formats", token);
    }

    /// <summary>
    /// Get the languages table containing the codes and names used through the API<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/1de776d20e873-languages
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLanguage[]?> GetLanguagesAsync(CancellationToken token = default)
    {
        return GetJsonAsync<OpenSubtitlesLanguage[]>("infos/languages", token);
    }

    /// <summary>
    /// Get the languages table containing the codes and names used through the API<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/ea912bb244ef0-user-informations
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesResultData<OpenSubtitlesUserInformation>?> GetUserInformationAsync(CancellationToken token = default)
    {
        return GetJsonAsync<OpenSubtitlesResultData<OpenSubtitlesUserInformation>>("infos/user", token);
    }

    /// <summary>
    /// Create a token to authenticate a user.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/73acf79accc0a-login
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLogin?> LoginAsync(OpenSubtitlesPostUserLogin data, CancellationToken token = default)
    {
        return PostJsonAsync<OpenSubtitlesLogin>("login", data, token);
    }

    /// <summary>
    /// Create a token to authenticate a user.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/73acf79accc0a-login
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLogin?> LoginAsync(string username, string password, CancellationToken token = default)
    {
        return LoginAsync(new OpenSubtitlesPostUserLogin(username, password), token);
    }

    /// <summary>
    /// Destroy a user token to end a session. Bearer token is required for this endpoint.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/9fe4d6d078e50-logout
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLogout?> LogoutAsync(CancellationToken token = default)
    {
        return DeleteJsonAsync<OpenSubtitlesLogout>("logout", token);
    }

    /// <summary>
    /// Discover popular features on opensubtitles.com, according to last 30 days downloads.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6d285998026d0-popular-features
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="languages">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesResultData<OpenSubtitlesPopularFeatures[]>?> GetPopularFeaturesAsync(OpenSubtitlesMovieType type, IEnumerable<string> languages, CancellationToken token = default)
    {
        var parameters = new Dictionary<string, object>();

        if (languages.Any())
        {
            parameters.Add("languages", string.Join(',', languages));
        }

        parameters.Add("type", type.ToString().ToLowerInvariant());

        return GetJsonAsync<OpenSubtitlesResultData<OpenSubtitlesPopularFeatures[]>>("discover/popular", parameters, token);
    }

    /// <summary>
    /// Discover popular features on opensubtitles.com, according to last 30 days downloads.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6d285998026d0-popular-features
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="language">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesResultData<OpenSubtitlesPopularFeatures[]>?> GetPopularFeaturesAsync(OpenSubtitlesMovieType type = default, string language = "all", CancellationToken token = default) 
	    => GetPopularFeaturesAsync(type, new []{ language }, token);

    /// <summary>
    /// Lists 60 latest uploaded subtitles<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6d285998026d0-popular-features
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="languages">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLatestSubtitles?> GetLatestSubtitlesAsync(OpenSubtitlesMovieType type, IEnumerable<string> languages, CancellationToken token = default)
    {
        var parameters = new Dictionary<string, object>();

        if (languages.Any())
        {
            parameters.Add("languages", string.Join(',', languages));
        }

        parameters.Add("type", type.ToString().ToLowerInvariant());

        return GetJsonAsync<OpenSubtitlesLatestSubtitles>("discover/latest", parameters, token);
    }

    /// <summary>
    /// Lists 60 latest uploaded subtitles<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6d285998026d0-popular-features
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="language">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesLatestSubtitles?> GetLatestSubtitlesAsync(OpenSubtitlesMovieType type = default, string language = "all", CancellationToken token = default)
	    => GetLatestSubtitlesAsync(type, new []{ language }, token);

    /// <summary>
    /// Discover popular subtitles, according to last 30 days downloads on opensubtitles.com. This list can be filtered by language code or feature type (movie, episode).<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/3a149b956fcab-most-downloaded-subtitles
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="languages">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesMostDownloadedSubtitles?> GetMostDownloadedSubtitlesAsync(OpenSubtitlesEpisodeType type, IEnumerable<string> languages, CancellationToken token = default)
    {
        var parameters = new Dictionary<string, object>();

        if (languages.Any())
        {
            parameters.Add("languages", string.Join(',', languages));
        }

        parameters.Add("type", type.ToString().ToLowerInvariant());

        return GetJsonAsync<OpenSubtitlesMostDownloadedSubtitles>("discover/most_downloaded", parameters, token);
    }

    /// <summary>
    /// Discover popular subtitles, according to last 30 days downloads on opensubtitles.com. This list can be filtered by language code or feature type (movie, episode).<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/3a149b956fcab-most-downloaded-subtitles
    /// </summary>
    /// <param name="type">The type (movie or tvshow).</param>
    /// <param name="language">The language codes (en,fr) or all.</param>
    /// <param name="token"></param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesMostDownloadedSubtitles?> GetMostDownloadedSubtitlesAsync(OpenSubtitlesEpisodeType type = default, string language = "all", CancellationToken token = default) 
	    => GetMostDownloadedSubtitlesAsync(type, new []{ language }, token);


    /// <summary>
    /// Request a download url for a subtitle. Subtitle file in temporary URL will be always in UTF-8 encoding.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6be7f6ae2d918-download
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesDownload?> DownloadAsync(OpenSubtitlesPostDownload data, CancellationToken token = default)
    {
        return PostJsonAsync<OpenSubtitlesDownload>("download", data, token);
    }

    /// <summary>
    /// Request a download url for a subtitle. Subtitle file in temporary URL will be always in UTF-8 encoding.<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/6be7f6ae2d918-download
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<OpenSubtitlesDownload?> DownloadAsync(int fileId, CancellationToken token = default)
    {
        return DownloadAsync(new OpenSubtitlesPostDownload(fileId), token);
    }

	/// <summary>
	/// Find subtitle for a video file. All parameters can be combined following various logics: searching by a specific external id (imdb, tmdb), a file moviehash, or a simple text query.<br/>
	/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/a172317bd5ccc-search-for-subtitles
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public Task<JsonNode?> SearchSubtitlesAsync(OpenSubtitlesGetSearchSubtitles data, CancellationToken token = default)
    {
	    return GetJsonAsync<JsonNode>("subtitles", data, token);
    }


	/// <summary>
	/// <para>Extracts as much information as possible from a video filename.</para>
	/// <para>It has a very powerful matcher that allows to guess properties from a video using its filename only. This matcher works with both movies and tv shows episodes.</para>
	/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/7783e082edcf7-guessit
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public Task<OpenSubtitlesGuessIt?> GuessItAsync(string fileName, CancellationToken token = default)
    {
	    var parameters = new Dictionary<string, object>
	    {
            {"filename", fileName}
	    };
		return GetJsonAsync<OpenSubtitlesGuessIt>("utilities/guessit", parameters, token);
    }

	#endregion
}