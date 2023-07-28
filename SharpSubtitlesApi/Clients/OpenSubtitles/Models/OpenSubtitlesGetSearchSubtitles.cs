using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models;

public class OpenSubtitlesGetSearchSubtitles
{
	/// <summary>
	/// exclude, include (default: include)
	/// </summary>
	[JsonPropertyName("ai_translated")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType AiTranslated { get; set; } = OpenSubtitlesInclusionType.Include;

	/// <summary>
	/// For Tvshows
	/// </summary>
	[JsonPropertyName("episode_number")]
	public int? EpisodeNumber { get; set; } = null;

	/// <summary>
	/// exclude, include, only (default: include)
	/// </summary>
	[JsonPropertyName("foreign_parts_only")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType ForeignPartsOnly { get; set; } = OpenSubtitlesInclusionType.Include;

	/// <summary>
	/// exclude, include, only (default: include)
	/// </summary>
	[JsonPropertyName("hearing_impaired")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType HearingImpaired { get; set; } = OpenSubtitlesInclusionType.Include;

	/// <summary>
	/// ID of the movie or episode
	/// </summary>
	[JsonPropertyName("id")]
	public int? Id { get; set; } = null;

	/// <summary>
	/// IMDB ID of the movie or episode
	/// </summary>
	[JsonPropertyName("imdb_id")]
	public int? ImdbId { get; set; } = null;

	/// <summary>
	/// Language code(s), comma separated, sorted in alphabetical order (en,fr)
	/// </summary>
	[JsonPropertyName("languages")]
	public List<string> Languages { get; set; } = new();

	/// <summary>
	/// exclude, include (default: exclude)
	/// </summary>
	[JsonPropertyName("machine_translated")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType MachineTranslated { get; set; } = OpenSubtitlesInclusionType.Exclude;

	/// <summary>
	/// Moviehash of the moviefile
	/// </summary>
	[JsonPropertyName("movie_hash")]
	public string? MovieHash { get; set; }

	/// <summary>
	/// include, only (default: include)
	/// </summary>
	[JsonPropertyName("moviehash_match")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType MovieHashMatch { get; set; } = OpenSubtitlesInclusionType.Include;

	/// <summary>
	/// Order of the returned results, accept any of above fields
	/// </summary>
	[JsonPropertyName("order_by")]
	public string? OrderBy { get; set; }

	/// <summary>
	/// Order direction of the returned results (asc,desc)
	/// </summary>
	[JsonPropertyName("OrderDirection")]
	public OpenSubtitlesOrderDirection? OrderDirection { get; set; } = null;

	/// <summary>
	/// Results page to display
	/// </summary>
	[JsonPropertyName("page")]
	public int? Page { get; set; }

	/// <summary>
	/// For Tvshows
	/// </summary>
	[JsonPropertyName("parent_feature_id")]
	public int? ParentFeatureId { get; set; }

	/// <summary>
	/// For Tvshows
	/// </summary>
	[JsonPropertyName("parent_imdb_id")]
	public int? ParentImdbId { get; set; }

	/// <summary>
	/// For Tvshows
	/// </summary>
	[JsonPropertyName("parent_tmdb_id")]
	public int? ParentTmdbId { get; set; }

	/// <summary>
	/// File name or text search
	/// </summary>
	[JsonPropertyName("query")]
	public string? Query { get; set; }

	/// <summary>
	/// For Tvshows
	/// </summary>
	[JsonPropertyName("season_number")]
	public int? SeasonNumber { get; set; }

	/// <summary>
	/// TMDB ID of the movie or episode
	/// </summary>
	[JsonPropertyName("tmdb_id")]
	public int? TmdbId { get; set; }

	/// <summary>
	/// include, only (default: include)
	/// </summary>
	[JsonPropertyName("trusted_sources")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesInclusionType TrustedSources { get; set; } = OpenSubtitlesInclusionType.Include;

	/// <summary>
	/// movie, episode or all, (default: all)
	/// </summary>
	[JsonPropertyName("type")]
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public OpenSubtitlesMovieType Type { get; set; } = OpenSubtitlesMovieType.All;

	/// <summary>
	/// To be used alone - for user uploads listing
	/// </summary>
	[JsonPropertyName("user_id")]
	public int? UserId { get; set; }

	/// <summary>
	/// To be used alone - for user uploads listing
	/// </summary>
	[JsonPropertyName("year")]
	public int? Year { get; set; }

	public override string ToString()
	{
		return $"{nameof(AiTranslated)}: {AiTranslated}, {nameof(EpisodeNumber)}: {EpisodeNumber}, {nameof(ForeignPartsOnly)}: {ForeignPartsOnly}, {nameof(HearingImpaired)}: {HearingImpaired}, {nameof(Id)}: {Id}, {nameof(ImdbId)}: {ImdbId}, {nameof(Languages)}: {Languages}, {nameof(MachineTranslated)}: {MachineTranslated}, {nameof(MovieHash)}: {MovieHash}, {nameof(MovieHashMatch)}: {MovieHashMatch}, {nameof(OrderBy)}: {OrderBy}, {nameof(OrderDirection)}: {OrderDirection}, {nameof(Page)}: {Page}, {nameof(ParentFeatureId)}: {ParentFeatureId}, {nameof(ParentImdbId)}: {ParentImdbId}, {nameof(ParentTmdbId)}: {ParentTmdbId}, {nameof(Query)}: {Query}, {nameof(SeasonNumber)}: {SeasonNumber}, {nameof(TmdbId)}: {TmdbId}, {nameof(TrustedSources)}: {TrustedSources}, {nameof(Type)}: {Type}, {nameof(UserId)}: {UserId}, {nameof(Year)}: {Year}";
	}

	protected bool Equals(OpenSubtitlesGetSearchSubtitles other)
	{
		return AiTranslated == other.AiTranslated && EpisodeNumber == other.EpisodeNumber && ForeignPartsOnly == other.ForeignPartsOnly && HearingImpaired == other.HearingImpaired && Id == other.Id && ImdbId == other.ImdbId && Languages.Equals(other.Languages) && MachineTranslated == other.MachineTranslated && MovieHash == other.MovieHash && MovieHashMatch == other.MovieHashMatch && OrderBy == other.OrderBy && OrderDirection == other.OrderDirection && Page == other.Page && ParentFeatureId == other.ParentFeatureId && ParentImdbId == other.ParentImdbId && ParentTmdbId == other.ParentTmdbId && Query == other.Query && SeasonNumber == other.SeasonNumber && TmdbId == other.TmdbId && TrustedSources == other.TrustedSources && Type == other.Type && UserId == other.UserId && Year == other.Year;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj)) return false;
		if (ReferenceEquals(this, obj)) return true;
		if (obj.GetType() != this.GetType()) return false;
		return Equals((OpenSubtitlesGetSearchSubtitles)obj);
	}

	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		hashCode.Add((int)AiTranslated);
		hashCode.Add(EpisodeNumber);
		hashCode.Add((int)ForeignPartsOnly);
		hashCode.Add((int)HearingImpaired);
		hashCode.Add(Id);
		hashCode.Add(ImdbId);
		hashCode.Add(Languages);
		hashCode.Add((int)MachineTranslated);
		hashCode.Add(MovieHash);
		hashCode.Add((int)MovieHashMatch);
		hashCode.Add(OrderBy);
		hashCode.Add(OrderDirection);
		hashCode.Add(Page);
		hashCode.Add(ParentFeatureId);
		hashCode.Add(ParentImdbId);
		hashCode.Add(ParentTmdbId);
		hashCode.Add(Query);
		hashCode.Add(SeasonNumber);
		hashCode.Add(TmdbId);
		hashCode.Add((int)TrustedSources);
		hashCode.Add((int)Type);
		hashCode.Add(UserId);
		hashCode.Add(Year);
		return hashCode.ToHashCode();
	}
}