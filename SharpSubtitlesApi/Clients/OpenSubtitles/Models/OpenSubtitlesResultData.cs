﻿using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

public class OpenSubtitlesResultData<T> where T : class
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}