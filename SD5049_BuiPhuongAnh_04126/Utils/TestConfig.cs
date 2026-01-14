namespace Unsplash.Automation.Tests.Utils;

public static class TestConfig
{
    public static string Email => "phuonganhbui102@gmail.com";
    public static string Password => "anhbp1211";

    // // From https://unsplash.com/documentation
    public static string AccessToken => "YOUR_ACCESS_TOKEN";

    // username of the test account
    public static string Username => "anhbp1211";
    // Default wait timeout (seconds) used across pages/tests
    public static double DefaultTimeoutSeconds => 15;
    // Default pause (milliseconds) used for Thread.Sleep replacements
    public static int DefaultPauseMilliseconds => 5000;

    // Centralized pause helper so test authors can control sleep durations from one place.
    // Prefer explicit waits over Pause() when possible; this exists for unavoidable timing issues.
    public static void Pause(int? milliseconds = null)
    {
        System.Threading.Thread.Sleep(milliseconds ?? DefaultPauseMilliseconds);
    }
}
