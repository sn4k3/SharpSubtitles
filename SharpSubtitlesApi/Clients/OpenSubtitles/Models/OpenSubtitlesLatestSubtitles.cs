using System;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

/// <summary>
/// Lists 60 latest uploaded subtitles<br/>
/// https://opensubtitles.stoplight.io/docs/opensubtitles-api/f36cef28efaa9-latest-subtitles
/// </summary>
public class OpenSubtitlesLatestSubtitles
{
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("data")]
    public OpenSubtitlesLatestSubtitlesData[] Data { get; set; } = Array.Empty<OpenSubtitlesLatestSubtitlesData>();

    public override string ToString()
    {
        return $"{nameof(TotalPages)}: {TotalPages}, {nameof(TotalCount)}: {TotalCount}, {nameof(Page)}: {Page}, {nameof(Data)}: {Data}";
    }

    public class OpenSubtitlesLatestSubtitlesData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("attributes")]
        public OpenSubtitlesLatestSubtitlesAttributes Attributes { get; set; } = new();

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Type)}: {Type}, {nameof(Attributes)}: {Attributes}";
        }
    }

    public class OpenSubtitlesLatestSubtitlesAttributes
    {
        [JsonPropertyName("subtitle_id")]
        public string SubtitleId { get; set; } = string.Empty;

        [JsonPropertyName("language")]
        public string Language { get; set; } = string.Empty;

        [JsonPropertyName("download_count")]
        public int DownloadCount { get; set; }

        [JsonPropertyName("new_download_count")]
        public int NewDownloadCount { get; set; }

        [JsonPropertyName("hearing_impaired")]
        public bool HearingImpaired { get; set; }

        [JsonPropertyName("hd")]
        public bool Hd { get; set; }

        [JsonPropertyName("fps")]
        public decimal Fps { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("ratings")]
        public decimal Ratings { get; set; }

        [JsonPropertyName("from_trusted")]
        public bool? FromTrusted { get; set; }

        [JsonPropertyName("foreign_parts_only")]
        public bool ForeignPartsOnly { get; set; }

        [JsonPropertyName("upload_date")]
        public DateTime UploadDate { get; set; }

        [JsonPropertyName("ai_translated")]
        public bool AiTranslated { get; set; }

        [JsonPropertyName("machine_translated")]
        public bool MachineTranslated { get; set; }

        [JsonPropertyName("release")]
        public string Release { get; set; } = string.Empty;

        [JsonPropertyName("comments")]
        public string Comments { get; set; } = string.Empty;

        [JsonPropertyName("legacy_subtitle_id")]
        public int? LegacySubtitleId { get; set; }

        [JsonPropertyName("uploader")]
        public OpenSubtitlesLatestSubtitlesUploader Uploader { get; set; } = new();

        [JsonPropertyName("feature_details")]
        public OpenSubtitlesLatestSubtitlesFeatureDetails FeatureDetails { get; set; } = new();

        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        [JsonPropertyName("related_links")]
        public OpenSubtitlesLatestSubtitlesRelatedLinks[] Title { get; set; } = Array.Empty<OpenSubtitlesLatestSubtitlesRelatedLinks>();

        [JsonPropertyName("files")]
        public OpenSubtitlesLatestSubtitlesFiles[] Files { get; set; } = Array.Empty<OpenSubtitlesLatestSubtitlesFiles>();

        public override string ToString()
        {
            return $"{nameof(SubtitleId)}: {SubtitleId}, {nameof(Language)}: {Language}, {nameof(DownloadCount)}: {DownloadCount}, {nameof(NewDownloadCount)}: {NewDownloadCount}, {nameof(HearingImpaired)}: {HearingImpaired}, {nameof(Hd)}: {Hd}, {nameof(Fps)}: {Fps}, {nameof(Votes)}: {Votes}, {nameof(Ratings)}: {Ratings}, {nameof(FromTrusted)}: {FromTrusted}, {nameof(ForeignPartsOnly)}: {ForeignPartsOnly}, {nameof(UploadDate)}: {UploadDate}, {nameof(AiTranslated)}: {AiTranslated}, {nameof(MachineTranslated)}: {MachineTranslated}, {nameof(Release)}: {Release}, {nameof(Comments)}: {Comments}, {nameof(LegacySubtitleId)}: {LegacySubtitleId}, {nameof(Uploader)}: {Uploader}, {nameof(FeatureDetails)}: {FeatureDetails}, {nameof(Url)}: {Url}, {nameof(Title)}: {Title}, {nameof(Files)}: {Files}";
        }
    }

    public class OpenSubtitlesLatestSubtitlesUploader
    {
        [JsonPropertyName("uploader_id")]
        public int? UploaderId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("rank")]
        public string Rank { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{nameof(UploaderId)}: {UploaderId}, {nameof(Name)}: {Name}, {nameof(Rank)}: {Rank}";
        }
    }

    public class OpenSubtitlesLatestSubtitlesFeatureDetails
    {
        [JsonPropertyName("feature_id")]
        public int FeatureId { get; set; }

        [JsonPropertyName("feature_type")]
        public string FeatureType { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("movie_name")]
        public string MovieName { get; set; } = string.Empty;

        [JsonPropertyName("imdb_id")]
        public int ImdbId { get; set; }

        [JsonPropertyName("tmdb_id")]
        public int TmdbId { get; set; }

        public override string ToString()
        {
            return $"{nameof(FeatureId)}: {FeatureId}, {nameof(FeatureType)}: {FeatureType}, {nameof(Year)}: {Year}, {nameof(Title)}: {Title}, {nameof(MovieName)}: {MovieName}, {nameof(ImdbId)}: {ImdbId}, {nameof(TmdbId)}: {TmdbId}";
        }
    }

    public class OpenSubtitlesLatestSubtitlesRelatedLinks
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public Uri? Url { get; set; }

        [JsonPropertyName("img_url")]
        public Uri? ImgUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Label)}: {Label}, {nameof(Url)}: {Url}, {nameof(ImgUrl)}: {ImgUrl}";
        }
    }

    public class OpenSubtitlesLatestSubtitlesFiles
    {
        [JsonPropertyName("file_id")]
        public int FileId { get; set; }

        [JsonPropertyName("cd_number")]
        public int CdNumber { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{nameof(FileId)}: {FileId}, {nameof(CdNumber)}: {CdNumber}, {nameof(FileName)}: {FileName}";
        }
    }
}