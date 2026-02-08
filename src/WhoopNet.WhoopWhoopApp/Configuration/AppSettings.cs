namespace WhoopNet.WhoopWhoopApp.Configuration;

public class AppSettings
{
    public AuthUnit Auth { get; set; }

    public class AuthUnit
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
