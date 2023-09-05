using Newtonsoft.Json;

using System.Collections.Generic;

namespace FootballLeaguesStandingsTableProject.Models
{
	public class FootballCountryModel
	{
        [JsonProperty("id")]
        public int ID { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("logo")]
		public string Logo { get; set; }



    }

	public class Results
	{

        [JsonProperty("league")]
        public object JsonResults { get; set; }
    }

	public class JsonRootObject
	{
        [JsonProperty("response")]
        public Results Rslt { get; set; }
    }
}
