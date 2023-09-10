using Newtonsoft.Json;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FootballLeaguesStandingsTableProject.Models
{
	public class League
	{
		[JsonProperty("league")]
		public object? LeagueData { get; set; }
	}

	public class Standings
	{
		[JsonProperty("standings")]
		public List<object>? StandingsData { get; set; }
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
		if (data != null)
		{
			this.Id = data.Id;
			this.Name = data.Name;
			this.Country = data.Country;
			this.CountryFlag = data.CountryFlag;
			this.Logo = data.Logo;
			this.Standings = data.Standings;
		}
    }

	public LeagueModel(LeagueModel data, bool getStandingsCollection)
	{
		if (data != null)
		{
			this.Id = data.Id;
			this.Name = data.Name;
			this.Country = data.Country;
			this.CountryFlag = data.CountryFlag;
			this.Logo = data.Logo;

			if (data.Standings != null && data.Standings.Count() > 0)
			{
				this.StandingsCollection = new ObservableCollection<FootballTeamInfoModel>(data.Standings.FirstOrDefault());
			}
		}
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
	public List<List<FootballTeamInfoModel>>? Standings { get; set; }

    public ObservableCollection<FootballTeamInfoModel>? StandingsCollection { get; set; }
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


