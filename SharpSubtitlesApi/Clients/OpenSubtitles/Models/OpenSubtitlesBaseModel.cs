using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Clients.OpenSubtitles.Models
{
	public class OpenSubtitlesBaseModel
	{
		[JsonPropertyName("errors")]
		public string[]? Errors { get; set; }

		[JsonPropertyName("status")]
		public int? Status { get; set; }

		[JsonPropertyName("message")]
		public string? Message { get; set; }

		public override string ToString()
		{
			return $"{nameof(Errors)}: {Errors}, {nameof(Status)}: {Status}, {nameof(Message)}: {Message}";
		}
	}
}
