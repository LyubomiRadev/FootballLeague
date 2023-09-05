using CommunityToolkit.Mvvm.ComponentModel;

using FootballLeaguesStandingsTableProject.Models;

using Newtonsoft.Json;

using RestSharp;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FootballLeaguesStandingsTableProject.ViewModels;

public partial class MainViewModel : ViewModelBase
{

    public MainViewModel()
    {
		this.LoadTransitons();
		this.GetApiData();
	}

	#region Properties

	[ObservableProperty]
	public ObservableCollection<IdNameModel> _transitionsList = new ObservableCollection<IdNameModel>();

	[ObservableProperty]
	public IdNameModel _selectedTransition = new IdNameModel();

	#endregion

	#region Methods

	private void LoadTransitons()
	{

		var none = new IdNameModel() { Id = 1, Name = "None" };

		var one = new IdNameModel() { Name = "one", Id = 2 };

		var two = new IdNameModel() { Name = "two", Id = 3 };

		var three = new IdNameModel() { Name = "three", Id = 4 };

		this.TransitionsList = new ObservableCollection<IdNameModel>() { none, one, two, three };

		this.SelectedTransition = this.TransitionsList.FirstOrDefault();
	}

    private void RaiseAndSetProp(ref IdNameModel prop)
	{

	}

	protected override void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		base.OnPropertyChanged(e);

		if (e.PropertyName == nameof(SelectedTransition))
		{

		}
	}

	private void GetApiData()
	{
		var client = new RestClient("https://v3.football.api-sports.io/{endpoint}");
		var request = new RestRequest("https://v3.football.api-sports.io/leagues",Method.Get) { RequestFormat = DataFormat.Json };
		request.AddHeader("x-rapidapi-key", "5030880d82b1e6f3ee3612cb64c53569");
		request.AddHeader("x-rapidapi-host", "v3.football.api-sports.io");
		RestResponse response = client.Execute(request);

		if (response.StatusCode == System.Net.HttpStatusCode.OK)
		{
			dynamic jsonObj = JsonConvert.DeserializeObject<Results>(response.Content);
		}
	}

	#endregion

}
