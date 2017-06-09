namespace Gware.Common.Client
{
    public interface IClient 
    {
        bool CanConnect();
        ClientConnectionStatus GetConnectionStatus();
    }
}