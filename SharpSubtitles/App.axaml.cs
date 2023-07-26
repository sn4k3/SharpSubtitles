using System.Diagnostics;
using System.Text.Json;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using SharpSubtitles.UI.ViewModels;
using SharpSubtitles.UI.Views;
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
        var ss = client.GetSubtitleFormatsAsync().Result;
        var login = client.LoginAsync(new OpenSubtitlesPostUserLogin()).Result;

        if (login is not null) client.AuthToken = login.Token;

        var lsubs = client.GetMostDownloadedSubtitlesAsync(OpenSubtitlesEpisodeTypes.Movie, "en").Result;
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
