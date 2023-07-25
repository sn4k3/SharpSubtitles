namespace SharpSubtitlesApi;

public class Language
{
    #region Constants

    /// <summary>
    /// Gets the list of ISO 639-1 languages
    /// </summary>
    public static readonly Language[] List = {
        new("af", "Afrikaans"),
        new("sq", "Albanian"),
        new("ar", "Arabic"),
        new("an", "Aragonese"),
        new("hy", "Armenian"),
        new("at", "Asturian"),
        new("eu", "Basque"),
        new("be", "Belarusian"),
        new("bn", "Bengali"),
        new("bs", "Bosnian"),
        new("br", "Breton"),
        new("bg", "Bulgarian"),
        new("my", "Burmese"),
        new("ca", "Catalan"),
        new("zh-cn", "Chinese (simplified)"),
        new("zh-tw", "Chinese (traditional)"),
        new("ze", "Chinese bilingual"),
        new("hr", "Croatian"),
        new("cs", "Czech"),
        new("da", "Danish"),
        new("nl", "Dutch"),
        new("en", "English"),
        new("eo", "Esperanto"),
        new("et", "Estonian"),
        new("fi", "Finnish"),
        new("fr", "French"),
        new("gl", "Galician"),
        new("ka", "Georgian"),
        new("de", "German"),
        new("el", "Greek"),
        new("he", "Hebrew"),
        new("hi", "Hindi"),
        new("hu", "Hungarian"),
        new("is", "Icelandic"),
        new("id", "Indonesian"),
        new("it", "Italian"),
        new("ja", "Japanese"),
        new("kk", "Kazakh"),
        new("km", "Khmer"),
        new("ko", "Korean"),
        new("lv", "Latvian"),
        new("lt", "Lithuanian"),
        new("lb", "Luxembourgish"),
        new("mk", "Macedonian"),
        new("ms", "Malay"),
        new("ml", "Malayalam"),
        new("ma", "Manipuri"),
        new("mn", "Mongolian"),
        new("me", "Montenegrin"),
        new("se", "Northern Sami"),
        new("no", "Norwegian"),
        new("nb", "Norwegian Bokmal"),
        new("oc", "Occitan"),
        new("fa", "Persian"),
        new("pl", "Polish"),
        new("pt-pt", "Portuguese"),
        new("pt-br", "Portuguese (Brazilian)"),
        new("ro", "Romanian"),
        new("ru", "Russian"),
        new("sr", "Serbian"),
        new("si", "Sinhalese"),
        new("sk", "Slovak"),
        new("sl", "Slovenian"),
        new("es", "Spanish"),
        new("sw", "Swahili"),
        new("sv", "Swedish"),
        new("sy", "Syriac"),
        new("tl", "Tagalog"),
        new("ta", "Tamil"),
        new("te", "Telugu"),
        new("th", "Thai"),
        new("tr", "Turkish"),
        new("uk", "Ukrainian"),
        new("ur", "Urdu"),
        new("uz", "Uzbek"),
        new("vi", "Vietnamese"),
    };
    #endregion

    /// <summary>
    /// Gets the ISO 639-1 short name of the language
    /// </summary>
    public string ShortName { get; }

    /// <summary>
    /// Gets the language name
    /// </summary>
    public string Name { get; }

    public Language(string shortName, string name)
    {
        ShortName = shortName;
        Name = name;
    }

    public override string ToString()
    {
        return $"{ShortName}: {Name}";
    }

    protected bool Equals(Language other)
    {
        return ShortName == other.ShortName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Language)obj);
    }

    public override int GetHashCode()
    {
        return ShortName.GetHashCode();
    }
}