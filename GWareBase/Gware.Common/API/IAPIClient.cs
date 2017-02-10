namespace Gware.Common.API
{
    public interface IAPIClient 
    {
        bool CanConnect();
        APIConnectionStatus GetConnectionStatus();
    }
}