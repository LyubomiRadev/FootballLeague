using CommunityToolkit.Mvvm.ComponentModel;

using FootballLeaguesStandingsTableProject.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace FootballLeaguesStandingsTableProject.ViewModels;

public partial class MainViewModel : ViewModelBase
{

	public MainViewModel()
	{
		this.LoadLeagues();
	}

	#region Properties

	[ObservableProperty]
	public ObservableCollection<FootballCountryModel> _footballLeaguesList = new ObservableCollection<FootballCountryModel>();

	[ObservableProperty]
	public FootballCountryModel _selectedFootballLeague = new FootballCountryModel();

	[ObservableProperty]
	public LeagueModel _leagueData = new LeagueModel();

	#endregion

	#region Methods

	private void LoadLeagues()
	{
		this.FootballLeaguesList = new ObservableCollection<FootballCountryModel>(this.GetApiData());

		if (this.FootballLeaguesList != null)
		{
			this.SelectedFootballLeague = this.FootballLeaguesList.Where(w => w.ID == 39).FirstOrDefault();
		}
	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);

		//if (e.PropertyName == nameof(SelectedFootballLeague))
		//{
		//	var data = new LeagueModel(this.GetLeaugeStandings(leagueId: this.SelectedFootballLeague.ID));
		//	this.LeagueData = new LeagueModel(data);
		//}
	}

	#region Get API Data

	private ObservableCollection<FootballCountryModel> GetApiData()
	{
		#region API Call 

		////setting the API
		//var client = new RestClient("https://v3.football.api-sports.io/{endpoint}");
		//var request = new RestRequest("https://v3.football.api-sports.io/leagues", Method.Get) { RequestFormat = DataFormat.Json };

		//request.AddHeader("x-rapidapi-key", "5030880d82b1e6f3ee3612cb64c53569");
		//request.AddHeader("x-rapidapi-host", "v3.football.api-sports.io");

		////call the API
		//RestResponse response = client.Execute(request);

		var leaguesList = new ObservableCollection<FootballCountryModel>();

		////if the API call is good parse the date from the API and place it in the returnig collection
		//if (response.StatusCode == System.Net.HttpStatusCode.OK)
		//{
		//	leaguesList = new ObservableCollection<FootballCountryModel>(JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<FootballCountryModel>()).ToList());
		//}

		#endregion End Api Call

		#region Depricated JSON PARSING

		//path for Desktop PC => C:\Users\beckh\source\repos\FootballLeague\JSonLeagues.json
		//path for Laptop => C:\Users\Lyubomir\Source\Repos\FootballLeague\JSonLeagues.json
		var rawLeaguesDataJSON = JsonConvert.DeserializeObject<LeaguesDeserializedModel>(File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\JSonLeagues.json"));
		leaguesList = new ObservableCollection<FootballCountryModel>();

		foreach (var rawLeagueData in rawLeaguesDataJSON.Response)
		{
			//leaguesList = new ObservableCollection<FootballCountryModel>(JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<FootballCountryModel>()).ToList());
			var lg = JsonConvert.DeserializeObject<Results>(rawLeagueData.ToString());
			var leagueData = JsonConvert.DeserializeObject<FootballCountryModel>(lg.JsonResults.ToString());

			if (leagueData != null)
			{
				leaguesList.Add(leagueData);
			}
		}

		#endregion End Depricated JSON PARSING

		return leaguesList;
	}

	#endregion End Get API Data

	#region GetLeaugeStandings

	public LeagueModel GetLeaugeStandings(int leagueId)
	{
		//var rawLeaguesDataJSON = JsonConvert.DeserializeObject<LeagueStandingsModel>(File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\PremierLeague.json"));
		//var jsonPL = File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\PremierLeague.json");
		//var deserializedData = JObject.Parse(jsonPL)["response"].Select(x => x["league"].ToObject<LeagueModel>()).ToList();
		//if (rawLeaguesDataJSON != null)
		//{
		//	foreach (var team in rawLeaguesDataJSON.Response)
		//	{
		//		var lg = JsonConvert.DeserializeObject<League>(team.ToString());
		//		var standings = JsonConvert.DeserializeObject<Standings>(lg.LeagueData.ToString());
		//		var stnds = JsonConvert.DeserializeObject<FootballTeamInfoModel>(standings.StandingsData.ToString());
		//	}
		//}
		var leagueData = new LeagueModel();
		var client = new RestClient("https://v3.football.api-sports.io/{endpoint}");
		var request = new RestRequest($"https://v3.football.api-sports.io/standings?league={leagueId}&season={DateTime.Now.Year}", Method.Get) { RequestFormat = DataFormat.Json };

		request.AddHeader("x-rapidapi-key", "5030880d82b1e6f3ee3612cb64c53569");
		request.AddHeader("x-rapidapi-host", "v3.football.api-sports.io");

		//call the API
		RestResponse response = client.Execute(request);

		//if the API call is good parse the date from the API and place it in the returnig collection
		if (response.StatusCode == System.Net.HttpStatusCode.OK)
		{
			//	leaguesList = new ObservableCollection<FootballCountryModel>(JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<FootballCountryModel>()).ToList());
			 leagueData = JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<LeagueModel>()).FirstOrDefault();
		}

		return leagueData;
	}
	#endregion End GetLeaugeStandings

	#endregion

}
