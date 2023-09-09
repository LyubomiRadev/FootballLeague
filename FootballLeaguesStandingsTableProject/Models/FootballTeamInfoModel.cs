using Newtonsoft.Json;

using System.Collections.Generic;

namespace FootballLeaguesStandingsTableProject.Models
{
	public class League
	{
		[JsonProperty("league")]
		public object LeagueData { get; set; }
	}

	public class Standings
	{
		[JsonProperty("standings")]
		public List<object> StandingsData { get; set; }
	}
}

public class Standings1
{
	[JsonProperty("standings")]
	public List<FootballTeamInfoModel>? LeagueTable { get; set; }
}



public class LeagueModel
{
    public LeagueModel()
    {
        
    }

    public LeagueModel(LeagueModel data)
    {
        this.Id = data.Id;
		this.Name = data.Name;
		this.Country = data.Country;
		this.CountryFlag = data.CountryFlag;
		this.Logo = data.Logo;
		this.Standings = data.Standings;
    }

    [JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }

	[JsonProperty("country")]
	public string? Country { get; set; }

	[JsonProperty("logo")]
	public string? Logo { get; set; }

	[JsonProperty("flag")]
	public string? CountryFlag { get; set; }

	[JsonProperty("standings")]
	public List<List<FootballTeamInfoModel>> Standings { get; set; }
}

public class FootballTeamInfoModel
{
	[JsonProperty("team")]
	public FootballTeamModel? TeamData { get; set; }

	[JsonProperty("rank")]
	public int LeaguePosition { get; set; }

	[JsonProperty("points")]
	public int Points { get; set; }

}

public class FootballTeamModel
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("name")]
	public string? Name { get; set; }

	[JsonProperty("logo")]
	public string? Logo { get; set; }
}


