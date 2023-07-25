﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public OpenSubtitlesClient() : base(ApiDefaultAddress, string.Empty) { }

    public OpenSubtitlesClient(HttpClient? httpClient) : base(ApiDefaultAddress, string.Empty, httpClient) { }

    public OpenSubtitlesClient(string apiKey, HttpClient? httpClient = null) : base(ApiDefaultAddress, apiKey, httpClient) { }
    #endregion

    #region Methods
    /// <inheritdoc />
    protected override Task OnBeforeSendRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        request.Headers.Add("Api-Key", ApiKey);

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
    public Task<OpenSubtitlesResultData<OpenSubtitlesSubtitleFormats>?> GetSubtitleFormatsAsync()
    {
        return GetJsonAsync<OpenSubtitlesResultData<OpenSubtitlesSubtitleFormats>>("infos/formats");
    }

    /// <summary>
    /// Get the languages table containing the codes and names used through the API<br/>
    /// https://opensubtitles.stoplight.io/docs/opensubtitles-api/1de776d20e873-languages
    /// </summary>
    public Task<List<OpenSubtitlesLanguage>?> GetLanguagesAsync()
    {
        return GetJsonAsync<List<OpenSubtitlesLanguage>>("infos/languages");
    }
    #endregion
}