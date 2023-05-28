namespace WeatherApp;

public partial class MainPage : ContentPage
{
	int count = 0;
	WeatherData data = null;
	WeatherAPI api = new WeatherAPI();

	public MainPage()
	{
		InitializeComponent();
	}

	//private void OnCounterClicked(object sender, EventArgs e)
	//{
	//	count++;

	//	if (count == 1)
	//		CounterBtn.Text = $"Clicked {count} time";
	//	else
	//		CounterBtn.Text = $"Clicked {count} times";

	//	SemanticScreenReader.Announce(CounterBtn.Text);
	//}

	private void OnRetrieveClicked(object sender, EventArgs e)
	{
		//Thread t = new Thread(api.StartClient(ZipEntry.Text));
		Task.Run(GetTemp);
	}

	private async Task GetTemp()
	{
		data = api.StartClient(ZipEntry.Text);
		Application.Current.MainPage.Dispatcher.Dispatch(() => WeatherText.Text = "The Current Temperature is " + data.current.Temp_f + " degrees.");
		//WeatherText.Text = "The Current Temperature is " + data.current.Temp_f + " degrees.";
    }
}

