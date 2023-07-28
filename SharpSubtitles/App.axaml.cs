using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SharpSubtitles.UI.ViewModels;
using SharpSubtitles.UI.Views;
using SharpSubtitlesApi.Clients;
using SharpSubtitlesApi.Clients.OpenSubtitles;
using SharpSubtitlesApi.Clients.OpenSubtitles.Models;

namespace SharpSubtitles;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);


        var client = new OpenSubtitlesClient("HH9Pkd6OWfi8sjwatEe5KyB5abXC2vQC");
        client.AutoWaitForRequestLimit = true;
		var ss = client.GetSubtitleFormatsAsync().Result;
        var login = client.LoginAsync(new OpenSubtitlesPostUserLogin()).Result;

        if (login?.Token is not null) client.AuthToken = login.Token;

        var lsubs = client.GetMostDownloadedSubtitlesAsync(OpenSubtitlesEpisodeType.Movie, "en").Result;
        var lsubs2 = client.GetLatestSubtitlesAsync(OpenSubtitlesMovieType.All, "en").Result;
        var lsubs3 = client.GetLatestSubtitlesAsync(OpenSubtitlesMovieType.All, "en").Result;
        var lsubs4 = client.GetLatestSubtitlesAsync(OpenSubtitlesMovieType.All, "en").Result;
        var lsubs5 = client.GetLatestSubtitlesAsync(OpenSubtitlesMovieType.All, "en").Result;
        var guessit = client.GuessItAsync("Cruella.2021.2160p.DSNP.WEB-DL.x265.10bit.HDR.DDP5.1.Atmos-CM").Result;
        var guessiTront = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;
        var asd = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;
        var asd2 = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;
        var asd3 = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;
        var asd4 = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;
        var asd5 = client.GuessItAsync("TRON - Legacy (2010) (1080p BluRay x265 10bit DTS Tigole)").Result;


        var data = new OpenSubtitlesGetSearchSubtitles
        {
	        Query = "Cruella.2021.2160p.DSNP.WEB-DL.x265.10bit.HDR.DDP5.1.Atmos-CM",
	        Languages = new List<string>
	        {
		        "en", "pt"
	        },
	        Type = OpenSubtitlesMovieType.Movie
        };

		var subs = client.SearchSubtitlesAsync(data).Result;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
