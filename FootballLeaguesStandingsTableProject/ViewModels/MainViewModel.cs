using CommunityToolkit.Mvvm.ComponentModel;

using FootballLeaguesStandingsTableProject.Models;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using RestSharp;

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

	#endregion

	#region Methods

	private void LoadLeagues()
	{
		this.FootballLeaguesList = new ObservableCollection<FootballCountryModel>(this.GetApiData());

		if (this.FootballLeaguesList != null)
		{
			this.SelectedFootballLeague = this.FootballLeaguesList.FirstOrDefault();
		}
	}

    private void RaiseAndSetProp(ref IdNameModel prop)
	{

	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);

		if (e.PropertyName == nameof(SelectedFootballLeague))
		{

		}
	}

	private ObservableCollection<FootballCountryModel> GetApiData()
	{
		#region Api Call 

		//var client = new RestClient("https://v3.football.api-sports.io/{endpoint}");
		//var request = new RestRequest("https://v3.football.api-sports.io/leagues",Method.Get) { RequestFormat = DataFormat.Json };
		//request.AddHeader("x-rapidapi-key", "5030880d82b1e6f3ee3612cb64c53569");
		//request.AddHeader("x-rapidapi-host", "v3.football.api-sports.io");
		//RestResponse response = client.Execute(request);

		//if (response.StatusCode == System.Net.HttpStatusCode.OK)
		//{
		//	var jsonObj = (JObject)JsonConvert.DeserializeObject(response.Content);

		//	if (jsonObj != null)
		//	{
		//		var leagues = jsonObj["response"].SelectMany(x => x["league"], x[""]);
		//	}
		//}

		#endregion End Api Call

		//path for Desktop PC => C:\Users\beckh\source\repos\FootballLeague\JSonLeagues.json
		//path for Laptop => C:\Users\Lyubomir\Source\Repos\FootballLeague\JSonLeagues.json
		var rawLeaguesDataJSON = JsonConvert.DeserializeObject<LeaguesDeserializedModel>(File.ReadAllText(@"C:\Users\beckh\source\repos\FootballLeague\JSonLeagues.json"));
		var leaguesList = new ObservableCollection<FootballCountryModel>();

		foreach (var rawLeagueData in rawLeaguesDataJSON.Response)
		{
			var lg = JsonConvert.DeserializeObject<Results>(rawLeagueData.ToString());
			var leagueData = JsonConvert.DeserializeObject<FootballCountryModel>(lg.JsonResults.ToString());

			if (leagueData != null)
			{
				leaguesList.Add(leagueData);
			}
		}

		return leaguesList;
	}

	#endregion

}
