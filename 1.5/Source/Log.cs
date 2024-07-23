namespace AADMod;

public static class Log
{
    private const string color = "#1c6beb";

    private const string mod = "AADMod";

    public static string Prefix => $"<color={color}>[{mod}] ";

    public static string Suffix => "</color>";

    public static bool OpenOnMessage
    {
        get => Verse.Log.openOnMessage;
        set => Verse.Log.openOnMessage = value;
    }

    public static void Message(string text)
    {
        Verse.Log.Message($"{Prefix}{text}{Suffix}");
    }

    public static void Message(object obj)
    {
        Message(obj.ToString());
    }

    public static void Warning(string text)
    {
        Verse.Log.Warning($"{Prefix}{text}{Suffix}");
    }

    public static void WarningOnce(string text, int key)
    {
        Verse.Log.WarningOnce($"{Prefix}{text}{Suffix}", key);
    }

    public static void Error(string text)
    {
        Verse.Log.Error($"{Prefix}{text}{Suffix}");
    }

    public static void ErrorOnce(string text, int key)
    {
        Verse.Log.ErrorOnce($"{Prefix}{text}{Suffix}", key);
    }

    public static void Clear()
    {
        Verse.Log.Clear();
    }

    public static void TryOpenLogWindow()
    {
        Verse.Log.TryOpenLogWindow();
    }
}
