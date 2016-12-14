namespace Gware.Common.API
{
    public interface IAPIClient
    {
        UserAuthenticationKey AuthenticationKey { get; }
        bool IsAuthenticated { get; }
        string Password { get; set; }
        int UserID { get; }
        string Username { get; set; }

        int Authenticate(string username,string password);
        bool CheckAuthenticationKey(int retry = 3);
        bool UnAuthenticate();
        bool CanConnect();

        APIConnectionStatus GetConnectionStatus();
    }
}