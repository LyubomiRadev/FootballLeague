using CommunityToolkit.Mvvm.ComponentModel;

using FootballLeaguesStandingsTableProject.Models;
using FootballLeaguesStandingsTableProject.Views;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using OxyPlot;
using OxyPlot.Series;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace FootballLeaguesStandingsTableProject.ViewModels;
public class ContinentItem
{
	public string? Name { get; set; }

	public double PopulationInMillions { get; set; }

	public bool IsExploded { get; set; }
}
public partial class MainViewModel : ViewModelBase
{

	public MainViewModel()
	{
		this.LoadLeagues();

		Continents = new ObservableCollection<ContinentItem>
		{
			new ContinentItem { Name = "Africa", PopulationInMillions = 1030, IsExploded = true },
			new ContinentItem { Name = "Americas", PopulationInMillions = 929, IsExploded = true },
			new ContinentItem { Name = "Asia", PopulationInMillions = 4157 },
			new ContinentItem { Name = "Europe", PopulationInMillions = 739, IsExploded = true },
			new ContinentItem { Name = "Oceania", PopulationInMillions = 35, IsExploded = true }
		};

	}

	#region Properties

	[ObservableProperty]
	public ObservableCollection<ContinentItem> _continents = new ObservableCollection<ContinentItem>();

	[ObservableProperty]
	public ObservableCollection<FootballCountryModel> _footballLeaguesList = new ObservableCollection<FootballCountryModel>();

	[ObservableProperty]
	public FootballCountryModel _selectedFootballLeague = new FootballCountryModel();

	[ObservableProperty]
	public LeagueModel _leagueData = new LeagueModel();

	[ObservableProperty]
	public ObservableCollection<FootballTeamInfoModel> _standings = new ObservableCollection<FootballTeamInfoModel>();

	[ObservableProperty]
	public FootballTeamInfoModel _selectedTeam = new FootballTeamInfoModel();

	[ObservableProperty]
	public ObservableCollection<PieSliceItemModel> _pieChartData = new ObservableCollection<PieSliceItemModel>();

	[ObservableProperty]
	public string _pieChartTitle = string.Empty;

	[ObservableProperty]
	public PieSeries _slices = new PieSeries();

	[ObservableProperty]
	public PlotModel plotModel = new PlotModel();

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

		if (e.PropertyName == nameof(SelectedFootballLeague))
		{
			var data = new LeagueModel(this.GetLeaugeStandings(leagueId: this.SelectedFootballLeague.ID));
			this.LeagueData = new LeagueModel(data: data,getStandingsCollection: true);
			this.Standings = new ObservableCollection<FootballTeamInfoModel>(this.LeagueData.StandingsCollection);
			this.SelectedTeam = this.Standings.FirstOrDefault();
		}
		else if (e.PropertyName == nameof(SelectedTeam))
		{
			this.PieChartTitle = $"{this.SelectedTeam.TeamData.Name} statistics";
			this.PieChartData = new ObservableCollection<PieSliceItemModel>()
			{
				new PieSliceItemModel{Name = "Won", PercentageValue = this.SelectedTeam.GamesData.GamesWon, IsExploded = true},
				new PieSliceItemModel{Name = "Drawn", PercentageValue = this.SelectedTeam.GamesData.GamesDrawn, IsExploded = true},
				new PieSliceItemModel{Name = "Lost", PercentageValue = this.SelectedTeam.GamesData.GamesLost, IsExploded = true},
			};

			this.PlotModel = new PlotModel() { Title = $"{this.SelectedTeam.TeamData.Name} statistics" };
			this.Slices = new PieSeries() { TextColor = OxyColors.White };
			this.Slices.Slices.Add(new PieSlice("Won", this.SelectedTeam.GamesData.GamesWon) {IsExploded = true, Fill = OxyColors.Green });
			this.Slices.Slices.Add(new PieSlice("Drawn", this.SelectedTeam.GamesData.GamesDrawn) {IsExploded = true, Fill = OxyColors.Ivory });
			this.Slices.Slices.Add(new PieSlice("Lost", this.SelectedTeam.GamesData.GamesLost) {IsExploded = true, Fill = OxyColors.Red });

			this.PlotModel.Series.Add(this.Slices);	
		}
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
		//path for Laptop => D:\Projects\Avalonia Projects\JSonLeagues.json
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
		var rawLeaguesDataJSON = JsonConvert.DeserializeObject<LeagueStandingsModel>(File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\PremierLeague.json"));
		var jsonPL = File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\PremierLeague.json");
		var deserializedData = JObject.Parse(jsonPL)["response"].Select(x => x["league"].ToObject<LeagueModel>()).ToList();
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
		if (deserializedData != null && deserializedData.FirstOrDefault() as LeagueModel != null)
		{
			leagueData = new LeagueModel(deserializedData.FirstOrDefault());
		}
		else
		{
		  leagueData = new LeagueModel();
		}
		//var client = new RestClient("https://v3.football.api-sports.io/{endpoint}");
		//var request = new RestRequest($"https://v3.football.api-sports.io/standings?league={leagueId}&season={DateTime.Now.Year}", Method.Get) { RequestFormat = DataFormat.Json };

		//request.AddHeader("x-rapidapi-key", "5030880d82b1e6f3ee3612cb64c53569");//key 1 - 5030880d82b1e6f3ee3612cb64c53569 | key 2 - e09dc70c29b92c46a363d3b5bb3d42ee
		//request.AddHeader("x-rapidapi-host", "v3.football.api-sports.io");

		////call the API
		//RestResponse response = client.Execute(request);

		////if the API call is good parse the date from the API and place it in the returnig collection
		//if (response.StatusCode == System.Net.HttpStatusCode.OK)
		//{
		//	//	leaguesList = new ObservableCollection<FootballCountryModel>(JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<FootballCountryModel>()).ToList());
		//	 leagueData = JObject.Parse(response.Content)["response"].Select(x => x["league"].ToObject<LeagueModel>()).FirstOrDefault();
		//}

		return leagueData;
	}
	#endregion End GetLeaugeStandings

	#endregion

}

public class PieSliceItemModel
{
    public string Name { get; set; }

    public double PercentageValue { get; set; }

    public bool IsExploded { get; set; }
}
