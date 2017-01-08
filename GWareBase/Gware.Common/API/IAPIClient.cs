using Gware.Common.API.Session;

namespace Gware.Common.API
{
    public interface IAPIClient
    {
        ISessonManager SessionManager { get; }
        bool CanConnect();
        APIConnectionStatus GetConnectionStatus();
        

    }
}