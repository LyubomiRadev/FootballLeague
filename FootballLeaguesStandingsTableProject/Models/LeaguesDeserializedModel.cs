using Newtonsoft.Json;

using System.Collections.Generic;

namespace FootballLeaguesStandingsTableProject.Models
{
	public class LeaguesDeserializedModel
	{
		[JsonProperty("parameters")]
		public object? Parameters { get; set; }

		[JsonProperty("errors")]
		public object? Errors { get; set; }

		[JsonProperty("results")]
		public int Results { get; set; }


		[JsonProperty("paging")]
		public object? Pagin { get; set; }

		[JsonProperty("response")]
		public List<object>? Response { get; set; }
	}

	public class LeagueStandingsModel
	{
		[JsonProperty("parameters")]
		public object? Parameters { get; set; }

		[JsonProperty("errors")]
		public object? Errors { get; set; }

		[JsonProperty("results")]
		public object? Results { get; set; }


		[JsonProperty("paging")]
		public object? Pagin { get; set; }

		[JsonProperty("response")]
		public List<object>? Response { get; set; }
	}
}
