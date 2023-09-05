using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeaguesStandingsTableProject.Models
{
	public class LeaguesDeserializedModel
	{
		[JsonProperty("parameters")]
		public object Parameters { get; set; }

		[JsonProperty("errors")]
		public object Errors { get; set; }

		[JsonProperty("results")]
		public int Results { get; set; }


		[JsonProperty("paging")]
		public object Pagin { get; set; }

		[JsonProperty("response")]
		public List<FootballCountryModel> Response { get; set; }
	}
}
