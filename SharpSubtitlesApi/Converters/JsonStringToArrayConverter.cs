using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharpSubtitlesApi.Converters
{
	public class JsonStringToArrayConverter : JsonConverter<string[]?>
	{
		public override string[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
			{
				return null;
			}
			if (reader.TokenType == JsonTokenType.StartArray)
			{
				return JsonSerializer.Deserialize<string[]>(ref reader, options);
			}

			if (reader.TokenType == JsonTokenType.String)
			{
				return new[] { JsonSerializer.Deserialize<string>(ref reader, options)! };
			}

			return new[] { reader.GetString()! };
		}

		public override void Write(Utf8JsonWriter writer, string[]? values, JsonSerializerOptions options)
		{
			
			if (values is null)
			{
				writer.WriteNullValue();
				return;
			}

			writer.WriteStartArray();

			if (values.Length == 1)
			{
				writer.WriteStringValue(values[0]);
			}
			else
			{
				foreach (var value in values)
				{
					writer.WriteStringValue(value);
				}
			}
			writer.WriteEndArray();
		}
	}
}
